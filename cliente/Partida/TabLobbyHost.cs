using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Drawing.Drawing2D;


namespace cliente.Partida
{
    public partial class TabLobbyHost : Tab
    {
        int numFormP;
        Socket conn;
        List<ColorJugador> coloresDisponibles = new List<ColorJugador>(6);
        public ColorJugador miColor;
        int invitadoSeleccionado = 0;       //nos indica la fila en la que se encuentra el nombre del invitado, 0 si no se ha seleccionado ninguno

        public TabLobbyHost(Socket conn)
        {
            InitializeComponent();
            this.conn = conn;
            //FALTA passar socket, numformpartida etc
        }

        public Color DameColor(ColorJugador color)
        {
            if (color == ColorJugador.Verde)
            {
                return Color.FromArgb(111, 145, 111);
            }
            else if (color == ColorJugador.Morado)
            {
                return Color.FromArgb(178, 95, 211);
            }
            else if (color == ColorJugador.Gris)
            {
                return Color.FromArgb(200, 190, 183);
            }
            else if (color == ColorJugador.Naranja)
            {
                return Color.FromArgb(225, 132, 13);
            }
            else if (color == ColorJugador.Rojo)
            {
                return Color.FromArgb(160, 44, 44);
            }
            else if (color == ColorJugador.Azul)
            {
                return Color.FromArgb(95, 171, 200);
            }
            else
                return Color.FromArgb(230, 154, 100);
        }
        public ColorJugador DameColorJugador(Color color)
        {
            if (color == Color.FromArgb(111, 145, 111))
            {
                return ColorJugador.Verde;
            }
            else if (color == Color.FromArgb(178, 95, 211))
            {
                return ColorJugador.Morado;
            }
            else if (color == Color.FromArgb(200, 190, 183))
            {
                return ColorJugador.Gris;
            }
            else if (color == Color.FromArgb(225, 132, 13))
            {
                return ColorJugador.Naranja;
            }
            else if (color == Color.FromArgb(160, 44, 44))
            {
                return ColorJugador.Rojo;
            }
            else if (color == Color.FromArgb(95, 171, 200))
            {
                return ColorJugador.Azul;
            }
            else
                return ColorJugador.Gris;
            
        }
        
        private void TabLobbyHost_Load(object sender, EventArgs e)
        {
            this.btnC1.Hide();
            this.btnC2.Hide();
            this.btnC3.Hide();
            this.btnC4.Hide();
            this.btnC5.Hide();
            this.btnC6.Hide();

            this.BackColor = Color.FromArgb(195, 96, 63);
            panelJugadores.BackColor = Color.White;
            panelConectados.BackColor = Color.White;
            btnInvitar.BackColor = Color.White;
            dataGridJugadores.BackgroundColor = Color.White;
            dataGridConectados.BackgroundColor = Color.White;


            dataGridJugadores.RowHeadersVisible = false;
            dataGridJugadores.Columns.Add("Nombres","Nombres");
            dataGridJugadores.Columns[0].Width = dataGridJugadores.Width / 2;
            dataGridJugadores.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridJugadores.Columns.Add("Colores", "Colores");
            dataGridJugadores.Columns[1].Width = dataGridJugadores.Width / 2;
            dataGridJugadores.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridJugadores.Rows.Add(4);
            dataGridJugadores.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            dataGridJugadores.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridJugadores.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridJugadores.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridConectados.RowHeadersVisible = false;
            dataGridConectados.Columns.Add("Nombres", "Nombres");
            dataGridConectados.Columns[0].Width = dataGridJugadores.Width;
            dataGridConectados.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridConectados.RowsDefaultCellStyle.SelectionBackColor = Color.White;
            dataGridConectados.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridConectados.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridConectados.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridConectados.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridConectados.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            //DADES D'EXEMPLE---------------------------------------------------------------------------//

            miColor = ColorJugador.Morado;
            dataGridJugadores.Rows[0].Cells[0].Value = "Alba";
            dataGridJugadores.Rows[1].Cells[0].Value = "Jonathan";     
            dataGridJugadores.Rows[2].Cells[0].Value = "Raul";     
            dataGridJugadores.Rows[3].Cells[0].Value = "Maria"; 
            dataGridJugadores.Rows[0].Cells[1].Style.BackColor = DameColor(miColor);
            dataGridJugadores.Rows[1].Cells[1].Style.BackColor = DameColor(ColorJugador.Verde);
            dataGridJugadores.Rows[2].Cells[1].Style.BackColor = DameColor(ColorJugador.Azul);
            dataGridJugadores.Rows[3].Cells[1].Style.BackColor = DameColor(ColorJugador.Rojo);
            dataGridJugadores.Rows[0].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[1].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[2].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[3].Height = dataGridJugadores.Height / 5;

            coloresDisponibles.Add(ColorJugador.Rojo);
            coloresDisponibles.Add(ColorJugador.Gris);
            coloresDisponibles.Add(ColorJugador.Morado);
            coloresDisponibles.Add(ColorJugador.Azul);
            coloresDisponibles.Add(ColorJugador.Naranja);
            coloresDisponibles.Add(ColorJugador.Verde);

            string res = "4/Alba,Jonathan,Raul,Maria";
            this.ActualizarListaConectados(res);
            //------------------------------------------------------------------------------------------//
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Tag = "EMPEZAR";
            this.Hide();
        }

