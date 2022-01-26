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
            // Comprueba que esten todos los campos cubiertos
            if (txtNombre.Text == "" || txtPass.Text == "")
            {
                lblError.Text = "Falta uno de los datos";
                lblError.Show();
                return;
            }
            // Petición para loggearse
            this.nombre = Convert.ToString(txtNombre.Text[0]).ToUpper() + txtNombre.Text[1..].ToLower();
            string pet = "2/" + nombre + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            // Muestra el tab de registrarse
            this.Tag = "REGISTRO";
            this.Hide();
        }

        /// <summary>
        /// Informa al usuario si se ha iniciado sesión correctamente o no
        /// y siendo afirmativo abre el menu principal
        /// </summary>
        /// <param name="res"> String con valor idJ si el inicio de sesion es
        /// correcto o -1 si no existe el jugador </param>
        public void ActualizarLogin(string res)
        {
            this.idJ = Convert.ToInt32(res);

            // Si es diferente -1 es porque no existe el jugador
            if (this.idJ != -1)
            {
                // Se esconde el tab de login
                this.Tag = "LOGIN";
                this.Hide();
            }
            // si es -1 se indica que es incorrecto
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
            // Comprueba si hay algún campo vacío
            if (txtNombre.Text == "" || txtPass.Text == "")
            {
                lblError.Text = "Falta uno de los datos";
                lblError.Show();
                return;
            }

            // Petición para eliminar usuario introducido
            this.nombre = Convert.ToString(txtNombre.Text[0]).ToUpper() + txtNombre.Text[1..].ToLower();
            string pet = "35/" + nombre + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
        }

        /// <summary>
        /// Informa del resultado de darse de baja
        /// </summary>
        /// <param name="res"></param>
        public void DarBaja(string res)
        {
            // Si afirmativo es porque se ha eliminado correctamente
            if(res == "YES")
            {
                txtNombre.Text = "";
                txtPass.Text = "";
                MessageBox.Show("Se ha dado de baja con éxito.", "Cuenta borrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // Si no se indica
            else
            {
                lblError.Text = "Credenciales incorrectas";
                lblError.Show();
            }
        }
    }
}
