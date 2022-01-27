#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "estructuras.h"

int tipos_casillas[19] = {0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5};

int tipos_puertos[9] = {0, 0, 0, 0, 1, 2, 3, 4, 5};
int saltos_puertos[9] = {3, 3, 3, 3, 3, 3, 4, 4, 4};

int tipos_cartas[25] = {0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 4, 4, 4, 4, 4};

int conn_add_jugador(listaconn_t *lista, int socket) {
    /*
    Descripcion:
        Añade un jugador a la lista de conectados
    Parametros:
        lista: lista de conectados
        socket: socket del cliente conectado
    Retorno:
        0 si OK, -1 si ERR
    */
    if (lista->num > MAX_CONN)
        return -1;
    lista->conectados[lista->num].nombre[0] = '\0';
    lista->conectados[lista->num].socket = socket;
    lista->num++;
    return 0;
}

int conn_id_jugador(listaconn_t *lista, char nombre[20], int socket) {
    /*
    Descripcion:
        Identifica al jugador conectado con un nombre
    Parametros:
        lista: lista de conectados
        nombre: nombre del jugador
        socket: socket del cliente conectado
    Retorno:
        0 si OK, -1 si ERR
    */
    for (int i = lista->num - 1; i >= 0; i--) {
        if (socket == lista->conectados[i].socket) {
            strcpy(lista->conectados[i].nombre, nombre);
            lista->numid++;
            return 0;
        }
    }
    return -1;
}

int conn_delete_jugador(listaconn_t *lista, int socket) {
    /*
    Descripcion:
        Elimina el jugador de la lista de conectados
    Parametros:
        lista: lista de conectados
        socket: socket del jugador conectado
    Retorno:
        0 si OK, -1 si ERR
    */
    for (int i = 0; i < lista->num; i++) {
        if (lista->conectados[i].socket == socket) {
            for (int j = i+1; j < lista->num; j++) {
                strcpy(lista->conectados[j-1].nombre, lista->conectados[j].nombre);
                lista->conectados[j-1].socket = lista->conectados[j].socket;
            }
            lista->num--;
            lista->numid--;
            return 0;
        }
    }
    return -1;
}

int conn_socket_jugador(listaconn_t *lista, char *nombre) {
    /*
    Descripcion:
        Devuelve el socket del jugador conectado
    Parametros:
        lista: lista de conectados
        nombre: nombre del jugador conectado
    Retorno:
        socket si OK, -1 si ERR
    */
    for (int i = 0; i < lista->num; i++) {
        if (strcmp(lista->conectados[i].nombre, nombre) == 0)
            return lista->conectados[i].socket;
    }
    return -1;
}

void inicializar_partidas(partida_t partidas[MAX_PART]) {
    /*
    Descripcion:
        Inicializa el vector de partidas
    Parametros:
        partidas: lista de Partidas
    */
    for (int i = 0; i < MAX_PART; i++) {
        partidas[i].estado = VACIA;
        partidas[i].numj = 0;
        partidas[i].numturnos = 1;
    }
}

int part_add_jugador(partida_t *partida, char nombre[20], int socket){
    /*
    Descripcion:
        Añade un jugador a la Partida
    Parametros:
        partida: puntero a la Partida
        nombre: nombre del jugador a añadir
        socket: socket del jugador a añadir
    Retorno:
        0 si OK, -1 si ERR
    */
    if (partida->numj < 4) {
        strcpy(partida->jugadores[partida->numj].nombre, nombre);
        partida->jugadores[partida->numj].socket = socket;
        // Asignar color disponible
        enum COLOR colores[6] = {AZUL, ROJO, NARANJA, GRIS, MORADO, VERDE};
        for (int i = 0; i < partida->numj; i++) {
            colores[partida->jugadores[i].color] = -1;
        }
        for (int i = 0; i < 6; i++) {
            if (colores[i] != -1) {
                partida->jugadores[partida->numj].color = colores[i];
                break;
            }
        }
        partida->numj++;
        return 0;
    }
    return -1;
}

