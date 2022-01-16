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
    public partial class TabLogin : TabMenu
    {
        Socket conn;
        public string nombre;
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
            if (txtNombre.Text == "" || txtPass.Text == "")
            {
                lblError.Text = "Falta uno de los datos";
                lblError.Show();
                return;
            }
            this.nombre = Convert.ToString(txtNombre.Text[0]).ToUpper() + txtNombre.Text[1..].ToLower();
            string pet = "2/" + nombre + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            this.Tag = "REGISTRO";
            this.Hide();
        }
        public void ActualizarLogin(string res)
        {
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
                lblError.Text = "Credenciales incorrectas";
                lblError.Show();
            }
        }

        private void btnDarBaja_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "" || txtPass.Text == "")
            {
                lblError.Text = "Falta uno de los datos";
                lblError.Show();
                return;
            }
            this.nombre = Convert.ToString(txtNombre.Text[0]).ToUpper() + txtNombre.Text[1..].ToLower();
            string pet = "35/" + nombre + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
        }

        public void DarBaja(string res)
        {
            if(res == "YES")
            {
                txtNombre.Text = "";
                txtPass.Text = "";
                MessageBox.Show("Se ha dado de baja con éxito.", "Cuenta borrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                lblError.Text = "Credenciales incorrectas";
                lblError.Show();
            }
        }
    }
}
