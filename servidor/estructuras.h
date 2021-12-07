#ifndef __ESTRUCTURAS_H__
#define __ESTRUCTURAS_H__

/* Macros */
#define MAX_CONN 100
#define MAX_PART 100

/* Estructuras */
typedef struct {
    char nombre[20];
    int socket;
} conectado_t;

typedef struct {
    int num;
    conectado_t conectados[MAX_CONN];
} listaconn_t;

enum COLOR {
    AZUL,
    ROJO,
    NARANJA,
    GRIS,
    MORADO,
    VERDE
};

typedef struct {
    char nombre[20];
    int socket;
    enum COLOR color;
} jugador_t;

enum ESTADO {
    LOBBY,
    JUGANDO,
    VACIA
};

typedef struct {
    int numj;
    jugador_t jugadores[4];
    int numturnos;          // numero de turnos
    int turno;              // a quien le toca
    enum ESTADO estado;
} partida_t;

/* Funciones */
int conn_add_jugador(listaconn_t *lista, char nombre[20], int socket);
int conn_delete_jugador(listaconn_t *lista, int socket);
int conn_socket_jugador(listaconn_t *lista, char nombre[20]);

void inicializar_partidas(partida_t partidas[MAX_PART]);
int part_add_jugador(partida_t *partida, char nombre[20], int socket);
int part_delete_jugador(partida_t *partida, char nombre[20]);
int part_cambio_color(partida_t *partida, char nombre[20], int color);

void barajar_casillas(char *asignacion);
void barajar_puertos(char *asignacion);
int escoger_carta();

#endif /* __ESTRUCTURAS_H__ */
