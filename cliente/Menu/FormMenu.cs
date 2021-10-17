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
    public partial class FormMenu : Form
    {
        Socket conn;
        int idJ;
        int idP;

        public FormMenu(Socket conn, int idJ)
        {
            InitializeComponent();
            this.conn = conn;
            this.idJ = idJ;
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            dataGridPartidas.RowHeadersVisible = false;
            dataGridPartidas.Columns.Add("ID", "ID");
            dataGridPartidas.Columns.Add("Posición", "Posición");
            dataGridPartidas.Columns.Add("Puntos", "Puntos");
            dataGridPartidas.Columns.Add("Fecha y Hora", "Fecha y Hora");
            dataGridPartidas.Columns.Add("Duración", "Duración");
            dataGridPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            try
            {
                string pet = "5/" + idJ.ToString();
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);

                byte[] res_b = new byte[512];
                conn.Receive(res_b);
                string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

                lblMedia.Text = "Puntuación media: " + res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No ha jugado aún ninguna partida.");
            }
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
                // Ahora primero separamos el numero de partidas juagadas
                // de los datos de cada partida
                string[] trozos = res.Split(new string[] { "/" }, 2, StringSplitOptions.None);  // Tendremos 2 trozos

                int partidas = Convert.ToInt32(trozos[0]);      // Numero de partidas


                if (partidas != 0)
                {
                    string[] datos = trozos[1].Split(',');          // Datos de las partidas

                    // Rellenamos la tabla con los datos de las partidas
                    dataGridPartidas.Rows.Add(partidas);
                    int j = 0;
                    for (int i = 0; i < partidas; i++)
                    {
                        dataGridPartidas.Rows[i].Cells[0].Value = datos[j];         // Id Partida
                        dataGridPartidas.Rows[i].Cells[1].Value = datos[j + 1];     // Posicion del jugador
                        dataGridPartidas.Rows[i].Cells[2].Value = datos[j + 2];     // Puntos del jugador
                        dataGridPartidas.Rows[i].Cells[3].Value = datos[j + 3];     // Fecha y hora de la partida
                        dataGridPartidas.Rows[i].Cells[4].Value = datos[j + 4];     // Duracion de la partida
                        j += 5;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("No ha jugado aún ninguna partida.");
            }
                

        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            // Cerramos el form y por tanto se cerrará la conexión
            this.Close();
        }

        private void dataGridPartidas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            this.idP = Convert.ToInt32(dataGridPartidas.Rows[row].Cells[0].Value);

            Form formInfoPartida = new FormInfoPartida(conn, idP);
            this.Hide();
            formInfoPartida.ShowDialog();
            this.Show();
        }
    }
}
