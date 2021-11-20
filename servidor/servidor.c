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

/* Variables globales */
listaconn_t conectados;
partida_t partidas[MAX_PART];
pthread_mutex_t mutex_estructuras = PTHREAD_MUTEX_INITIALIZER;

void *atender_cliente(void *socket);

int main(int argc, char *argv[]) {
    int sock_listen, sock_conn;
    struct sockaddr_in host_addr;
    int puerto;

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

    conectados.num = 0;
    inicializar_partidas(partidas);
    
    int sockets[MAX_CONN];
    pthread_t threads[MAX_CONN];
    int i = 0;
    for (;;) {
        log_msg("MAIN", "Escuchando...\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        log_msg("MAIN", "Conexion establecida\n");
        
        sockets[i] = sock_conn;
        pthread_create(&threads[i], NULL, atender_cliente, &sockets[i]);
        i++;
        if (i == MAX_CONN) {
            i = 0;
        }
    }
    bdd_terminar();
}

void *atender_cliente(void *sock_ptr) {
    int socket = *(int *)sock_ptr;
    char nombre[20];
    char peticion[512], tag[32];
    int nbytes, codigo = 1;
    char *resto;

    sprintf(tag, "THREAD %d", socket);
    
    while (codigo != 0) {
        nbytes = read(socket, peticion, sizeof(peticion));
        peticion[nbytes] = '\0';
        log_msg(tag, "Peticion recibida: %s\n", peticion);
        
        codigo = atoi(strtok_r(peticion, "/", &resto));
        
        switch (codigo) {
            case 0:
                /* DESCONEXION */
                pet_desconectar(socket);
                break;
            case 1:
                /* REGISTRO DE JUGADOR */
                pet_registrar_jugador(resto, socket);
                break;
            case 2:
                /* INICIO SESION DE JUGADOR */
                strncpy(nombre, strtok_r(resto, ",", &resto), sizeof(nombre));
                pet_iniciar_sesion(nombre, resto, socket);
                break;
            case 3:
                /* PARTIDAS DE JUGADOR */
                pet_informacion_partidas_jugador(resto, socket);
                break;
            case 4:
                /* INFORMACION DE PARTIDA */
                pet_informacion_partida(resto, socket);
                break;
            case 5:
                /* PUNTUACION MEDIA DE JUGADOR */
                pet_puntuacion_media_jugador(resto, socket);
                break;
            case 7:
                /* CREAR LOBBY */
                pet_crear_lobby(nombre, socket);
                break;
            case 8:
                /* ENVIAR INVITACION LOBBY */
                pet_invitar_lobby(resto, nombre, socket);
                break;
            case 9:
                /* RESPONDER INVITACION LOBBY */
                pet_responder_invitacion(resto, nombre, socket);
                break;
            case 10:
                /* ABANDONAR LOBBY */
                pet_abandonar_lobby(resto, nombre, socket);
                break;
            case 12:
                /* SELECCIONAR COLOR */
                pet_cambio_color(resto, nombre, socket);
                break;
	    case 13:
		/* ENVIAR CHAT */
		pet_enviar_chat(resto, socket);
		break;
            default:
                log_msg(tag, "Peticion desconocida: %d\n", codigo);
                close(socket);
                return NULL;
        }

    }
    log_msg(tag, "Cerrando conexion...\n");
    close(socket);
}
