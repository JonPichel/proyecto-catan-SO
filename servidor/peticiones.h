#ifndef __PETICIONES_H__
#define __PETICIONES_H__

#include "estructuras.h"

/* Funciones peticiones */
void pet_registrar_jugador(char *nombre, char *pass, char *respuesta);
int pet_iniciar_sesion(char *nombre, char *pass, char *respuesta);
void pet_informacion_partidas_jugador(int idJ, char *respuesta);
void pet_informacion_partida(int idP, char *respuesta);
void pet_puntuacion_media_jugador(int idJ, char *respuesta);
int pet_crear_lobby(char *nombre, int socket, char *respuesta); 
void pet_cambio_color(int idP, char *nombre, int color, char *respuesta);

void not_lista_conectados(char *tag);
void not_lista_jugadores(int idP, char *tag);

#endif /* __PETICIONES_H__ */
