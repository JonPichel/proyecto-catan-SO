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

void pet_registrar_jugador(char *nombre, char *pass, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de registrar jugador y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    */
    char pass_real[20];
    int idJ;
    

    if (bdd_nombre_pass(nombre, NULL) == -1) {
        if (bdd_registrar_jugador(nombre, pass) == 0)
            strcpy(respuesta, "1/YES");
        else
            strcpy(respuesta, "1/NO");
    }
    else
        // Ya existe el usuario
        strcpy(respuesta, "1/NO");
}

int pet_iniciar_sesion(char *nombre, char *pass, char *respuesta) {
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
    char pass_real[20];
    int idJ;

    idJ = bdd_nombre_pass(nombre, pass_real);
    if (idJ != -1) {
        if (strcmp(pass, pass_real) == 0) {
            sprintf(respuesta, "2/%d", idJ);
            return idJ;
        } else {
            // Contraseña incorrecta
            strcpy(respuesta, "2/-1");
            return -1;
        }
    } else {
        // No existe el usuario
        strcpy(respuesta, "2/-1");
        return -1;
    }
}

void pet_informacion_partidas_jugador(int idJ, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de datos de partidas del jugador y genera la respuesta
    Parametros:
        idJ: id del Jugador
        respuesta: mensaje de respuesta con los datos de las partidas
    */
    char datos[512];
    int nump = bdd_info_partidas(idJ, datos);
    char *p;
    
    sprintf(respuesta, "3/%d/", nump);
    p = strtok(datos, "#");
        
    if (nump != -1) {
        for (int j = 0; j < nump; j++) {
            sprintf(respuesta,"%s%s,%d,", respuesta, p, bdd_posicion(idJ, atoi(p)));
            p = strtok(NULL,"#");
            sprintf(respuesta,"%s%s,", respuesta, p);
            p = strtok(NULL,"#");
        }
    }

    respuesta[strlen(respuesta) - 1]='\0';
}

void pet_informacion_partida(int idP, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de informacion de partida y genera la respuesta
    Parametros:
        idP: id de la Partida
        respuesta: mensaje de respuesta con los datos
    */

    int i, numj, *ids; // ids de los jugadores
    char info[200];
    char *p;

    
    numj = bdd_info_participaciones(idP, &ids, info);
    sprintf(respuesta, "4/%d/", numj);
    p = strtok(info, ",");
    for (i = 0; i < numj; i++) {
        sprintf(respuesta, "%s%d,", respuesta, bdd_posicion(ids[i], idP));
        strcat(respuesta, p);
        strcat(respuesta, ",");
        p = strtok(NULL, ",");
        strcat(respuesta, p);
        strcat(respuesta, ",");
        p = strtok(NULL, ",");
    }
    // Eliminar la coma del final
    respuesta[strlen(respuesta) - 1] = '\0';
}

void pet_puntuacion_media_jugador(int idJ, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de puntuacion media y genera la respuesta
    Parametros:
        idJ: id del Jugador
        respuesta: mensaje de respuesta con la media 
    */

    sprintf(respuesta, "5/%.2f", bdd_puntuacion_media(idJ));
}


int pet_crear_lobby(char *nombre, int socket, char *respuesta) {
    for (int i = 0; i < MAX_PART; i++) {
        if (partidas[i].estado == VACIA) {
            partidas[i].estado = LOBBY;
            pthread_mutex_lock(&mutex_estructuras);
            part_add_jugador(&partidas[i], nombre, socket);
            pthread_mutex_unlock(&mutex_estructuras);
            sprintf(respuesta, "7/%d", i);
            return i;
        }
    }
    strcpy(respuesta, "7/-1");
    return -1;
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
    char respuesta[512];
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
