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
int bdd_nombres_jugadores_partida(int idP, char *nombres); // alba
float bdd_puntuacion_media(int idJ);                        // raul
int bdd_posicion(int idJ, int idP);                         // jon
int bdd_nombre_pass(char *nombre, char *pass);
int bdd_registrar_jugador(char *nombre, char *pass);
int bdd_info_participaciones(int idP, int **ids, char *info);
int bdd_info_partidas(int idJ, char *datos);

int main(void) {
    int sock_listen, sock_conn, nbytes;
    struct sockaddr_in host_addr;
    char peticion[512], respuesta[512];

    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
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
                /* CERRAR CONEXION */
                break;
            }

            char nombre[20], pass[20];
            int idJ, idP;
            switch (codigo) {
                case 1:
                    /* REGISTRO DE JUGADOR */
                    p = strtok(NULL, ",");
                    strncpy(nombre, p, sizeof(nombre));
                    p = strtok(NULL, ",");
                    strncpy(pass, p, sizeof(pass));
                    pet_registrar_jugador(nombre, pass, respuesta);
                    break;
                case 2:
                    /* INICIO SESION DE JUGADOR */
                    p = strtok(NULL, ",");
                    strncpy(nombre, p, sizeof(nombre));
                    p = strtok(NULL, ",");
                    strncpy(pass, p, sizeof(pass));
                    pet_iniciar_sesion(nombre, pass, respuesta);
                    break;
                case 3:
                    /* PARTIDAS DE JUGADOR */
                    p = strtok(NULL, ",");
                    idJ = atoi(p);
                    pet_informacion_partidas_jugador(idJ, respuesta);
                    break;
                case 4:
                    /* INFORMACION DE PARTIDA */
                    p = strtok(NULL, ",");
                    idP = atoi(p);
                    pet_informacion_partida(idP, respuesta);
                    break;
                case 5:
                    /* PUNTUACION MEDIA DE JUGADOR */
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
//JONATHAN
void pet_registrar_jugador(char *nombre, char *pass, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de registrar jugador y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    */
    char pass_real[20];
    int idJ;
    

    if (bdd_nombre_pass(nombre, NULL) == -1) {
        if (bdd_registrar_jugador(nombre, pass) == 0)
            strcpy(respuesta, "YES");
        else
            strcpy(respuesta, "NO");
    }
    else
        // Ya existe el usuario
        strcpy(respuesta, "NO");
}

//ALBA
void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de iniciar sesion y genera la respuesta
    Parametros:
        nombre: nombre Jugador
        pass: contraseña introducida
        respuesta: mensaje de respuesta con el id del Jugador
    */
    char pass_real[20];
    int idJ;

    idJ = bdd_nombre_pass(nombre, pass_real);
    if (idJ != -1){
        if (strcmp(pass, pass_real) == 0)
            sprintf(respuesta, "%d", idJ);
        else
            // Contraseña incorrecta
            strcpy(respuesta, "-1");
        }
    else
        // No existe el usuario
        strcpy(respuesta, "-1");
}

// Usar la de jonathan - ALBA
void pet_informacion_partidas_jugador(int idJ, char *respuesta) {
    
    char datos[400];
    int nump = bdd_info_partidas(idJ, datos);
    char *p;
    
    sprintf(respuesta, "%d/", nump);
    p = strtok(datos, "#");
        
    if (nump != -1){
        for (int j = 0; j < nump; j++) {
            sprintf(respuesta,"%s%s,%d,", respuesta, p, bdd_posicion(idJ, atoi(p)));
            p = strtok(NULL,"#");
            sprintf(respuesta,"%s%s,", respuesta, p);
            p = strtok(NULL,"#");
        }
    }

    respuesta[strlen(respuesta) - 1]='\0';
}

// Usar las de jonathan y alba - JONATHAN
void pet_informacion_partida(int idP, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de informacion de partida y genera la respuesta
    Parametros:
        idP: id de la Partida
        respuesta: mensaje de respuesta con los datos
    */

    int i, numj, *ids; // ids de los jugadores
    char info[200];
    char *p;

    
    sprintf(respuesta, "%d/", numj);
    p = strtok(info, ",");
    for (i = 0; i < numj; i++) {
        sprintf(respuesta, "%s%d,", respuesta, bdd_posicion(ids[i], idP));
        strcat(respuesta, p);
        strcat(respuesta, ",");
        p = strtok(NULL, ",");
        strcat(respuesta, p);
        strcat(respuesta, ",");
        p = strtok(NULL, ",");
    }
    // Eliminar la coma del final
    respuesta[strlen(respuesta) - 1] = '\0';
}

