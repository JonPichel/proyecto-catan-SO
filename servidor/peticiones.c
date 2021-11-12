#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "peticiones.h"
#include "base_datos.h"
#include "estructuras.h"

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
            sprintf(respuesta, "2/%d", idJ);
        else
            // Contraseña incorrecta
            strcpy(respuesta, "2/-1");
        }
    else
        // No existe el usuario
        strcpy(respuesta, "2/-1");
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

void pet_lista_conectados(listaconn_t *lista, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de lista de conectados y genera la respuesta
    Parametros:
        lista: lista de usuarios conectados
        respuesta: mensaje con la lista de nombres de usuarios
    */

    sprintf(respuesta, "6/%d/", lista->num);
    if (lista->num == 0)
        return;
    for (int i = 0; i < lista->num; i++) {
        sprintf(respuesta, "%s%s,", respuesta, lista->conectados[i].nombre);
    }
    respuesta[strlen(respuesta) - 1] = '\0';
}
