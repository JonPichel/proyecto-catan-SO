using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace cliente.Menu
{
    public partial class FormMenuPrincipal : Form
    {
        Socket conn;
        List<Tab> tabs = new List<Tab>();
        int idJ;

        public FormMenuPrincipal()
        {
            InitializeComponent();
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            // Conectar al servidor
            IPAddress addrServer = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(addrServer, 4444);

            conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                conn.Connect(ipep);
                MessageBox.Show("Conectado");
            }
            catch (SocketException exc)
            {
                MessageBox.Show("No se ha podido conectar con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.conn = null;
                this.Close();
            }

            // Preparar pantallas
            tabs.Add(new TabLogin(this.conn));
            tabs.Add(new TabRegistro(this.conn));
            tabs.Add(new TabMenuPrincipal(this.conn));
            tabs.Add(new TabInfoPartida(this.conn));

            foreach (Tab tab in this.tabs)
            {
                tab.Hide();
                tab.Location = new Point(0, 0);
                this.Controls.Add(tab);
            }
            tabs[0].VisibleChanged += this.tabLogin_VisibleChanged;
            tabs[1].VisibleChanged += this.tabRegistro_VisibleChanged;
            tabs[2].VisibleChanged += this.tabMenuPrincipal_VisibleChanged;
            tabs[3].VisibleChanged += this.tabInfoPartidas_VisibleChanged;

            tabs[0].Show();
        }

        private void FormMenuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
            {
                // Desconectar servidor
                string pet = "0/";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);

                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
            }
        }

        private void tabLogin_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta
            if (!tabs[0].Visible)
            {
                switch (tabs[0].Tag)
                {
                    case "REGISTRO":
                        tabs[1].Show();
                        break;
                    case "LOGIN":
                        this.idJ = ((TabLogin)tabs[0]).idJ;
                        ((TabMenuPrincipal)tabs[2]).idJ = this.idJ;
                        tabs[2].Show();
                        break;
                }
            }
        }

        private void tabRegistro_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta
            if (!tabs[1].Visible)
            {
                switch (tabs[1].Tag)
                {
                    case "REGISTRO SI":
                    case "ATRAS":
                        tabs[0].Show();
                        break;
                }
            }
        }
        
        private void tabMenuPrincipal_VisibleChanged(object sender, EventArgs e)
        {
            if (!tabs[2].Visible)
            {
                switch (tabs[2].Tag)
                {
                    case "DESCONECTAR":
                        this.Close();
                        break;
                    case "INFO PARTIDA":
                        tabs[3].Show();
                        ((TabInfoPartida)tabs[3]).MostrarInfoPartida(((TabMenuPrincipal)tabs[2]).idP);
                        break;
                }
            }
        }

        private void tabInfoPartidas_VisibleChanged(object sender, EventArgs e)
        {
            if (!tabs[3].Visible)
            {
                tabs[2].Show();
            }
        }
    }
}
