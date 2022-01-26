using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using cliente.Partida;



namespace cliente.Menu
{
    public partial class TabMenuPrincipal : TabMenu
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
            // Configuración inicial de la tabla de partidas
            dataGridPartidas.RowHeadersVisible = false;
            dataGridPartidas.Columns.Add("ID", "ID");
            dataGridPartidas.Columns.Add("Posición", "Posición");
            dataGridPartidas.Columns.Add("Puntos", "Puntos");
            dataGridPartidas.Columns.Add("Fecha y Hora", "Fecha y Hora");
            dataGridPartidas.Columns.Add("Duración", "Duración");
            dataGridPartidas.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            dataGridPartidas.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            // Configuracion inicial de la lista de jugadores conectados
            dataGridJugadores.RowHeadersVisible = false;
            dataGridJugadores.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            dataGridJugadores.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridJugadores.ColumnHeadersVisible = false;
            dataGridJugadores.Columns.Add("Nombre","Nombre");
            dataGridJugadores.Columns[0].Width = dataGridJugadores.Width;
            dataGridJugadores.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// Envia al servidor la peticion de lista de partidas del jugador
        /// </summary>
        private void btnPartidas_Click(object sender, EventArgs e)
        {
            string pet = "3/" + idJ.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }

        /// <summary>
        /// Envia al servidor la peticion de obtener la media de puntos del jugador
        /// </summary>
        private void btnMedia_Click(object sender, EventArgs e)
        {
            string pet = "5/" + idJ.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }

        /// <summary>
        /// Envia al servidor la peticion de crear un lobby
        /// </summary>
        private void btnCrearLobby_Click(object sender, EventArgs e)
        {
            string pet = "7/";
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }

        /// <summary>
        /// Envia al servidor la peticion de desconexión
        /// </summary>
        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            this.Tag = "DESCONECTAR";
            this.Hide();
        }

        /// <summary>
        /// Envia al servidor la peticion para obtener la participación de una partida seleccionada
        /// </summary>
        private void dataGridPartidas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            // Se ha clickado en el header (no hay envio de la peticion)
            if (row == -1) return;

            this.idP = Convert.ToInt32(dataGridPartidas.Rows[row].Cells[0].Value);
            string pet = "4/" + this.idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
            this.Tag = "INFO PARTIDA";
            this.Hide();
        }

        /// <summary>
        /// Actualiza el datagridview de partidas con los datos de las partidas del jugador
        /// </summary>
        /// <param name="res"> String de la forma: #partidas/idP,posicion,puntos,fechahora,duracion,... </param>       
        public void ActualizarDataGrid(string res)
        {
            try
            {
                // Ahora primero separamos el numero de partidas jugadas
                // de los datos de cada partida
                string[] trozos = res.Split("/", 2);        // Tendremos 2 trozos
                int nump = Convert.ToInt32(trozos[0]);      // Numero de partidas

                // Ha habido un error al hacer la consulta a la base de datos
                if (nump == -1)
                {
                    MessageBox.Show("Ha habido un error. Inténtelo de nuevo.", "Servidor",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // No hay partidas registradas al jugador
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
                    dataGridPartidas.Rows[i].Cells[0].Value = datos[5 * i + 0];     // Id Partida
                    dataGridPartidas.Rows[i].Cells[1].Value = datos[5 * i + 1];     // Posicion del jugador
                    dataGridPartidas.Rows[i].Cells[2].Value = datos[5 * i + 2];     // Puntos del jugador
                    dataGridPartidas.Rows[i].Cells[3].Value = datos[5 * i + 3];     // Fecha y hora de la partida
                    dataGridPartidas.Rows[i].Cells[4].Value = datos[5 * i + 4];     // Duracion de la partida
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza el label de información de la media de puntos del jugador
        /// </summary>
        /// <param name="res"> String de la media de puntos </param>   
        public void ActualizarMedia(string res)
        {
            // Si no tiene puntos porque nunca ha jugado
            if (res == "-1.00")
                lblMedia.Text = "Puntuación media: N/A";
            // Ha jugado alguna partida pero nunca obtuvo ningún punto
            else
                lblMedia.Text = "Puntuación media: " + res;
        }

        /// <summary>
        /// Actualiza el datagridview de la lista de conectados
        /// </summary>
        /// <param name="res"> String de la forma: #jug/nombre1,nombre2,nombre3... </param>   
        public void ActualizarListaConectados(string res)
        {
            try
            {
                // Primero separamos el numero de juadores de los datos de cada jugador
                string[] trozos = res.Split("/", 2);        // Tendremos 2 trozos
                int nump = Convert.ToInt32(trozos[0]);      // Numero de jugadores
                string[] datos = trozos[1].Split(',');      // Nombres de los jugadores

                // Rellenamos la tabla con los nombres de los jugadores
                dataGridJugadores.Rows.Clear();
                dataGridJugadores.Rows.Add(nump);
                for (int i = 0; i < nump; i++)
                {
                    dataGridJugadores.Rows[i].Cells[0].Value = datos[i];     // Nombre del jugador
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Abre el formulario con el manual de reglas del juego
        /// </summary>
        private void btnManual_Click(object sender, EventArgs e)
        {
            FormManual form = new FormManual();
            form.Show();
        }
    }
}
