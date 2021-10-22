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
    public partial class TabLogin : Tab
    {
        Socket conn;
        public int idJ { get; private set; }

        public TabLogin(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void TabLogin_Load(object sender, EventArgs e)
        {
            this.lblError.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string pet = "2/" + txtNombre.Text + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);

            byte[] res_b = new byte[20];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            this.idJ = Convert.ToInt32(res);
            if (this.idJ != -1)
            {
                this.Tag = "LOGIN";
                this.Hide();
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
            this.Tag = "REGISTRO";
            this.Hide();
        }
    }
}
