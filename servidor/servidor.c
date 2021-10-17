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
void bdd_nombre_pass(char nombre, int *idJ, char *pass);

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
                    /* INICIO SESION DE JUGADOR */
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
                    /* INFORMACION DE PARTIDA */
                    int idP;
                    p = strtok(NULL, ",");
                    idP = atoi(p);
                    pet_informacion_partida(idP, respuesta);
                    break;
                case 5:
                    /* PUNTUACION MEDIA DE JUGADOR */
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
//JONATHAN
void pet_registrar_jugador(char *nombre, char *pass, char *respuesta) {

}
//ALBA
void pet_iniciar_sesion(char *nombre, char *pass, char *respuesta) {
    /*
    Descripcion:
	Busca un Jugador en la base de datos y si existe comprueba que la contraseña sea correcta
    Parametros:
	nombre: nombre Jugador
	pass: contraseña introducida
	respuesta: Mensaje de respuesta con el Id del Jugador
    */
    char pass_real[20];
    int idJ;
	
    int existe = bdd_nombre_pass(nombre, idJ, pass_real);
	
    if (existe ==1){
	if (strcmp(pass,pass_real)==0)
		sprintf(respuesta, "%d", idJ);
	else
		sprintf(respuesta, "0");
    }
    else
	sprintf(respuesta, "0")
}

// Usar la de jonathan - JONATHAN
void pet_informacion_partidas_jugador(int idJ, char *respuesta) {

}

// Usar las de jonathan y alba - ALBA
void pet_informacion_partida(int idP, char *respuesta) {

	char nombres[100];


}

// Usar la de raul - ALBA
void pet_puntuacion_media_jugador(int idJ, char *respuesta) {
    /*
    Descripcion:
        Construye el mensaje de respuesta en caso de pedir la media de puntuaciones obtenidas por un Jugador
    Parametros:
        idJ: id del Jugador
		respuesta: Mensaje de respuesta con la media 
    */

    float media = bdd_puntuacion_media(idJ);
    if (media == -1)
	printf("Error en realizar la consulta");
    else{
	sprintf(respuesta, "%f", media);
    }	
}

int bdd_nombre_pass(char nombre, int *idJ, char *pass) {
    /*
    Descripcion:
        Busca el ID y contrasena de un Jugador dado su nombre
    Parametros:
	nombre: nombre del Jugador
        idJ: id del Jugador
	pass: contraseÃ±a del Jugador
    Retorno:
	1 si ha encontrado el Jugador en la base de datos, -1 si no lo ha encontrado o ha habido un error
    */
   
    MYSQL *conn;
    int err;
    MYSQL_RES *resultado;
    MYSQL_ROW row;
    char consulta[80];
	
	
    conn=mysql_init(NULL);
    if (conn==NULL) {
	printf("Error al crear la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
	return -1;
    }
    conn=mysql_real_connect(conn, "localhost","root","mysql","catan",0,NULL,0);
    if (conn==NULL) {
	printf("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
	return -1;
    }
	
    sprintf(consulta,"SELECT Jugador.id, Jugador.pass FROM Jugador WHERE Jugador.nombre = '%s';",nombre);

    err = mysql_query(conn,consulta);
	
    if (err!=0) {
	printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
	return -1;
    }
	
    resultado = mysql_store_result(conn);
    row = mysql_fetch_row (resultado);
    mysql_close(conn);

    if (row == NULL){
	printf("No se han obtenido datos en la consulta\n");
	return -1;
    }
    else{
	idJ = atoi(row[0]);
	sprintf(pass,"%s",row[1]); 
	return 1;
    }
}

// Alba
void bdd_nombres_jugadores_partida(int idP, char *nombres) {
    /*
    Descripcion:
        Devuelve los nombres de los Jugadores que participaron en una partida dado el ID de la partida.
    Parametros:
        idP: id de la partida
	nombres: nombres de Jugadores de una partida separados por una coma
    */
   
    MYSQL *conn;
    int err;
    MYSQL_RES *resultado;
    MYSQL_ROW row;
    char consulta[80];
	
    conn=mysql_init(NULL);
    if (conn==NULL) {
	printf("Error al crear la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		
    }
    conn=mysql_real_connect(conn, "localhost","root","mysql","catan",0,NULL,0);
    if (conn==NULL) {
	printf("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		
    }
	
    sprintf(consulta,"SELECT Jugador.nombre FROM Jugador, Participacion "
	"WHERE Participacion.idP = %d AND Participacion.idJ = Jugador.id;",idP);

    err = mysql_query(conn,consulta);
	
    if (err!=0) {
	printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
	
    }
	
    resultado = mysql_store_result(conn);
    row = mysql_fetch_row (resultado);
	
    if (row == NULL)
	printf("No se han obtenido datos en la consulta\n");
    else{
	printf("Los Jugadores de la partida son:\n");
	while (row !=NULL) {
		printf("%s\n", row[0]);
		sprintf(nombres,"%s%s,",nombres,row[0]);
		row = mysql_fetch_row(resultado);
	}
	nombres[strlen(nombres)-1]='\0';
    }
    mysql_close(conn);
}

// Raul
float bdd_puntuacion_media(int idJ) {
    /*
    Descripcion:
        Devuelve el calculo del promedio de puntos de un jugador obtenidos en todas sus partidas
    Parametros:
        idJ: id del Jugador
    Retorno:
       	Promedio de puntos, -1 si ha habido un error
    */

    MYSQL *conn;
    int err;
    MYSQL_RES *resultado;
    MYSQL_ROW row;
    float media;
    char nombre[20];
    char consulta[100];
	
    conn=mysql_init(NULL);
    if (conn==NULL) {
	printf("Error al crear la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
	return -1;
    }
    conn=mysql_real_connect(conn, "localhost","root","mysql","catan",0,NULL,0);
    if (conn==NULL) {
	printf("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
	return -1;
    }
	
    sprintf(consulta,"SELECT AVG(Participacion.puntos) FROM (Participacion) WHERE Participacion.idJ = %d;",idJ);

    err = mysql_query(conn,consulta);
	
    if (err!=0) {
	printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
	return -1;
    }
	
    resultado = mysql_store_result(conn);
    row = mysql_fetch_row (resultado);
	
    if (row == NULL){
	printf("No se han obtenido datos en la consulta\n");
	media = 0;
    }
    else{
	printf ("Promedio de puntos: %s puntos\n", row[0]);
	media = atof(row[0]);
    }
    mysql_close(conn);
    return media;
}

// Jonathan
int bdd_posicion(int idJ, int idP, int *puntuacion) {
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
