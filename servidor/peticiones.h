#ifndef __PETICIONES_H__
#define __PETICIONES_H__

#include "estructuras.h"

/* Funciones peticiones */
void pet_registrar_jugador(char *nombre, char *pass, char *respuesta);
void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta);
void pet_informacion_partidas_jugador(int idJ, char *respuesta);
void pet_informacion_partida(int idP, char *respuesta);
void pet_puntuacion_media_jugador(int idJ, char *respuesta);
void pet_lista_conectados(listaconn_t *lista, char *respuesta);

#endif /* __PETICIONES_H__ */
