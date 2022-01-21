#ifndef __BASE_DATOS_H__
#define __BASE_DATOS_H__

int bdd_inicializar(char *maquina);
void bdd_terminar();

int bdd_nombre_pass(char *nombre, char *pass);
float bdd_puntuacion_media(int idJ);
int bdd_posicion(int idJ, int idP);
int bdd_registrar_jugador(char *nombre, char *pass);
int bdd_actualizar_participacion(char *nombre, char *pass);
int bdd_borrar_jugador(char *nombre, char *pass);
int bdd_info_partidas(int idJ, char *datos);
int bdd_info_participaciones(int idP, int **ids, char *info);
int bdd_registrar_partida(char *fechahora, int duracion, char *ganador);
int bdd_registrar_participacion(int idP, char *nombre, int puntos);

#endif /* __BASE_DATOS_H__ */
