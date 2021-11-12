#ifndef __ESTRUCTURAS_H__
#define __ESTRUCTURAS_H__

/* Macros */
#define MAX_CONN 100

/* Estructuras */
typedef struct {
    char nombre[20];
    int socket;
} conectado_t;

typedef struct {
    int num;
    conectado_t conectados[MAX_CONN];
} listaconn_t;

/* Funciones */
int conn_add_jugador(listaconn_t *lista, char nombre[20], int socket);
int conn_delete_jugador(listaconn_t *lista, int socket);
int conn_socket_jugador(listaconn_t *lista, char nombre[20]);

#endif /* __ESTRUCTURAS_H__ */
