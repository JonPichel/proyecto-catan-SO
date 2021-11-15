#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h> 
#include <netinet/in.h> 
#include <pthread.h>

#include "peticiones.h"
#include "base_datos.h"
#include "estructuras.h"
#include "util.h"

listaconn_t conectados;
pthread_mutex_t mutex_estructuras = PTHREAD_MUTEX_INITIALIZER;

void *atender_cliente(void *socket);

int main(int argc, char *argv[]) {
    int sock_listen, sock_conn;
    struct sockaddr_in host_addr;
    int puerto;

    conectados.num = 0;

    if (argc < 3) {
        printf("Uso: %s <PORT> <MODO>\n", argv[0]);
        return -1;
    } else {
        puerto = atoi(argv[1]);
        if (puerto == 0) {
            printf("Numero de puerto invalido: %s\n", argv[1]);
            return -1;
        } else {
            log_msg("MAIN", "Corriendo en el puerto #%d\n", puerto);
        }
        if (strcmp(argv[2], "DESARROLLO") == 0) {
            if (bdd_inicializar("localhost") != 0)
                return -1;
        } else if (strcmp(argv[2], "PRODUCCION") == 0) {
            if (bdd_inicializar("shiva2.upc.es") != 0)
                return -1;
        } else {
            printf("Modos disponibles: DESARROLLO, PRODUCCION\n");
            return -1;
        }
    }

    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        log_msg("MAIN", "Error creando el socket de escucha\n");
        return -1;
    }

	memset(&host_addr, 0, sizeof(host_addr));

	host_addr.sin_family = AF_INET;
    // Asocia el socket a cualquier IP de la maquina
	host_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	host_addr.sin_port = htons(puerto);

    // Hacemos bind al socket
	if (bind(sock_listen, (struct sockaddr *)&host_addr, sizeof(host_addr)) < 0) {
        log_msg("MAIN", "Error en el bind\n");
        return -1;
    }

	if (listen(sock_listen, MAX_CONN) < 0) {
        log_msg("MAIN", "Error en el listen\n");
        return -1;
    }
    
    int sockets[MAX_CONN];
    pthread_t threads[MAX_CONN];
    for (int i = 0; i < MAX_CONN; i++) {
        log_msg("MAIN", "Escuchando...\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        log_msg("MAIN", "Conexion establecida\n");
        
        sockets[i] = sock_conn;
        pthread_create(&threads[i], NULL, atender_cliente, &sockets[i]);
    }
    log_msg("MAIN", "Nº maximo de conexiones alcanzado\n");
    for (int i = 0; i < MAX_CONN; i++) {
        pthread_join(threads[i], NULL);
    }
    bdd_terminar();
}

void *atender_cliente(void *socket) {
    int sock_conn = *(int *)socket;
    char peticion[512], respuesta[512];
    char tag[32]; 
    char *p;
    int nbytes, codigo;
    int actualizar;

    sprintf(tag, "THREAD %d", sock_conn);
    
    while (1) {
        nbytes = read(sock_conn, peticion, sizeof(peticion));
        peticion[nbytes] = '\0';
        log_msg(tag, "Peticion recibida: %s\n", peticion);
        
        p = strtok(peticion, "/");
        codigo = atoi(p);
        
        if (codigo == 0) {
            /* CERRAR CONEXION */
            pthread_mutex_lock(&mutex_estructuras);
            conn_delete_jugador(&conectados, sock_conn);
            pthread_mutex_unlock(&mutex_estructuras);
            pet_lista_conectados(&conectados, respuesta);
            for (int i = 0; i < conectados.num; i++) {
                log_msg(tag, "Lista de conectados por el socket %d: %s\n",
                        conectados.conectados[i].socket, respuesta);
                write(conectados.conectados[i].socket, respuesta, strlen(respuesta));
            }
            break;
        }
        
        char nombre[20], pass[20];
        int idJ, idP;
        actualizar = 0;
        switch (codigo) {
            case 1:
                /* REGISTRO DE JUGADOR */
                p = strtok(NULL, ",");
                strncpy(nombre, p, sizeof(nombre));
                p = strtok(NULL, ",");
                strncpy(pass, p, sizeof(pass));
                pet_registrar_jugador(nombre, pass, respuesta);
                break;
            case 2:
                /* INICIO SESION DE JUGADOR */
                p = strtok(NULL, ",");
                strncpy(nombre, p, sizeof(nombre));
                p = strtok(NULL, ",");
                strncpy(pass, p, sizeof(pass));
                pet_iniciar_sesion(nombre, pass, respuesta);
                if (strcmp(respuesta, "-1") != 0) {
                    pthread_mutex_lock(&mutex_estructuras);
                    conn_add_jugador(&conectados, nombre, sock_conn);
                    pthread_mutex_unlock(&mutex_estructuras);
                    actualizar = 1;
                }
                break;
            case 3:
                /* PARTIDAS DE JUGADOR */
                p = strtok(NULL, ",");
                idJ = atoi(p);
                pet_informacion_partidas_jugador(idJ, respuesta);
                break;
            case 4:
                /* INFORMACION DE PARTIDA */
                p = strtok(NULL, ",");
                idP = atoi(p);
                pet_informacion_partida(idP, respuesta);
                break;
            case 5:
                /* PUNTUACION MEDIA DE JUGADOR */
                p = strtok(NULL, ",");
                idJ = atoi(p);
                pet_puntuacion_media_jugador(idJ, respuesta);
                break;
            case 6:
                /* LISTA DE CONECTADOS */
                pet_lista_conectados(&conectados, respuesta);
                break;
            default:
                log_msg(tag, "Peticion desconocida: %d\n", codigo);
                close(sock_conn);
                return NULL;
        }
        log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
        write(sock_conn, respuesta, strlen(respuesta));
        if (actualizar) {
            sleep(1);
            pet_lista_conectados(&conectados, respuesta);
            for (int i = 0; i < conectados.num; i++) {
                log_msg(tag, "Lista de conectados por el socket %d: %s\n",
                        conectados.conectados[i].socket, respuesta);
                write(conectados.conectados[i].socket, respuesta, strlen(respuesta));
            }
        }
    }
        
    log_msg(tag, "Cerrando conexion...\n");
    close(sock_conn);
}
