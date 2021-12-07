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
                switch (tab.Tag)
                {
                    case "EMPEZAR":
                        tabs[2].Show();
                        break;
                    case "DESCONECTAR":
                        this.Close();
                        break;
                }
            }
        }

        public void AtenderListaJugadores(string mensaje)
        {
            mensaje = mensaje.Split("/",2)[1];
            if (host)
            {
                ((TabLobbyHost)tabs[0]).ActualizarListaJugadores(mensaje);
            } else
            {
                ((TabLobbyGuest)tabs[1]).ActualizarListaJugadores(mensaje);
            }
        }

        public void ActualizarListaConectados(string mensaje)
        {
            ((TabLobbyHost)tabs[0]).ActualizarListaConectados(mensaje);
        }

        public void ActualizarChat(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            if (host)
            {
                ((TabLobbyHost)tabs[0]).ActualizarChat(mensaje);
            }
            else
            {
                ((TabLobbyGuest)tabs[1]).ActualizarChat(mensaje);
            }
            if(tabs[2].Visible)
                ((TabTablero)tabs[2]).ActualizarChat(mensaje);
        }
        
        public void RespuestaInvitacion(string mensaje)
        {
            ((TabLobbyHost)tabs[0]).RespuestaInvitacion(mensaje);
        }
        public void PartidaCancelada(string mensaje)
        {
            MessageBox.Show("La partida ha sido cancelada por el anfitrión","Partida cancelada",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }
        public void PartidaEmpezada(string mensaje)
        {
            MessageBox.Show("La partida ha empezado", "Partida empezada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }
        public void PartidaTurno(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).CambiarTurno(mensaje);
        }
        public void TirarDados(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).TirarDados(mensaje);
        }

        public void ComprarCarta(string mensaje)
        {
            mensaje = mensaje.Split("/")[1];
            ((TabTablero)tabs[2]).ComprarCarta(mensaje);
        }
    }
}
