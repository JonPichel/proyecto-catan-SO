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
    public partial class FormInfoPartida : Form
    {
        Socket conn;
        int idP;

        public FormInfoPartida(Socket conn, int idP)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
        }

        private void FormInfoPartida_Load(object sender, EventArgs e)
        {
            lblIdPartida.Text = "Clasificación de la partida: " + Convert.ToString(idP);
            dataGridResultadoPartida.RowHeadersVisible = false;
            dataGridResultadoPartida.Columns.Add("Posición", "Posición");
            dataGridResultadoPartida.Columns.Add("Jugador", "Jugador");
            dataGridResultadoPartida.Columns.Add("Puntos", "Puntos");
            dataGridResultadoPartida.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            string pet = "4/" + idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[512];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            // Ahora primero separamos el numero de partidas juagadas
            // de los datos de cada partida
            string[] trozos = res.Split(new string[] { "/" }, 2, StringSplitOptions.None);  // Tendremos 2 trozos

            int jugadores = Convert.ToInt32(trozos[0]);      // Numero de jugadores
            string[] datos = trozos[1].Split(',');          // Datos de las partidas


            // Rellenamos la tabla con los datos de las partidas
            dataGridResultadoPartida.Rows.Add(jugadores);
            int j = 0;
            for (int i = 0; i < jugadores; i++)
            {
                dataGridResultadoPartida.Rows[i].Cells[0].Value = datos[j];         // Posición del jugador
                dataGridResultadoPartida.Rows[i].Cells[1].Value = datos[j + 1];     // Nombre del jugador
                dataGridResultadoPartida.Rows[i].Cells[2].Value = datos[j + 2];     // Puntos del jugador
                j += 3;
            }

        }
    }
}
