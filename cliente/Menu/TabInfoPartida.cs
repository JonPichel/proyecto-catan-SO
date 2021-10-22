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
    public partial class TabInfoPartida : Tab
    {
        Socket conn;

        public TabInfoPartida(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void TabInfoPartida_Load(object sender, EventArgs e)
        {
            dataGridResultadoPartida.RowHeadersVisible = false;
            dataGridResultadoPartida.Columns.Add("Posición", "Posición");
            dataGridResultadoPartida.Columns.Add("Jugador", "Jugador");
            dataGridResultadoPartida.Columns.Add("Puntos", "Puntos");
            dataGridResultadoPartida.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        public void MostrarInfoPartida(int idP)
        {
            lblIdPartida.Text = "Clasificación de la partida: " + idP.ToString();

            string pet = "4/" + idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[512];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            // Ahora primero separamos el numero de partidas juagadas
            // de los datos de cada partida
            string[] trozos = res.Split("/", 2);  // Tendremos 2 trozos

            int numj = Convert.ToInt32(trozos[0]);      // Numero de jugadores
            string[] datos = trozos[1].Split(',');          // Datos de las partidas


            // Rellenamos la tabla con los datos de las partidas
            dataGridResultadoPartida.Rows.Clear();
            dataGridResultadoPartida.Rows.Add(numj);
            for (int i = 0; i < numj; i++)
            {
                dataGridResultadoPartida.Rows[i].Cells[0].Value = datos[3*i + 0];     // Posición del jugador
                dataGridResultadoPartida.Rows[i].Cells[1].Value = datos[3*i + 1];     // Nombre del jugador
                dataGridResultadoPartida.Rows[i].Cells[2].Value = datos[3*i + 2];     // Puntos del jugador
            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
