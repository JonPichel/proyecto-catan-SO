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

        delegate void DelegadoRespuestas(ColorJugador res);

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
            tabs.Add(new TabTablero());

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
                        ((TabTablero)tabs[2]).colorJugador = ((TabLobbyHost)tabs[0]).miColor;
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
            if (host)
            {
                ((TabLobbyHost)tabs[0]).Tag = "EMPEZAR";
                ((TabLobbyHost)tabs[0]).Hide();
            }
            else
            {
                ((TabLobbyGuest)tabs[1]).Tag = "EMPEZAR";
                ((TabLobbyGuest)tabs[1]).Hide();
            }
        }
    }
}
