using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Catan
{
    public partial class Registro : Form
    {
        Socket server;
        public Registro()
        {
            InitializeComponent();
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            //Comprobar contraseña y repetir contraseña son iguales
            if (tBoxPwd.Text != tBoxPwd2.Text)
                MessageBox.Show("La contraseña repetida no coincide con la primera", "Repetir contraseña incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (!ckBoxTerminos.Checked)
                MessageBox.Show("Debe aceptar los términos y condiciones de uso");

            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9070);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                MessageBox.Show("Conectado");

                //Quieremos comprobar si el nombre y la contraseña estan en la base de datos
                string mensaje = "1/" + tBoxNickname.Text + "," + tBoxPwd.Text;
                //Enviamos al servidor el mensaje
                byte[] peticion = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(peticion);

                //Recibimos la respuesta del servidor
                byte[] respuesta = new byte[512];
                server.Receive(respuesta);
                mensaje = Encoding.ASCII.GetString(respuesta).Split('\0')[0];

                if (mensaje != "0")
                    //Si se recibe '0' es porque no se pudo iniciar sesión
                    MessageBox.Show("Tu nombre ES bonito.");
                else
                {

                }
            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }
    }
}
