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
    public partial class TabRegistro : TabMenu
    {
        Socket conn;

        public TabRegistro(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            // Todo el formulario debe estar cubierto
            if ((txtNombre.Text == "") || (txtPass.Text == "") || (txtPassRept.Text == ""))
                MessageBox.Show("Debe completar todos los campos", "Formulario incompleto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // Los campos de contraseña deben coincidir
            else if (txtPass.Text != txtPassRept.Text)
                MessageBox.Show("La contraseña repetida no coincide con la primera", "Repetir contraseña incorrecto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // Los términos deben ser aceptados
            else if (!chkTerminos.Checked)
                MessageBox.Show("Debe aceptar los términos y condiciones de uso", "Términos y condiciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string pet = "1/" + txtNombre.Text + "," + txtPass.Text;
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);

            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            // El tab de registro se esconde
            this.Tag = "ATRAS";
            this.Hide();
        }

        /// <summary>
        /// Informa de el correcto o incorrecto registro de un jugador
        /// </summary>
        /// <param name="res"> String de la forma YES o NO </param>
        public void ActualizarRegistro(string res)
        {
            // Se ha registrado bien el usuario
            if (res == "YES")
            {
                MessageBox.Show("Usuario creado correctamente.", "Registro de usuario",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Respuesta del tab de registro correcto
                this.Tag = "REGISTRO SI";
                this.Hide();
            }
            // Si no es porque ya existe
            else
            {
                MessageBox.Show("Usuario ya existente.", "Registro de usuario",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
