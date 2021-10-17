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
    public partial class FormRegistro : Form
    {
        Socket conn;

        public FormRegistro(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            //Comprobar contraseña y repetir contraseña son iguales
            if ((txtNombre.Text == "") || (txtPass.Text == "") || (txtPassRept.Text == ""))
            {
                MessageBox.Show("Debe completar todos los campos");
                return;
            }

            if (txtPass.Text != txtPassRept.Text)
            {
                MessageBox.Show("La contraseña repetida no coincide con la primera", "Repetir contraseña incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
              
            if (!chkTerminos.Checked)
            {
                MessageBox.Show("Debe aceptar los términos y condiciones de uso");
                return;
            }

            string pet = "1/" + txtNombre.Text + "," + txtPass.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[20];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            if (res == "YES")
            {
                MessageBox.Show("Usuario creado correctamente.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario ya existente.");
            }
        }
    }
}