// Usar la de raul - ALBA
void pet_puntuacion_media_jugador(int idJ, char *respuesta) {
    /*
    Descripcion:
        Atiende la peticion de puntuacion media y genera la respuesta
    Parametros:
        idJ: id del Jugador
        respuesta: Mensaje de respuesta con la media 
    */

    sprintf(respuesta, "%.2f", bdd_puntuacion_media(idJ));
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

    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];

    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    sprintf(consulta, "SELECT Jugador.id, Jugador.pass FROM Jugador WHERE Jugador.nombre = \'%s\';", nombre);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    tabla = mysql_store_result(conn);
    fila = mysql_fetch_row (tabla);
    mysql_close(conn);

    if (fila != NULL) {
            if (pass != NULL)
                strcpy(pass, fila[1]);
            return atoi(fila[0]);
    } else {
        return -1;
    }
}

// Raul
float bdd_puntuacion_media(int idJ) {
    /*
    Descripcion:
        Obtiene el promedio de puntos de un jugador en todas sus partidas de la base de datos
    Parametros:
        idJ: id del Jugador
    Retorno:
        Promedio de puntos, -1 si ERR
    */

    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];
    float media;

    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    sprintf(consulta, "SELECT AVG(Participacion.puntos) FROM (Participacion) WHERE Participacion.idJ = %d;", idJ);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        mysql_close(conn);
        return -1;
    }

    tabla = mysql_store_result(conn);
    fila = mysql_fetch_row(tabla);
    mysql_close(conn);

    // No hay partidas para el jugador
    if (fila == NULL)
        return -1;
    else
        return atof(fila[0]);
}

// Jonathan
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
    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];
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

    sprintf(consulta, "SELECT Jugador.id, Participacion.puntos FROM (Jugador, Participacion)"
            "WHERE Participacion.IdP = %d AND Jugador.id = Participacion.idJ;", idP);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        mysql_close(conn);
        return -1;
    }

    tabla = mysql_store_result(conn);
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


    // Cerrar la conexion y liberar la memoria
    mysql_close(conn);

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
    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[160];

    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    sprintf(consulta, "INSERT INTO Jugador VALUES (0, '%s', '%s')", nombre, pass);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        mysql_close(conn);
        return -1;
    } else {
        mysql_close(conn);
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
    
    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[200];
    int nump;
    
    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }
    
    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }
    
    sprintf(consulta, "SELECT Partida.id,Participacion.puntos,Partida.fechahora,Partida.duracion "
            "FROM (Partida,Participacion) "
            "WHERE Participacion.idJ = %d AND Partida.id = Participacion.idP ORDER BY Partida.id DESC;", idJ);
    
    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        mysql_close(conn);
        return -1;
    }
    
    tabla = mysql_store_result(conn);
    nump = mysql_num_rows(tabla);
    if (nump == 0) {
        mysql_close(conn);
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
    
    mysql_close(conn);
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
    MYSQL *conn;
    MYSQL_RES *tabla;
    MYSQL_ROW fila;
    char consulta[200];
    int numj;

    if ((conn = mysql_init(NULL)) == NULL) {
        printf("Error al inicializar MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "catan", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al conectar con MySQL: %u %s\n", mysql_errno(conn), mysql_error(conn));
        return -1;
    }

    sprintf(consulta, "SELECT Jugador.id, Jugador.nombre, Participacion.puntos FROM (Jugador, Participacion)"
            "WHERE Participacion.IdP = %d AND Jugador.id = Participacion.idJ ORDER BY Participacion.puntos DESC;", idP);

    if (mysql_query(conn, consulta) != 0) {
        printf("Error en la consulta: %u %s\n", mysql_errno(conn), mysql_error(conn));
        mysql_close(conn);
        return -1;
    }

    tabla = mysql_store_result(conn);
    numj = mysql_num_rows(tabla);
    if (numj == 0) {
        mysql_close(conn);
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
    mysql_close(conn);
    return numj;
}
