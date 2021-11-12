#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "estructuras.h"

int conn_add_jugador(listaconn_t *lista, char nombre[20], int socket) {
    if (lista->num > MAX_CONN)
        return -1;
    strcpy(lista->conectados[lista->num].nombre, nombre);
    lista->conectados[lista->num].socket = socket;
    lista->num++;
    return 0;
}

int conn_delete_jugador(listaconn_t *lista, int socket) {
    for (int i = 0; i < lista->num; i++) {
        if (lista->conectados[i].socket == socket) {
            for (int j = i+1; j < lista->num; j++) {
                strcpy(lista->conectados[j-1].nombre, lista->conectados[j].nombre);
                lista->conectados[j-1].socket = lista->conectados[j].socket;
            }
            lista->num--;
            return 0;
        }
    }
    return -1;
}

int conn_socket_jugador(listaconn_t *lista, char nombre[20]) {
    for (int i = 0; i < lista->num; i++) {
        if (strcmp(lista->conectados[i].nombre, nombre) == 0)
            return lista->conectados[i].socket;
    }
    return -1;
}