        private void btnCambioColor_Click(object sender, EventArgs e)
        {
            Button[] btns = {btnC1, btnC2, btnC3, btnC4, btnC5, btnC6};
            for (int i = 0; i < coloresDisponibles.Count; i++)
            {
                btns[i].BackColor = DameColor(coloresDisponibles[i]);
                btns[i].Show();
                btns[i].Click += new System.EventHandler(this.CambioColorRealizado);
            }
        }
        private void btnInvitar_Click(object sender, EventArgs e)
        {
            if (invitadoSeleccionado != 0)
            {
                string pet = "8/" + numFormP.ToString() + "/" + dataGridConectados.Rows[invitadoSeleccionado].Cells[0].Value.ToString();
                MessageBox.Show(pet);
                //byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                //conn.Send(pet_b);
            }
            else
                MessageBox.Show("Debes seleccionar un Jugador de la lista de conectados");
        }
    
        private void CambioColorRealizado(object sender, EventArgs e)
        {
            Button[] btns = { btnC1, btnC2, btnC3, btnC4, btnC5, btnC6 };
            Button btn = (Button)sender;
            string pet = "12/" + numFormP.ToString() + "/" + DameColorJugador(btn.BackColor).ToString();
            MessageBox.Show(pet);
            //byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            //conn.Send(pet_b);
            this.miColor = DameColorJugador(btn.BackColor);
            dataGridJugadores.Rows[0].Cells[1].Style.BackColor = btn.BackColor;
            for (int i = 0; i < 6; i++)
            {
                btns[i].Hide();
            }
        }

        public void ActualizarListaConectados(string res)
        {
            try
            {
                // Primero separamos el numero de juadores
                // de los datos de cada jugador
                string[] trozos = res.Split("/", 2);        // Tendremos 2 trozos
                int nump = Convert.ToInt32(trozos[0]);      // Numero de jugadores

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
                string[] datos = trozos[1].Split(',');          // Nombres de los jugadores

                // Rellenamos la tabla con los nombres de los jugadores
                dataGridConectados.Rows.Clear();
                dataGridConectados.Rows.Add(nump);
                for (int i = 0; i < nump; i++)
                {
                    dataGridConectados.Rows[i].Cells[0].Value = datos[i];     // Nombre del jugador
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ActualizarListaJugadores(string res)
        {
            try
            {
                string[] datos = res.Split(',');          // Nombres y colores de los jugadores

                // Rellenamos la tabla con los nombres de los jugadores
                dataGridJugadores.Rows.Clear();
                dataGridJugadores.Rows.Add(4);
                dataGridJugadores.Rows[0].Height = dataGridJugadores.Height / 5;
                dataGridJugadores.Rows[1].Height = dataGridJugadores.Height / 5;
                dataGridJugadores.Rows[2].Height = dataGridJugadores.Height / 5;
                dataGridJugadores.Rows[3].Height = dataGridJugadores.Height / 5;
                int i = 0;
                int count = 0;
                while(i < datos.Length/2)
                {
                    dataGridJugadores.Rows[i].Cells[0].Value = datos[count];     // Nombre del jugador
                    //dataGridJugadores.Rows[i].Cells[1].Style.BackColor = DameColor(Convert.ToInt32(datos[count + 1]));     // Nombre del jugador
                    //listadisponibles
                    i = i + 1;
                    count = count + 2;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridConectados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (invitadoSeleccionado == e.RowIndex && dataGridConectados.RowsDefaultCellStyle.SelectionBackColor == Color.AntiqueWhite)
            {
                dataGridConectados.RowsDefaultCellStyle.SelectionBackColor = Color.White;
                invitadoSeleccionado = 0;
            }
            else
            {
                invitadoSeleccionado = e.RowIndex;
                dataGridConectados.RowsDefaultCellStyle.SelectionBackColor = Color.AntiqueWhite;
            }
                
        }

    }
   
}
