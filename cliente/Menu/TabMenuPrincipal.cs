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
    public partial class TabMenuPrincipal : Tab
    {
        Socket conn;
        public int idJ { private get; set; }
        public int idP { get; private set; }

        public TabMenuPrincipal(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void TabMenuPrincipal_Load(object sender, EventArgs e)
        {
            dataGridPartidas.RowHeadersVisible = false;
            dataGridPartidas.Columns.Add("ID", "ID");
            dataGridPartidas.Columns.Add("Posición", "Posición");
            dataGridPartidas.Columns.Add("Puntos", "Puntos");
            dataGridPartidas.Columns.Add("Fecha y Hora", "Fecha y Hora");
            dataGridPartidas.Columns.Add("Duración", "Duración");
            dataGridPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dataGridJugadores.RowHeadersVisible = false;
            dataGridJugadores.Columns.Add("ID", "ID");
            dataGridJugadores.Columns.Add("Nombre", "Nombre");
            dataGridJugadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void btnPartidas_Click(object sender, EventArgs e)
        {
            string pet = "3/" + idJ.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[512];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            try
            {
                // Ahora primero separamos el numero de partidas jugadas
                // de los datos de cada partida
                string[] trozos = res.Split("/", 2);  // Tendremos 2 trozos
                int nump = Convert.ToInt32(trozos[0]);      // Numero de partidas

                if (nump == -1)
                {
                    MessageBox.Show("Ha habido un error. Inténtelo de nuevo.", "Servidor",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (nump == 0)
                {
                    MessageBox.Show("No hay partidas para mostrar.", "Servidor",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string[] datos = trozos[1].Split(',');          // Datos de las partidas

                // Rellenamos la tabla con los datos de las partidas
                dataGridPartidas.Rows.Clear();
                dataGridPartidas.Rows.Add(nump);
                for (int i = 0; i < nump; i++)
                {
                    dataGridPartidas.Rows[i].Cells[0].Value = datos[5*i + 0];     // Id Partida
                    dataGridPartidas.Rows[i].Cells[1].Value = datos[5*i + 1];     // Posicion del jugador
                    dataGridPartidas.Rows[i].Cells[2].Value = datos[5*i + 2];     // Puntos del jugador
                    dataGridPartidas.Rows[i].Cells[3].Value = datos[5*i + 3];     // Fecha y hora de la partida
                    dataGridPartidas.Rows[i].Cells[4].Value = datos[5*i + 4];     // Duracion de la partida
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {

        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            this.Tag = "DESCONECTAR";
            this.Hide();
        }

        private void dataGridPartidas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            // Se ha clickado en el header
            if (row == -1) return;

            this.idP = Convert.ToInt32(dataGridPartidas.Rows[row].Cells[0].Value);
            this.Tag = "INFO PARTIDA";
            this.Hide();
        }

        private void btnJugOnline_Click(object sender, EventArgs e)
        {
            //RES: #jug/id1,nombre1,id2,nombre2,id3,nombre3.
            // Petición lista de jugadores online
            string pet = "6/";
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[512];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            try
            {
                // Primero separamos el numero de juadores
                // de los datos de cada jugador
                string[] trozos = res.Split("/", 2);        // Tendremos 2 trozos
                int nump = Convert.ToInt32(trozos[0]);      // Numero de partidas

                if (nump == -1)
                {
                    MessageBox.Show("Ha habido un error. Inténtelo de nuevo.", "Servidor",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (nump == 0)
                {
                    MessageBox.Show("No hay partidas para mostrar.", "Servidor",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string[] datos = trozos[1].Split(',');          // Datos de las partidas

                // Rellenamos la tabla con los datos de las partidas
                dataGridJugadores.Rows.Clear();
                dataGridJugadores.Rows.Add(nump);
                for (int i = 0; i < nump; i++)
                {
                    dataGridJugadores.Rows[i].Cells[0].Value = datos[2 * i + 0];     // Id Jugador
                    dataGridJugadores.Rows[i].Cells[1].Value = datos[2 * i + 1];     // Nombre del jugador
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