int part_delete_jugador(partida_t *partida, char nombre[20]){
    /*
    Descripcion:
        Elimina un jugador de la Partida
    Parametros:
        partida: puntero a la Partida
        nombre: nombre del jugador a eliminar
    Retorno:
        posicion del jugador si OK, -1 si ERR (no encontrado)
    */
    for (int i = 0; i < partida->numj; i++) {
        if (strcmp(partida->jugadores[i].nombre, nombre) == 0) {
            for (int j = i+1; j < partida->numj; j++) {
                strcpy(partida->jugadores[j-1].nombre, partida->jugadores[j].nombre);
                partida->jugadores[j-1].socket = partida->jugadores[j].socket;
                partida->jugadores[j-1].color = partida->jugadores[j].color;
            } 
            partida->numj--;
            return i;
        }
    }
    return -1;
}

int part_cambio_color(partida_t *partida, char nombre[20], int color){
    /*
    Descripcion:
        Cambia el color de un jugador de la partida
    Parametros:
        partida: puntero a la Partida
        nombre: nombre del jugador a eliminar
        color: color elegido
    Retorno:
        0 si OK, -1 si ERR (no se ha podido cambiar)
    */
    int num;
    enum COLOR colores[6] = {AZUL, ROJO, NARANJA, GRIS, MORADO, VERDE};
    for (int i = 0; i < partida->numj; i++) {
        if((strcmp(partida->jugadores[i].nombre,nombre) == 0))
            num = i;
        else
            colores[partida->jugadores[i].color] = -1;
    }
    if (colores[color] == -1)
        return -1;
    else{
        partida->jugadores[num].color = color;
        return 0;
    }
}

void barajar(int *a, int size) {
    // Durstenfeld's version of Fisher-Yates algorithm
    int i, j, aux;
    for (i = size-1; i >= 0; i--) {
        j = rand() % (i + 1);
        aux = a[i];
        a[i] = a[j];
        a[j] = aux;
    }
}

void barajar_casillas(char *asignacion) {
    /*
    Descripcion:
        Baraja las casillas para usar en una partida
    Parametros:
        asignacion: puntero a un vector de caracteres para escribir
    */
    barajar(tipos_casillas, 19);

    for (int i = 0; i < 19; i++) {
        switch (tipos_casillas[i]) {
            case 0:
                sprintf(asignacion, "%sDESIERTO,", asignacion);
                break;
            case 1:
                sprintf(asignacion, "%sMADERA,", asignacion);
                break;
            case 2:
                sprintf(asignacion, "%sLADRILLO,", asignacion);
                break;
            case 3:
                sprintf(asignacion, "%sOVEJA,", asignacion);
                break;
            case 4:
                sprintf(asignacion, "%sTRIGO,", asignacion);
                break;
            case 5: sprintf(asignacion, "%sPIEDRA,", asignacion);
                break;
        }
    }
    asignacion[strlen(asignacion) - 1] = '/';
}

void barajar_puertos(char *asignacion) {
    /*
    Descripcion:
        Baraja los puertos para usar en una partida
    Parametros:
        asignacion: puntero a un vector de caracteres para escribir
    */
    barajar(tipos_puertos, 9);
    barajar(saltos_puertos, 9);

    int pos = rand() % 30;
    for (int i = 0; i < 9; i++) {
        pos = (pos + saltos_puertos[i]) % 30;
        switch (tipos_puertos[i]) {
            case 0:
                sprintf(asignacion, "%sGENERAL,%d,", asignacion, pos);
                break;
            case 1:
                sprintf(asignacion, "%sMADERA,%d,", asignacion, pos);
                break;
            case 2:
                sprintf(asignacion, "%sLADRILLO,%d,", asignacion, pos);
                break;
            case 3:
                sprintf(asignacion, "%sOVEJA,%d,", asignacion, pos);
                break;
            case 4:
                sprintf(asignacion, "%sTRIGO,%d,", asignacion, pos);
                break;
            case 5:
                sprintf(asignacion, "%sPIEDRA,%d,", asignacion, pos);
                break;
        }
    }
    asignacion[strlen(asignacion) - 1] = '\0';
}

void barajar_cartas(partida_t *partida) {
    /*
    Descripcion:
        Baraja las cartas para usar en una partida
    Parametros:
        partida: puntero a la Partida
    */
    for (int i = 0; i < 25; i++) {
        partida->cartas[i] = tipos_cartas[i];
    }
    barajar(partida->cartas, 25);
    partida->carta = 0;
}
