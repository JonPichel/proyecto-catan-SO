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
void pet_enviar_chat(char *resto, int socket);
void pet_empezar_partida(char *resto, int socket);
void pet_acabar_turno(char *resto, int socket);
void pet_tirar_dados(char *resto, int socket);
void pet_carta(char *resto, int socket);
void pet_usar_carta(int codigo, char *resto, int socket);
void pet_dar_recurso_mono(char *resto, int socket);
void pet_colocar(int codigo, char *resto, int socket);
void pet_oferta_comercio(char *resto, int socket);
void pet_respuesta_comercio(char *resto, int socket);
void pet_resultado_comercio(char *resto, int socket);
void pet_resultado_comercio_mar(char *resto, int socket);
void pet_dar_recursos_ladron(char *resto, int socket);
void pet_pedir_recursos_ladron(char *resto, int socket);

#endif /* __PETICIONES_H__ */
