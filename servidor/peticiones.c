#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <pthread.h>

#include "peticiones.h"
#include "base_datos.h"
#include "estructuras.h"
#include "util.h"

/* Variables globales externas */
extern listaconn_t conectados;
extern partida_t partidas[MAX_PART];
extern pthread_mutex_t mutex_estructuras;

void pet_desconectar(int socket) {
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    // Borrar de la lista de conectados
    pthread_mutex_lock(&mutex_estructuras);
    conn_delete_jugador(&conectados, socket);
    pthread_mutex_unlock(&mutex_estructuras);
    // Notificar la desconexion
    not_lista_conectados(tag);
}
void pet_registrar_jugador(char *resto, int socket) {
    /*
    Descripcion:
        Atiende la peticion de registrar jugador y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);
    char nombre[20], pass[20];
    char respuesta[6];

    strncpy(nombre, strtok_r(resto, ",", &resto), sizeof(nombre));
    strncpy(pass, strtok_r(resto, ",", &resto), sizeof(pass));

    if (bdd_nombre_pass(nombre, NULL) == -1) {
        if (bdd_registrar_jugador(nombre, pass) == 0)
            strcpy(respuesta, "1/YES");
        else
            strcpy(respuesta, "1/NO");
    } else {
        // Ya existe el usuario
        strcpy(respuesta, "1/NO");
    }
    // Enviar
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
}

void pet_iniciar_sesion(char *nombre, char *resto, int socket) {
    /*
    Descripcion:
        Atiende la peticion de iniciar sesion y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    Devuelve:
        idJ si OK, -1 si ERR
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    char pass[20], pass_real[20];
    char respuesta[32];
    int idJ, conectado = 0;

    strncpy(pass, strtok_r(resto, ",", &resto), sizeof(pass));
    idJ = bdd_nombre_pass(nombre, pass_real);
    if (idJ != -1) {
        if (strcmp(pass, pass_real) == 0) {
            // Contraseña correcta
            sprintf(respuesta, "2/%d", idJ);
            conectado = 1;
        } else {
            // Contraseña incorrecta
            strcpy(respuesta, "2/-1");
        }
    } else {
        // No existe el usuario
        strcpy(respuesta, "2/-1");
    }
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
    if (conectado) {
        pthread_mutex_lock(&mutex_estructuras);
        conn_add_jugador(&conectados, nombre, socket);
        pthread_mutex_unlock(&mutex_estructuras);
        /* NOTIFICACION LISTA DE CONECTADOS */
        sleep(1);
        not_lista_conectados(tag);
    }
}

