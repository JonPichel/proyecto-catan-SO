#ifndef __PETICIONES_H__
#define __PETICIONES_H__

#include "estructuras.h"

/* Funciones peticiones */
void pet_desconectar(int socket);
void pet_registrar_jugador(char *resto, int socket);
void pet_iniciar_sesion(char *nombre, char *resto, int socket);
void pet_informacion_partidas_jugador(char *resto, int socket);
void pet_informacion_partida(char *resto, int socket);
void pet_puntuacion_media_jugador(char *resto, int socket);
void pet_crear_lobby(char *nombre, int socket);
void pet_invitar_lobby(char *resto, char *nombre, int socket);
void pet_responder_invitacion(char *resto, char *nombre, int socket);
void pet_abandonar_lobby(char *resto, char *nombre, int socket);
void pet_cambio_color(char *resto, char *nombre, int socket);

void not_lista_conectados(char *tag);
void not_lista_jugadores(int idP, char *tag);
void not_terminar_partida(int idP, char *tag);

#endif /* __PETICIONES_H__ */
