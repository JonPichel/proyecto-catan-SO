#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h> 
#include <netinet/in.h> 
#include <mysql.h>

#define PORT        4444
#define MAX_CONN    5

/* Funciones peticiones */
void pet_registrar_jugador(char *nombre, char *pass, char *respuesta);
void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta);
void pet_informacion_partidas_jugador(int idJ, char *respuesta);
void pet_informacion_partida(int idP, char *respuesta);
void pet_puntuacion_media_jugador(int idJ, char *respuesta);

/* Consultas bases de datos (ejercicios individuales) */
void bdd_nombres_jugadores_partida(int idP, char *nombres); // alba
float bdd_puntuacion_media(int idJ);                        // raul
int bdd_posicion(int idJ, int idP);                         // jon

int main(void) {
    int sock_listen, sock_conn, nbytes;
    struct sockaddr_in host_addr;
    char peticion[512], respuesta[512];

    if ((sock_listen = socket(AF_INET, SOCK_STREM, 0)) < 0) {
		printf("Error creando el socket");
        return -1;
    }

	memset(&host_addr, 0, sizeof(host_addr));

	host_addr.sin_family = AF_INET;
    // Asocia el socket a cualquier IP de la maquina
	host_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	host_addr.sin_port = htons(PORT);

    // Hacemos bind al socket
	if (bind(sock_listen, (struct sockaddr *)&host_addr, sizeof(host_addr)) < 0) {
		printf("Error en el bind");
        return -1;
    }

	// Maximo 2 conexiones en la cola de espera
	if (listen(sock_listen, MAX_CONN) < 0) {
		printf("Error en el listen");
        return -1;
    }

    char *p;
    int codigo;
    while (1) {
        printf("Escuchando...\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        printf("Conexion establecida\n");
        while (1) {
            nbytes = read(sock_conn, peticion, sizeof(peticion));
            peticion[nbytes] = '\0';
            printf("Peticion: %s\n", peticion);

            p = strtok(peticion, "/");
            codigo = atoi(p);

            if (codigo == 0) {
                /* CERRAR CONEXIÓN */
                break;
            }

            switch (codigo) {
                case 1:
                    /* REGISTRO DE JUGADOR */
                    char nombre[20], pass[20];
                    p = strtok(NULL, ",");
                    strncpy(nombre, p, sizeof(nombre));
                    p = strtok(NULL, ",");
                    strncpy(pass, p, sizeof(pass));
                    pet_registrar_jugador(nombre, pass, respuesta);
                    break;
                case 2:
                    /* INICIO SESIÓN DE JUGADOR */
                    char nombre[20], pass[20];
                    p = strtok(NULL, ",");
                    strncpy(nombre, p, sizeof(nombre));
                    p = strtok(NULL, ",");
                    strncpy(pass, p, sizeof(pass));
                    pet_iniciar_sesion(nombre, pass, respuesta);
                    break;
                case 3:
                    /* PARTIDAS DE JUGADOR */
                    int idJ;
                    p = strtok(NULL, ",");
                    idJ = atoi(p);
                    pet_informacion_partidas_jugador(idJ, respuesta);
                    break;
                case 4:
                    /* INFORMACIÓN DE PARTIDA */
                    int idP;
                    p = strtok(NULL, ",");
                    idP = atoi(p);
                    pet_informacion_partida(idP, respuesta);
                    break;
                case 5:
                    /* PUNTUACIÓN MEDIA DE JUGADOR */
                    int idJ;
                    p = strtok(NULL, ",");
                    idJ = atoi(p);
                    pet_puntuacion_media_jugador(idJ, respuesta);
                    break;
                default:
                    printf("Peticion desconocida: %d\n", codigo);
                    close(sock_conn);
                    return -1;
            }

            printf("Respuesta: %s\n", respuesta);
            write(sock_conn, respuesta, strlen(respuesta));
        }

        printf("Cerrando conexion...\n\n\n");
        close(sock_conn);
    }
}

void pet_registrar_jugador(char *nombre, char *pass, char *respuesta) {

}

void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta) {

}

// Usar la de jonathan
void pet_informacion_partidas_jugador(int idJ, char *respuesta) {

}

// Usar las de jonathan y alba
void pet_informacion_partida(int idP, char *respuesta) {

}

// Usar la de raul
void pet_puntuacion_media_jugador(int idJ, char *respuesta) {

}

// Alba
void bdd_nombres_jugadores_partida(int idP, char *nombres) {

}

// Raul
float bdd_puntuacion_media(int idJ) {

}

// Jonathan
int bdd_posicion(int idJ, int idP) {
    /*
    Descripcion:
        Devuelve la posicion en la que ha quedado el jugador en la partida.
    Parametros:
        idJ: id del jugador
        idP: id de la partida
    Retorno:
        Posicion del jugador en la partida desde 1, -1 si no participa o ha habido un error.
    */
    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char buffer[160];
    int numj, puntuacion, *puntos;
    int i, j, aux;

    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    sprintf(buffer, "SELECT Jugador.id, Participacion.puntos FROM (Jugador, Participacion)"
            "WHERE Participacion.IdP = %d AND Jugador.id = Participacion.idJ;", idP);

    if (mysql_query(conn, buffer) != 0) {
        printf("Error en la consulta de datos: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    numj = mysql_num_rows(tabla);

    // Reservamos la memoria del array de puntos dinámicamente
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

    // Cerrar la conexion y liberar la memoria
    mysql_close(conn);

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
