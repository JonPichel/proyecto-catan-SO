#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>
#include <time.h>

#include "util.h"

#define FORMAT "%x %X" 
#define STREAM stdout

void log_msg(char *tag, char *format, ...) {
    /*
    Descripcion:
        Imprime en pantalla un mensaje de log
    */
    time_t rawtime;
    struct tm *caltime;
    char timestamp[18];
    va_list argp;
    va_start(argp, format);
    time(&rawtime);
    caltime = localtime(&rawtime);
    strftime(timestamp, 18, FORMAT, caltime);
    fprintf(STREAM, "[%s] %s: ", timestamp, tag);
    vfprintf(STREAM, format, argp);
    va_end(argp);
}
