#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h> 
#include <netinet/in.h> 
#include <mysql.h>
#include <pthread.h>

#include "peticiones.h"
#include "base_datos.h"

#define MAX_CONN    100

/* Decidi usar ids de Jugador en vez de nombres */
typedef struct {
    char nombre[20];
    int socket;
} TJugador;

typedef struct {
    int num;
    TJugador jugadores[MAX_CONN];
} TListaJugador;

void *atender_cliente(void *socket);

int add_jugador(TListaJugador *lista, char nombre[20], int socket);
int del_jugador(TListaJugador *lista, int socket);
int socket_jugador(TListaJugador *lista, char nombre[20]);
void lista_jugadores(TListaJugador *lista, char *respuesta);

TListaJugador conectados;
pthread_mutex_t mutex_lock = PTHREAD_MUTEX_INITIALIZER;

int main(int argc, char *argv[]) {
    int sock_listen, sock_conn;
    struct sockaddr_in host_addr;
    int puerto;

    conectados.num = 0;

    if (argc < 2) {
        printf("Uso: %s <PORT>\n", argv[0]);
        return -1;
    } else {
        puerto = atoi(argv[1]);
        if (puerto == 0) {
            printf("Numero de puerto invalido: %s\n", argv[1]);
            return -1;
        } else {
            printf("Se usara el #%d\n", puerto);
        }
    }

    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
		printf("Error creando el socket");
        return -1; }

	memset(&host_addr, 0, sizeof(host_addr));

	host_addr.sin_family = AF_INET;
    // Asocia el socket a cualquier IP de la maquina
	host_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	host_addr.sin_port = htons(puerto);

    // Hacemos bind al socket
	if (bind(sock_listen, (struct sockaddr *)&host_addr, sizeof(host_addr)) < 0) {
		printf("Error en el bind");
        return -1;
    }

	// Maximo 2 conexiones en la cola de espera
	if (listen(sock_listen, MAX_CONN) < 0) {
		printf("Error en el listen");
        return -1;
    }
    
    int sockets[MAX_CONN];
    pthread_t threads[MAX_CONN];
    for (int i = 0; i < MAX_CONN; i++) {
        printf("Escuchando...\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        printf("Conexion establecida\n");
        
        sockets[i] = sock_conn;
        pthread_create(&threads[i], NULL, atender_cliente, &sock_conn);
    }
    for (int i = 0; i < MAX_CONN; i++) {
        pthread_join(threads[i], NULL);
    }
}

void *atender_cliente(void *socket) {
    int sock_conn = *(int *)socket;
    char peticion[512], respuesta[512];
    char *p;
    int nbytes, codigo;
    
    while (1) {
        nbytes = read(sock_conn, peticion, sizeof(peticion));
        peticion[nbytes] = '\0';
        printf("Peticion: %s\n", peticion);
        
        p = strtok(peticion, "/");
        codigo = atoi(p);
        
        if (codigo == 0) {
            /* CERRAR CONEXION */
            pthread_mutex_lock(&mutex_lock);
            del_jugador(&conectados, sock_conn);
            pthread_mutex_unlock(&mutex_lock);
            break;
        }
        
        char nombre[20], pass[20];
        int idJ, idP;
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
                    pthread_mutex_lock(&mutex_lock);
                    add_jugador(&conectados, nombre, sock_conn);
                    pthread_mutex_unlock(&mutex_lock);
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
                lista_jugadores(&conectados, respuesta);
                break;
            default:
                printf("Peticion desconocida: %d\n", codigo);
                close(sock_conn);
                return NULL;
        }
        printf("Respuesta: %s\n", respuesta);
        write(sock_conn, respuesta, strlen(respuesta));
    }
        
    printf("Cerrando conexion...\n\n\n");
    close(sock_conn);
}

int add_jugador(TListaJugador *lista, char nombre[20], int socket) {
    if (lista->num > MAX_CONN)
        return -1;
    strcpy(lista->jugadores[lista->num].nombre, nombre);
    lista->jugadores[lista->num].socket = socket;
    lista->num++;
    return 0;
}

int del_jugador(TListaJugador *lista, int socket) {
    for (int i = 0; i < lista->num; i++) {
        if (lista->jugadores[i].socket == socket) {
            for (int j = i+1; j < lista->num; j++) {
                strcpy(lista->jugadores[j-1].nombre, lista->jugadores[j].nombre);
                lista->jugadores[j-1].socket = lista->jugadores[j].socket;
            }
            lista->num--;
            return 0;
        }
    }
    return -1;
}

int socket_jugador(TListaJugador *lista, char nombre[20]) {
    for (int i = 0; i < lista->num; i++) {
        if (strcmp(lista->jugadores[i].nombre, nombre) == 0)
            return lista->jugadores[i].socket;
    }
    return -1;
}

void lista_jugadores(TListaJugador *lista, char *respuesta) {
    sprintf(respuesta, "%d/", lista->num);
    if (lista->num == 0)
        return;
    for (int i = 0; i < lista->num; i++) {
        sprintf(respuesta, "%s%s,", respuesta, lista->jugadores[i].nombre);
    }
    respuesta[strlen(respuesta) - 1] = '\0';
}
