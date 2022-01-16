#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <mysql.h>
#include <pthread.h>

#include "base_datos.h"
#include "util.h"

MYSQL *conn;
pthread_mutex_t mutex_bdd = PTHREAD_MUTEX_INITIALIZER;

int bdd_inicializar(char *maquina) {
    if ((conn = mysql_init(NULL)) == NULL) {
        log_msg("BDD", "Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, maquina, "root", "mysql", "T8_Catan", 0, NULL, 0);
    if (conn == NULL) {
        log_msg("BDD", "Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }
    return 0;
}

void bdd_terminar() {
    mysql_close(conn);
}

int bdd_nombre_pass(char *nombre, char *pass) {
    /*
    Descripcion:
        Obtiene el id y la contraseña de un Jugador de la base de datos dado su nombre
    Parametros:
        nombre: nombre del Jugador
        pass: contraseña del Jugador
    Retorno:
        idJ si OK, -1 si no lo ha encontrado o ERR
    */

    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];

    sprintf(consulta, "SELECT Jugador.id, Jugador.pass FROM Jugador WHERE Jugador.nombre = \'%s\';", nombre);

    pthread_mutex_lock(&mutex_bdd);
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    pthread_mutex_unlock(&mutex_bdd);
    fila = mysql_fetch_row (tabla);

    if (fila != NULL) {
            if (pass != NULL)
                strcpy(pass, fila[1]);
            return atoi(fila[0]);
    } else {
        return -1;
    }
}

float bdd_puntuacion_media(int idJ) {
    /*
    Descripcion:
        Obtiene el promedio de puntos de un jugador en todas sus partidas de la base de datos
    Parametros:
        idJ: id del Jugador
    Retorno:
        Promedio de puntos, -1 si ERR
    */

    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];
    float media;

    sprintf(consulta, "SELECT AVG(Participacion.puntos) FROM (Participacion) WHERE Participacion.idJ = %d;", idJ);

    pthread_mutex_lock(&mutex_bdd);
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    pthread_mutex_unlock(&mutex_bdd);
    fila = mysql_fetch_row(tabla);

    // No hay partidas para el jugador
    if (fila == NULL)
        return -1;
    else {
        if (fila[0] != NULL)
            return atof(fila[0]);
        else
            return -1;
    }
}

int bdd_posicion(int idJ, int idP) {
    /*
    Descripcion:
        Obtiene la posicion en la que ha quedado el jugador en la partida
    Parametros:
        idJ: id del jugador
        idP: id de la partida
    Retorno:
        Posicion del jugador en la partida desde 1, -1 si no participa o ha habido un error.
    */
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];
    int numj, puntuacion, *puntos;
    int i, j, aux;

    sprintf(consulta, "SELECT Jugador.id, Participacion.puntos FROM (Jugador, Participacion)"
            "WHERE Participacion.IdP = %d AND Jugador.id = Participacion.idJ;", idP);

    pthread_mutex_lock(&mutex_bdd);
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    pthread_mutex_unlock(&mutex_bdd);
    numj = mysql_num_rows(tabla);

    // Reservamos la memoria del array de puntos dinamicamente
    puntos = (int *)calloc(numj, sizeof(int));
    puntuacion = -1;
    i = 0;
    while ((fila = mysql_fetch_row(tabla)) != NULL) {
        // Guardamos la puntuacion que nos interesa a parte
        if (idJ == atoi(fila[0])) {
            puntuacion = atoi(fila[1]);
        }
        // Guardamos todas las puntuaciones en un vector
        puntos[i++] = atoi(fila[1]);
    }

    // El jugador no participo en la partida
    if (puntuacion == -1)
        return -1;

    // Ordenar las puntuaciones de mayor a menor (bubblesort)
    for (i = 0; i < numj - 1; i++) {
        for (j = 0; j < (numj - i - 1); j++) {
            if (puntos[j+1] > puntos[j]) {
                aux = puntos[j];
                puntos[j] = puntos[j+1];
                puntos[j+1] = aux;
            }
        }
    }


    for (i = 0; i < numj; i++) {
        if (puntos[i] == puntuacion) {
            // Liberamos la memoria de puntos y devolvemos el valor
            free(puntos);
            return i+1;
        }
    }
}

int bdd_registrar_jugador(char *nombre, char *pass) {
    /*
    Descripcion:
        Registra al jugador en la base de datos
    Parametros:
        nombre: nombre del jugador
        pass: contraseña del jugador
    Retorno:
        0 si OK, -1 si ERR
    */

    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];

    sprintf(consulta, "INSERT INTO Jugador VALUES (0, '%s', '%s')", nombre, pass);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    } else {
        return 0;
    }
}

