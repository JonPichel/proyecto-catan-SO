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
        Thread atender;
        int idJ;
        ColorJugador miColor;

        List<Tab> tabs = new List<Tab>();

        delegate void DelegadoRespuestas(ColorJugador res);

        public FormPartida()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }



        private void FormPartida_Load(object sender, EventArgs e)
        {
            // Conectar al servidor (FALTA!!)


            // Preparar pantallas
            tabs.Add(new TabLobbyHost(conn));
            tabs.Add(new TabTablero());

            foreach (Tab tab in this.tabs)
            {
                tab.Hide();
                tab.Location = new Point(0, 0);
                this.Controls.Add(tab);
            }

            tabs[0].VisibleChanged += this.tabLobby_VisibleChanged;
            tabs[0].Show();
        }

        private void tabLobby_VisibleChanged(object sender, EventArgs e)
        {

            // Solo nos importa cuando se oculta
            if (!tabs[0].Visible)
            {
                switch (tabs[0].Tag)
                {
                    case "EMPEZAR":
                        this.miColor = ((TabLobbyHost)tabs[0]).miColor;
                        ((TabTablero)tabs[1]).colorJugador = this.miColor;
                        tabs[1].Show();
                        break;
                }
            }
        }
    }
}
