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
    public partial class Menu : Form
    {
        Socket server;
        int idJ;

        public Menu(Socket server, int idJ)
        {
            InitializeComponent();
            this.server = server;
            this.idJ = idJ;
        }

        private void btnPromedio_Click(object sender, EventArgs e)
        {
            //Quieremos obtener el promedio de puntos a partir de la id del jugador
            string mensaje = "5/" + idJ;
            //Enviamos al servidor el mensaje
            byte[] peticion = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(peticion);

            //Recibimos la respuesta del servidor
            byte[] respuesta = new byte[512];
            server.Receive(respuesta);
            mensaje = Encoding.ASCII.GetString(respuesta).Split('\0')[0];

            tBoxMedia.Text = mensaje;

        }

        private void btnDesconex_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Nos desconectamos del servidor
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            //Volvemos a la pantalla de inicio de sesión
            Form login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }
        private void btnPartidas_Click(object sender, EventArgs e)
        {
            //Quieremos obtener todas las partidas en las que ha participado el
            //jugador mediante su id correspondiente
            string mensaje = "3/" + idJ;
            //Enviamos al servidor el mensaje
            byte[] peticion = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(peticion);

            //Recibimos la respuesta del servidor
            byte[] respuesta = new byte[512];
            server.Receive(respuesta);
            mensaje = Encoding.ASCII.GetString(respuesta).Split('\0')[0];

            //Cabecera de la tabla de partidas
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Posición", "Posición");
            dataGridView1.Columns.Add("Duración", "Duración");
            dataGridView1.Columns.Add("Fecha", "Fecha");
            dataGridView1.Columns.Add("Hora", "Hora");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Quieremos obtener información de la partida seleccionada
            //int idP = dataGridView1.SelectedRows.Cells[1].Value.ToString();
            string mensaje = "3/" + idP;
            //Enviamos al servidor el mensaje
            byte[] peticion = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(peticion);

            //Recibimos la respuesta del servidor
            byte[] respuesta = new byte[512];
            server.Receive(respuesta);
            mensaje = Encoding.ASCII.GetString(respuesta).Split('\0')[0];

        }
    }
}
