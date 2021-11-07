#ifndef __BASE_DATOS_H__
#define __BASE_DATOS_H__

int bdd_nombre_pass(char *nombre, char *pass);
float bdd_puntuacion_media(int idJ);
int bdd_posicion(int idJ, int idP);
int bdd_registrar_jugador(char *nombre, char *pass);
int bdd_info_partidas(int idJ, char *datos);
int bdd_info_participaciones(int idP, int **ids, char *info);

#endif /* __BASE_DATOS_H__ */