int bdd_borrar_jugador(char *nombre, char *pass) {
    /*
    Descripcion:
        Borra al jugador en la base de datos
    Parametros:
        nombre: nombre del jugador
        pass: contraseña del jugador
    Retorno:
        0 si OK, -1 si ERR
    */

    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];

    sprintf(consulta, "DELETE FROM Jugador WHERE nombre = '%s' AND pass = '%s'", nombre, pass);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    } else {
        return 0;
    }
}

int bdd_info_partidas(int idJ, char *datos){
    /*
    Descripcion:
    Obtiene informacion de las partidas de un Jugador
    Parametros:
    idJ: id del Jugador
    datos: mensaje con formato idp1,puntos1,fechahora1,duracion1...
    Retorno:
    num. de Partidas si OK, -1 si ERR
    */   
    
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[200];
    int nump;
    
    sprintf(consulta, "SELECT Partida.id,Participacion.puntos,Partida.fechahora,Partida.duracion "
            "FROM (Partida,Participacion) "
            "WHERE Participacion.idJ = %d AND Partida.id = Participacion.idP ORDER BY Partida.id DESC;", idJ);
    
    pthread_mutex_lock(&mutex_bdd);
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }
    
    tabla = mysql_store_result(conn);
    pthread_mutex_unlock(&mutex_bdd);
    nump = mysql_num_rows(tabla);
    if (nump == 0) {
        return 0;
    }
    
    datos[0] = '\0';
    int j = 0;
    while ((fila = mysql_fetch_row(tabla)) != NULL) {
        sprintf(datos, "%s%s#%s,%s,%s#", datos, fila[0], fila[1], fila[2], fila[3]);
        j++;
    }
    // Borrar barra
    datos[strlen(datos)-1]='\0';
    
    return nump;
}

int bdd_info_participaciones(int idP, int **ids, char *info) {
    /*
    Descripcion:
        Obtiene informacion de las participaciones de una partida
    Parametros:
        idP: id de la partida
        ids: pointer a un vector de ids
        info: mensaje con formato nombre1,puntos1,nombre2,puntos2...
    Retorno:
        num. de Jugadores si OK, -1 si ERR
    */
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[200];
    int numj;

    sprintf(consulta, "SELECT Jugador.id, Jugador.nombre, Participacion.puntos FROM (Jugador, Participacion)"
            "WHERE Participacion.IdP = %d AND Jugador.id = Participacion.idJ ORDER BY Participacion.puntos DESC;", idP);

    pthread_mutex_lock(&mutex_bdd);
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    pthread_mutex_unlock(&mutex_bdd);
    numj = mysql_num_rows(tabla);
    if (numj == 0) {
        return 0;
    }

    *ids = (int *)malloc(sizeof(int) * numj);

    int j = 0;
    info[0] = '\0';
    while ((fila = mysql_fetch_row(tabla)) != NULL) {
        (*ids)[j] = atoi(fila[0]);
        sprintf(info, "%s%s,%s,", info, fila[1], fila[2]); // ...nombre,puntos,...
        j++;
    }
    // Borrar coma
    info[strlen(info)-1]='\0';
    return numj;
}

