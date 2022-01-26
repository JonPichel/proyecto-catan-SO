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
    public partial class TabInfoPartida : TabMenu
    {
        Socket conn;

        public TabInfoPartida(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void TabInfoPartida_Load(object sender, EventArgs e)
        {
            // Carga header de la tabla
            dataGridResultadoPartida.RowHeadersVisible = false;
            dataGridResultadoPartida.Columns.Add("Posición", "Posición");
            dataGridResultadoPartida.Columns.Add("Jugador", "Jugador");
            dataGridResultadoPartida.Columns.Add("Puntos", "Puntos");
            dataGridResultadoPartida.Columns[0].Width = dataGridResultadoPartida.Width / 3;
            dataGridResultadoPartida.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridResultadoPartida.Columns[1].Width = dataGridResultadoPartida.Width / 3;
            dataGridResultadoPartida.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridResultadoPartida.Columns[2].Width = dataGridResultadoPartida.Width / 3;
            dataGridResultadoPartida.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridResultadoPartida.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridResultadoPartida.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridResultadoPartida.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridResultadoPartida.ClearSelection();

        }

        public void MostrarInfoPartida(string res)
        {
           
            // Ahora primero separamos el numero de partidas juagadas
            // de los datos de cada partida
            string[] trozos = res.Split("/", 2);  // Tendremos 2 trozos

            int numj = Convert.ToInt32(trozos[0]);      // Numero de jugadores
            string[] datos = trozos[1].Split(',');          // Datos de las partidas


            // Rellenamos la tabla con los datos de las partidas
            dataGridResultadoPartida.Rows.Add(numj);
            dataGridResultadoPartida.BackgroundColor = Color.FromArgb(213, 79, 10);
            for (int i = 0; i < numj; i++)
            {
                dataGridResultadoPartida.Rows[i].Cells[0].Value = datos[3*i + 0];     // Posición del jugador
                dataGridResultadoPartida.Rows[i].Cells[1].Value = datos[3*i + 1];     // Nombre del jugador
                dataGridResultadoPartida.Rows[i].Cells[2].Value = datos[3*i + 2];     // Puntos del jugador
                
            }
            
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            dataGridResultadoPartida.Rows.Clear();
            this.Hide();
        }
        private void dataGridJugadores_SelectionChanged(object sender, EventArgs e)
        {
            dataGridResultadoPartida.ClearSelection();
        }
    }
}
