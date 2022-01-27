using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace cliente.Partida
{
    public partial class FormPartida : Form
    {
        Socket conn;
        public int idP;
        string nombre;
        public bool host;

        List<TabPartida> tabs = new List<TabPartida>();


        Form FormSecundario = null;

        public FormPartida(Socket conn, int idP, string nombre, bool host)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
            this.host = host;
        }

        private void FormPartida_Load(object sender, EventArgs e)
        {
            // Preparar pantallas
            tabs.Add(new TabLobbyHost(conn, idP, nombre));
            tabs.Add(new TabLobbyGuest(conn, idP, nombre));
            tabs.Add(new TabTablero(conn, idP, nombre));
            tabs.Add(new TabPartidaGanada());

            foreach (TabPartida tab in this.tabs)
            {
                tab.Hide();
                tab.Location = new Point(0, 0);
                this.Controls.Add(tab);
            }

            tabs[0].VisibleChanged += this.tabLobby_VisibleChanged;
            tabs[1].VisibleChanged += this.tabLobby_VisibleChanged;
            if (host)
            {
                tabs[0].Show();
            } else
            {
                tabs[1].Show();
            }

        }

        private void tabLobby_VisibleChanged(object sender, EventArgs e)
        {
            TabPartida tab = (TabPartida)sender;

            // Solo nos importa cuando se oculta
            if (!tab.Visible)
            {
                // Comprobamos el tag
                switch (tab.Tag)
                {
                    case "EMPEZAR":
                        // Se habre el tab tablero
                        tabs[2].Show();
                        break;
                    case "DESCONECTAR":
                        // Se cierra el form de partida
                        this.Close();
                        break;
                }
            }
        }

        /// <summary>
        /// Pasa la lista de jugadores al tab de lobby
        /// </summary>
        /// <param name="mensaje"> string de la forma: idPartida/host,
        /// color,guest1,color,guest2,color,guest3,color... </param>
        public void AtenderListaJugadores(string mensaje)
        {
            mensaje = mensaje.Split("/",2)[1];
            // Si somos el host al tab de host
            if (host)
            {
                ((TabLobbyHost)tabs[0]).ActualizarListaJugadores(mensaje);
            } 
            // Si no al de guest
            else
            {
                ((TabLobbyGuest)tabs[1]).ActualizarListaJugadores(mensaje);
            }
        }

        /// <summary>
        /// Pasa la lista de conectados al tab de lobby host
        /// </summary>
        /// <param name="mensaje"> string de la forma: #jug/nombre1,
        /// nombre2,nombre3... </param>
        public void ActualizarListaConectados(string mensaje)
        {
            ((TabLobbyHost)tabs[0]).ActualizarListaConectados(mensaje);
        }

        /// <summary>
        /// Pasa los mensajes del chat al tab de partida
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/mensaje </param>
        public void ActualizarChat(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];        // solo nos interesa el mensaje
            // Si es el host se mandará a su tab
            if (host)
            {
                ((TabLobbyHost)tabs[0]).ActualizarChat(mensaje);
            }
            // Si no al del lobby
            else
            {
                ((TabLobbyGuest)tabs[1]).ActualizarChat(mensaje);
            }
            // Si la partida ya empezado al tab tablero
            if(tabs[2].Visible)
                ((TabTablero)tabs[2]).ActualizarChat(mensaje);
        }

        /// <summary>
        /// Pasa la respuesta de un guest a la invitación de unirse
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/guest/SI o NO </param>
        public void RespuestaInvitacion(string mensaje)
        {
            // Solo se le indica al host
            ((TabLobbyHost)tabs[0]).RespuestaInvitacion(mensaje);
        }

        /// <summary>
        /// Muestra un mensaje conforme un jugador ha abandonado
        /// </summary>
        /// <param name="mensaje"> String con el idPartida </param>
        public void PartidaCancelada(string mensaje)
        {
            // Se cierra el form de partida
            MessageBox.Show("Un jugador ha abandonado la partida. \nLa partida ha sido cancelada.", "Partida cancelada",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }

        /// <summary>
        /// Pasa al tab tablero la orden de empezar y toda la distribución de
        /// contenido así como la configuración de paneles y recursos a 0
        /// </summary>
        /// <param name="mensaje"> String con la estructura de las fichas del 
        /// tablero (puertos, hexagonos y numeros) </param>
        public void PartidaEmpezada(string mensaje)
        {
            //MessageBox.Show("La partida ha empezado", "Partida empezada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string[] trozos = mensaje.Split("/")[1..];
            ((TabTablero)tabs[2]).CargarTablero(trozos);
            if (host)
            {
                TabLobbyHost tab = ((TabLobbyHost)tabs[0]);
                tab.Tag = "EMPEZAR";
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (tab.colores[i] == (ColorJugador)(-1))
                    {
                        break;
                    }
                    else if (this.nombre == tab.nombres[i])
                        tabs[2].colorJugador = tab.colores[i];
                }
                ((TabTablero)tabs[2]).colores = new ColorJugador[i];
                ((TabTablero)tabs[2]).nombres = new string[i];

                for (i = 0; i < ((TabTablero)tabs[2]).colores.Length; i++)
                {
                    ((TabTablero)tabs[2]).colores[i] = tab.colores[i];
                    ((TabTablero)tabs[2]).nombres[i] = tab.nombres[i];
                }
                tab.Hide();

                ((TabTablero)tabs[2]).nombres = tab.nombres;

            }
            else
            {
                TabLobbyGuest tab = ((TabLobbyGuest)tabs[1]);
                tab.Tag = "EMPEZAR";
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (tab.colores[i] == (ColorJugador)(-1))
                    {
                        break;
                    }
                    else if (this.nombre == tab.nombres[i])
                        tabs[2].colorJugador = tab.colores[i];
                }
                ((TabTablero)tabs[2]).colores = new ColorJugador[i];
                ((TabTablero)tabs[2]).nombres = new string[i];
                for (i = 0; i < ((TabTablero)tabs[2]).colores.Length; i++)
                {
                    ((TabTablero)tabs[2]).colores[i] = tab.colores[i];
                    ((TabTablero)tabs[2]).nombres[i] = tab.nombres[i];
                }
                tab.Hide();
            }
            TabTablero Tab = (TabTablero)tabs[2];
            ((TabPartidaGanada)tabs[3]).colores = Tab.colores;
            ((TabPartidaGanada)tabs[3]).nombres = Tab.nombres;
        }
        /// <summary>
        /// Se llama a la petición de mandar el nombre y puntos del jugador
        /// </summary>
        /// <param name="mensaje"> String de la forma idPartida/idPartidaBDD </param>
        public void PartidaGanada(string mensaje)
        {
            TabTablero tab = (TabTablero)tabs[2];

            string pet;
            byte[] pet_b;
            int puntos = tab.puntos;
            if (puntos > 10)
                puntos = 10;
            pet = "34/" + mensaje + "/" + this.nombre + "," + Convert.ToString(puntos);
            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            tab.Hide();
            tabs[3].Show();
            timerFinalPartida.Start();

        }

        /// <summary>
        /// Pasa al tab de podio el nombre y puntos del jugador que manda
        /// la petición
        /// </summary>
        /// <param name="mensaje"> String de la forma: nombre,puntos </param>
        public void Participacion(string mensaje)
        {
            ((TabPartidaGanada)tabs[3]).ActualizarRanking(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero el turno del siguiente jugador
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/nombre </param>
        public void PartidaTurno(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).CambiarTurno(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero el número de los dados
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/dado1/dado2 </param>
        public void TirarDados(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).TirarDados(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero el resultado de la compra de una carta de desarrollo
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/tipoCarta(0..4) 
        /// donde el tipo solo lo procesará el comprador </param>
        public void ComprarCarta(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).ComprarCarta(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero la carta que ha usado un jugador
        /// </summary>
        /// <param name="mensaje"> String de la forma cod/idPartida 
        /// donde el cod es el codigo de la notificación e indica el tipo 
        /// de carta usada </param>
        public void UsarCarta(string mensaje)
        {
            ((TabTablero)tabs[2]).UsarCarta(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero la posición y tipo de ficha que se debe ha colocado
        /// </summary>
        /// <param name="mensaje"> String de la forma: cod/idPartida/coords
        /// donde cod es el codigo de la notificación e indica el tipo de
        /// ficha a colocar y coords las coordenadas de la ficha en el tablero </param>
        public void Colocar(string mensaje)
        {
            ((TabTablero)tabs[2]).Colocar(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero los recursos entregados trás ser afectado por una carta de 
        /// monopolio
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/nombreEntrega, recurso,cantidad
        /// Indicando el nombre del jugador que entrega, el tipo y número de recursos </param>
        public void DarMonopolio(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).DarMonopolio(mensaje);
	}

        /// <summary>
        /// Pasa la tab tablero una oferta de comercio de un jugador
        /// </summary>
        /// <param name="mensaje"> String de la forma: cod/idPartida/OMadera,OLadrillo...,
        /// PMadera,PLadrillo... representando los números de cada recurso y O
        /// la cantidad que se da y P la que se recibe </param>
        public void ComercioOferta(string mensaje)
        {
            ((TabTablero)tabs[2]).ComercioOferta(mensaje);
        }

        /// <summary>
        /// Pasa la tab tablero la respuesta de un jugador ante una oferta de 
        /// comercio
        /// </summary>
        /// <param name="mensaje"> String de la forma: 28/idPartida/NombreAcepta/SI o NO </param>
        public void ComercioRespuesta(string mensaje)
        {
            ((TabTablero)tabs[2]).ComercioRespuesta(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero el resultado de un comercio entre dos jugadores
        /// </summary>
        /// <param name="mensaje"> String de la forma: cod/idPartida/OMadera,OLadrillo...,
        /// PMadera,PLadrillo... representando los números de cada recurso y O
        /// la cantidad que se da y P la que se recibe </param>
        public void ComercioResultado(string mensaje)
        {
            ((TabTablero)tabs[2]).ComercioResultado(mensaje);
        }

        /// <summary>
        /// Pasa al tab tablero el resultado de un comercio marítimo o con la banca
        /// </summary>
        /// <param name="mensaje"> String de la forma: cod/idPartida/OMadera,OLadrillo...,
        /// PMadera,PLadrillo... representando los números de cada recurso y O
        /// la cantidad que se da y P la que se recibe </param>
        public void ComercioMaritimo(string mensaje)
        {
            ((TabTablero)tabs[2]).ComercioMaritimo(mensaje);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensaje"></param>
        public void DarLadron(string mensaje)
        {
            ((TabTablero)tabs[2]).DarLadron(mensaje);
        }

        private void timerFinalPartida_Tick(object sender, EventArgs e)
        {
            // Cuando acaba una partida se espera un tiempo y se vuelve al tab
            // de lobby
            tabs[3].Hide();
            if (host)
                tabs[0].Show();
            else
                tabs[1].Show();

            // Reseteamos los Tabs de Tablero y PartidaGanada
            this.Controls.Remove(tabs[2]);
            this.Controls.Remove(tabs[3]);
            tabs.Remove(tabs[3]);
            tabs.Remove(tabs[2]);
            tabs.Add(new TabTablero(this.conn, this.idP, this.nombre));
            tabs.Add(new TabPartidaGanada());
            tabs[2].Hide();
            tabs[2].Location = new Point(0, 0);
            tabs[3].Hide();
            tabs[3].Location = new Point(0, 0);
            this.Controls.Add(tabs[2]);
            this.Controls.Add(tabs[3]);

            timerFinalPartida.Stop();
        }
    }
}
