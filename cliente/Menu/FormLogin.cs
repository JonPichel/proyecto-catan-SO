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
    public partial class FormLogin : Form
    {
        Socket conn;
        int idJ;
        bool noConn = false;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
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
                MessageBox.Show("No he podido conectar con el servidor");
                this.noConn = true;
                this.Close();
            }

            // Inicializar GUI
            lblError.Hide();
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!noConn)
            {
                // Desconectar servidor
                string pet = "0/";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);

                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string pet = "2/" + txtNombre.Text + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[20];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            this.idJ = Convert.ToInt32(res);
            if (this.idJ != -1)
            {
                this.Hide();
                FormMenu formMenu = new FormMenu(this.conn, this.idJ);
                formMenu.ShowDialog();
                this.Close();
            } 
            else
            {
                txtNombre.Text = "";
                txtPass.Text = "";
                lblError.Text = "*Credenciales incorrectas";
                lblError.Show();
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            Form formRegistro = new FormRegistro(this.conn);
            this.Hide();
            formRegistro.ShowDialog();
            this.Show();
        }
    }
}

