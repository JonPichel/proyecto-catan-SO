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
    public partial class Login : Form
    {
        Socket server;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            Form registro = new Registro();
            this.Hide();
            registro.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9070);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                //MessageBox.Show("Conectado");

                //Quieremos comprobar si el nombre y la contraseña estan en la base de datos
                string mensaje = "2/" + tboxNickname.Text + "," + tboxPwd.Text;
                //Enviamos al servidor el mensaje
                byte[] peticion = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(peticion);

                //Recibimos la respuesta del servidor
                byte[] respuesta = new byte[512];
                server.Receive(respuesta);
                mensaje = Encoding.ASCII.GetString(respuesta).Split('\0')[0];

                if (mensaje != "0")
                {
                    //Si se recibe '0' es porque no se pudo iniciar sesión
                    MessageBox.Show("Contraseña incorrecta");
                    tboxNickname.Text = "";
                    tboxPwd.Text = "";
                }
                    
                else
                {
                    int idJ = Convert.ToInt32(mensaje);
                    Form menu = new Menu(server, idJ);
                    this.Hide();
                    menu.ShowDialog();
                    this.Close();
                }
                    
            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No se ha podido conectar con el servidor");
                return;
            }
        }

        
    }
}
