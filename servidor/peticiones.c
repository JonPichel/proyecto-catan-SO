#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "peticiones.h"
#include "base_datos.h"

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
            strcpy(respuesta, "YES");
        else
            strcpy(respuesta, "NO");
    }
    else
        // Ya existe el usuario
        strcpy(respuesta, "NO");
}

void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de iniciar sesion y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    */
    char pass_real[20];
    int idJ;

    idJ = bdd_nombre_pass(nombre, pass_real);
    if (idJ != -1){
        if (strcmp(pass, pass_real) == 0)
            sprintf(respuesta, "%d", idJ);
        else
            // Contraseña incorrecta
            strcpy(respuesta, "-1");
        }
    else
        // No existe el usuario
        strcpy(respuesta, "-1");
}

void pet_informacion_partidas_jugador(int idJ, char *respuesta) {
    char datos[512];
    int nump = bdd_info_partidas(idJ, datos);
    char *p;
    
    sprintf(respuesta, "%d/", nump);
    p = strtok(datos, "#");
        
    if (nump != -1){
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
    sprintf(respuesta, "%d/", numj);
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
        respuesta: Mensaje de respuesta con la media 
    */

    sprintf(respuesta, "%.2f", bdd_puntuacion_media(idJ));
}

