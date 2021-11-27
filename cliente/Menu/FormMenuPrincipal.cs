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
using cliente.Partida;

namespace cliente.Menu
{
    public partial class FormMenuPrincipal : Form
    {
        Socket conn;
        Thread atender;

        List<TabMenu> tabs = new List<TabMenu>();
        Dictionary<int, FormPartida> partidas = new Dictionary<int, FormPartida>();

        int idJ;
        string nombre;

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
            IPEndPoint ipep = new IPEndPoint(addrServer, 4445);

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

            foreach (TabMenu tab in this.tabs)
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
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                atender.Interrupt();
            }
        }

        private void AtenderServidor()
        {
            ThreadStart ts;
            Thread thread;
            int idP;
            while (true)
            {
                DelegadoRespuestas delegado;

                byte[] res_b = new byte[512];
                conn.Receive(res_b);
                if (res_b[0] == 0)
                    continue;
                
                string entrada = Encoding.ASCII.GetString(res_b).Split('\0')[0];
                string[] trozos = entrada.Split("~~END~~");

                for (int i = 0; i < trozos.Length; i++)
                {
                    if (trozos[i] == "")
                        continue;
                    string[] res = trozos[i].Split('/', 2);
                    
                    int codigo = Convert.ToInt32(res[0]);
                    string mensaje = res[1];

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
                            foreach (FormPartida form in partidas.Values)
                            {
                                if (form.host)
                                {
                                    delegado = new DelegadoRespuestas(form.ActualizarListaConectados);
                                    form.Invoke(delegado, new object[] { mensaje });
                                }
                            }
                            break;
                        case 7:
                            idP = Convert.ToInt32(mensaje);
                            ts = delegate { AbrirPartidaHost(idP); };
                            thread = new Thread(ts);
                            thread.Start();
                            break;
                        case 8:
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].RespuestaInvitacion);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 9:
                            ts = delegate { Invitacion(mensaje); };
                            thread = new Thread(ts);
                            thread.Start();
                            break;
                        case 10:
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaCancelada);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                                partidas[idP].Close();
                            }
                            break;
                        case 11:
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].AtenderListaJugadores);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 13:
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ActualizarChat);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 14:
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaEmpezada);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                    }
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
                        this.nombre = ((TabLogin)tabs[0]).nombre;
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

        private void AbrirPartidaHost(int idP)
        {
            FormPartida form = new FormPartida(this.conn, idP, this.nombre, true);
            partidas.Add(idP, form);
            form.FormClosed += EliminarEntradaForm;
            form.ShowDialog();
        }

        private void Invitacion(string mensaje)
        {
            string[] trozos = mensaje.Split("/");
            string pet;
            int idP = Convert.ToInt32(trozos[0]);
            FormEscoger form = new FormEscoger(
                "Has sido invitado por " + trozos[1],
                "Aceptar", "Rechazar"
            );
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.Yes)
            {
                // Aceptar
                FormPartida formP = new FormPartida(this.conn, idP, this.nombre, false);
                partidas.Add(idP, formP);
                formP.FormClosed += EliminarEntradaForm;
                formP.ShowDialog();
            } else
            {
                // Rechazar
                pet = "9/" + trozos[0] + "/NO";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
            }
        }

        private void EliminarEntradaForm(object sender, EventArgs e)
        {
            FormPartida form = (FormPartida)sender;
            string pet = "10/" + form.idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
            partidas.Remove(form.idP);
        }
    }
}