void pet_informacion_partidas_jugador(char *resto, int socket) {
    /*
    Descripcion:
        Atiende la peticion de datos de partidas del jugador y genera la respuesta
    Parametros:
        resto: mensaje sin el codigo
        socket: socket del thread que recibe la peticion
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    char datos[512], respuesta[512];
    int idJ = atoi(strtok_r(resto, ",", &resto));
    int nump = bdd_info_partidas(idJ, datos);
    char *p, *p2;
    
    // Generar respuesta
    sprintf(respuesta, "3/%d/", nump);
    if (nump != -1) {
        p2 = datos;
        for (int j = 0; j < nump; j++) {
            p = strtok_r(p2, "#", &p2);
            sprintf(respuesta,"%s%s,%d,", respuesta, p, bdd_posicion(idJ, atoi(p)));
            sprintf(respuesta,"%s%s,", respuesta, strtok_r(p2, "#", &p2));
        }
    }
    respuesta[strlen(respuesta) - 1]='\0';

    // Enviar
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
}

void pet_informacion_partida(char *resto, int socket) {
    /*
    Descripcion:
        Atiende la peticion de informacion de partida y genera la respuesta
    Parametros:
        idP: id de la Partida
        respuesta: mensaje de respuesta con los datos
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    char respuesta[256];
    int idP = atoi(strtok_r(resto, ",", &resto));
    int numj, *ids;
    char datos[200];
    char *p;
    
    numj = bdd_info_participaciones(idP, &ids, datos);
    sprintf(respuesta, "4/%d/", numj);
    p = datos;
    for (int i = 0; i < numj; i++) {
        sprintf(respuesta, "%s%d,", respuesta, bdd_posicion(ids[i], idP));
        strcat(respuesta, strtok_r(p, ",", &p));
        strcat(respuesta, ",");
        strcat(respuesta, strtok_r(p, ",", &p));
        strcat(respuesta, ",");
    }
    respuesta[strlen(respuesta) - 1] = '\0';

    // Enviar
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
}

void pet_puntuacion_media_jugador(char *resto, int socket) {
    /*
    Descripcion:
        Atiende la peticion de puntuacion media y genera la respuesta
    Parametros:
        idJ: id del Jugador
        respuesta: mensaje de respuesta con la media 
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    char respuesta[20];
    sprintf(respuesta, "5/%.2f", bdd_puntuacion_media(atoi(strtok_r(resto, ",", &resto))));

    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
}


void pet_crear_lobby(char *nombre, int socket) {
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    char respuesta[10];
    int lleno = 1;
    for (int i = 0; i < MAX_PART; i++) {
        if (partidas[i].estado == VACIA) {
            partidas[i].estado = LOBBY;
            pthread_mutex_lock(&mutex_estructuras);
            part_add_jugador(&partidas[i], nombre, socket);
            pthread_mutex_unlock(&mutex_estructuras);
            sprintf(respuesta, "7/%d", i);
            lleno = 0;
            break;
        }
    }
    if (lleno)
        strcpy(respuesta, "7/-1");

    // Enviar
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
    write(socket, respuesta, strlen(respuesta));
}

void pet_invitar_lobby(char *resto, char *nombre, int socket) {
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    int idP = atoi(strtok_r(resto, "/", &resto));
    char nombre_guest[20];
    strcpy(nombre_guest, strtok_r(resto, "/", &resto));
    int socket_guest = conn_socket_jugador(&conectados, nombre_guest);
    char respuesta[80];
    if (socket_guest == -1) {
        // Se ha desconectado: rechazar la peticion directamente
        sprintf(respuesta, "8/%d/%s/NO", idP, nombre_guest);
        log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
        write(socket, respuesta, strlen(respuesta));
    } else {
        // Enviar peticion por el socket del invitado
        sprintf(respuesta, "9/%d/%s", idP, nombre);
        log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
        write(socket_guest, respuesta, strlen(respuesta));
    }
}

void pet_responder_invitacion(char *resto, char *nombre, int socket) {
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    int idP = atoi(strtok_r(resto, "/", &resto));
    char decision[2];
    strcpy(decision, strtok_r(resto, "/", &resto));
    char respuesta[80];
    if (strcmp(decision, "SI") == 0) {
        // Add el jugador a la partida y notificar
        pthread_mutex_lock(&mutex_estructuras);
        part_add_jugador(&partidas[idP], nombre, socket);
        pthread_mutex_unlock(&mutex_estructuras);
        not_lista_jugadores(idP, tag);
    }
    // Responder al host
    sprintf(respuesta, "8/%d/%s/%s", idP, nombre, decision);
    log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);				
    write(partidas[idP].jugadores[0].socket, respuesta, strlen(respuesta));
}

void pet_abandonar_lobby(char *resto, char *nombre, int socket) {
    char tag[32];
    sprintf(tag, "THREAD %d", socket);

    int idP = atoi(strtok_r(resto, "/", &resto));
    if (strcmp(partidas[idP].jugadores[0].nombre, nombre) == 0) {
        // Abandona el host
        partidas[idP].estado = VACIA;
        not_terminar_partida(idP, tag);
    } else {
        // Abandona un invitado
        part_delete_jugador(&partidas[idP], nombre);
        not_lista_jugadores(idP, tag);
    }
}

void pet_cambio_color(char *resto, char *nombre, int socket) {
    /*
    Descripcion:
        Realiza el cambio de color de un jugador si este esta disponible
    Parametros:
        idP: identificador de la partida
        nombre, color deseado, respuesta
    */
    char tag[32];
    sprintf(tag, "THREAD %d", socket);
    
    int idP = atoi(strtok_r(resto, "/", &resto));
    int color = atoi(strtok_r(resto, "/", &resto));
    int ret = part_cambio_color(&partidas[idP], nombre, color);
    char respuesta[7];
    if (ret == 0) {
        sprintf(respuesta,"12/OK");
        log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
        sleep(1);
        write(socket, respuesta, strlen(respuesta));
        not_lista_jugadores(idP, tag);
    }
    else {
        sprintf(respuesta,"12/ERR");
        log_msg(tag, "Transmitiendo respuesta: %s\n", respuesta);
        write(socket, respuesta, strlen(respuesta));
    }
}

void not_lista_conectados(char *tag) {
    /*
    Descripcion:
        Notifica a todos los conectados de un cambio en la lista de conectados
    Parametros:
        tag: tag del thread que activa la notificacion
    */
    // Generar mensaje
    char respuesta[512];
    sprintf(respuesta, "6/%d/", conectados.num);
    if (conectados.num != 0) {
        for (int i = 0; i < conectados.num; i++) {
            sprintf(respuesta, "%s%s,", respuesta, conectados.conectados[i].nombre);
        }
        respuesta[strlen(respuesta) - 1] = '\0';
    }
    // Enviar lista
    for (int i = 0; i < conectados.num; i++) {
        log_msg(tag, "Lista de conectados por el socket %d: %s\n",
                conectados.conectados[i].socket, respuesta);
        write(conectados.conectados[i].socket, respuesta, strlen(respuesta));
    }
}

void not_lista_jugadores(int idP, char *tag) {
    /*
    Descripcion:
        Genera el mensaje de notificacion de lista de jugadores
    Parametros:
        idP: id de la partida en la tabla de partidas
        tag: tag del thread que activa la notificacion
    */
    // Generar mensaje
    char respuesta[256];
	sprintf(respuesta, "11/%d/", idP);
    for (int i = 0; i < partidas[idP].numj; i++) {
        sprintf(respuesta, "%s%s,%d,", respuesta, partidas[idP].jugadores[i].nombre, partidas[idP].jugadores[i].color);
    }
	respuesta[strlen(respuesta) - 1] = '\0';
    // Enviar lista
	for (int i = 0; i < partidas[idP].numj; i++) {
		log_msg(tag, "Lista de jugadores por el socket %d: %s\n",
				partidas[idP].jugadores[i].socket, respuesta);
		write( partidas[idP].jugadores[i].socket, respuesta, strlen(respuesta));
	}
}

void not_terminar_partida(int idP, char *tag) {
    // Enviar lista
    char respuesta[32];
    sprintf(respuesta, "10/%d", idP);
	for (int i = 0; i < partidas[idP].numj; i++) {
		log_msg(tag, "Notificando terminar partida por el socket %d: %s\n",
				partidas[idP].jugadores[i].socket, respuesta);
		write(partidas[idP].jugadores[i].socket, respuesta, strlen(respuesta));
	}
}
