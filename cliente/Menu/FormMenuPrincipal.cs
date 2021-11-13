using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace cliente.Menu
{
    public partial class FormMenuPrincipal : Form
    {
        Socket conn;
        Thread atender;

        List<Tab> tabs = new List<Tab>();
        int idJ;

        delegate void DelegadoRespuestas(string res);

        public FormMenuPrincipal()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            // Conectar al servidor
            IPAddress addrServer = IPAddress.Parse("10.0.2.2");
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
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
        }

        private void FormMenuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
            {
                // Desconectar servidor
                string pet = "0/";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                atender.Interrupt();
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
            }
        }

        private void AtenderServidor()
        {
            while (true)
            {
                DelegadoRespuestas delegado;

                byte[] res_b = new byte[512];
                conn.Receive(res_b);
                if (res_b[0] == 0)
                    continue;

                string[] res = Encoding.ASCII.GetString(res_b).Split('/', 2);
                int codigo = Convert.ToInt32(res[0]);
                string mensaje = res[1].Split('\0')[0];

                switch (codigo)
                {
                    case 1:
                        delegado = new DelegadoRespuestas(((TabRegistro)tabs[1]).ActualizarRegistro);
                        tabs[1].Invoke(delegado, new object[] { mensaje });
                        break;
                    case 2:
                        delegado = new DelegadoRespuestas(((TabLogin)tabs[0]).ActualizarLogin);
                        tabs[0].Invoke(delegado, new object[] { mensaje });
                        break;
                    case 3:
                        delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarDataGrid);
                        tabs[2].Invoke(delegado, new object[] { mensaje });
                        break;
                    case 4:
                        delegado = new DelegadoRespuestas(((TabInfoPartida)tabs[3]).MostrarInfoPartida);
                        tabs[3].Invoke(delegado, new object[] { mensaje });
                        break;
                    case 5:
                        delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarMedia);
                        tabs[2].Invoke(delegado, new object[] { mensaje });
                        break;
                    case 6:
                        delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarListaConectados);
                        tabs[2].Invoke(delegado, new object[] { mensaje });
                        break;
                }
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
                        string pet = "4/" + ((TabMenuPrincipal)tabs[2]).idP.ToString();
                        byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                        conn.Send(pet_b);

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
