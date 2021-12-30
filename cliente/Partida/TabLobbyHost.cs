﻿using System;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;


namespace cliente.Partida
{
    public partial class TabLobbyHost : TabPartida
    {
        Socket conn;
        int idP;
        string nombre;

        public string[] nombres;
        public ColorJugador[] colores;

        Button[] btns;

        public TabLobbyHost(Socket conn, int idP, string nombre)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;

            nombres = new string[4];
            colores = new ColorJugador[4] { (ColorJugador)(-1), (ColorJugador)(-1), (ColorJugador)(-1), (ColorJugador)(-1) };
        }

        private void TabLobbyHost_Load(object sender, EventArgs e)
        {
            btns = new Button[] { btnC1, btnC2, btnC3, btnC4, btnC5, btnC6 };
            foreach (Button btn in btns)
            {
                btn.Hide();
                btn.Click += this.btnColor_Click;
            }

            // Formato dataGridJugadores
            dataGridJugadores.RowHeadersVisible = false;
            dataGridJugadores.Columns.Add("Nombres", "Nombres");
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
            dataGridJugadores.CellPainting += new DataGridViewCellPaintingEventHandler(this.dataGrid_CellPainting);
            dataGridJugadores.ClearSelection();

            dataGridJugadores.Rows[0].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[1].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[2].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[3].Height = dataGridJugadores.Height / 5;

            // Formato dataGridConectados
            dataGridConectados.ColumnHeadersVisible = false;
            dataGridConectados.RowHeadersVisible = false;
            dataGridConectados.Columns.Add("Nombres", "Nombres");
            dataGridConectados.Columns.Add("Nombres2", "Nombres2");
            dataGridConectados.Columns[0].Width = dataGridConectados.Width/2;
            dataGridConectados.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridConectados.Columns[1].Width = dataGridConectados.Width/2;
            dataGridConectados.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridConectados.RowsDefaultCellStyle.SelectionBackColor = Color.AntiqueWhite;
            dataGridConectados.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridConectados.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridConectados.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridConectados.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridConectados.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            btnInvitar.Enabled = false;
        }


        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            this.Tag = "EMPEZAR";
            string pet = "14/" + idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }

        private void btnCambioColor_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ColorJugador color in Enum.GetValues(typeof(ColorJugador)))
            {
                if (!colores.Contains(color))
                {
                    btns[i].BackColor = Colores.DameColor(color);
                    btns[i].Show();
                    i++;
                }
            }
        }

        private void btnInvitar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell celda in dataGridConectados.SelectedCells)
            {
                if (celda.Value == null)
                {
                    dataGridConectados.ClearSelection();
                    return;
                }
                if(!nombres.Contains(celda.Value.ToString()))
                {
                    string pet = "8/" + idP.ToString() + "/" + celda.Value.ToString();
                    byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                    conn.Send(pet_b);
                    Thread.Sleep(500);
                }
            }
            dataGridConectados.ClearSelection();
            btnInvitar.Enabled = false;
        }
    
        private void btnColor_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string pet = "12/" + idP.ToString() + "/" + ((int)Colores.DameColorJugador(btn.BackColor)).ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            foreach (Button but in btns)
            {
                but.Hide();
            }
        }

        public void ActualizarListaConectados(string res)
        {
            try
            {
                string[] trozos = res.Split("/", 2);        // Tendremos 2 trozos
                int numj = Convert.ToInt32(trozos[0]);      // Numero de jugadores

                string[] datos = trozos[1].Split(',');      // Nombres de los jugadores

                // Rellenamos la tabla con los nombres de los jugadores
                dataGridConectados.Rows.Clear();
                dataGridConectados.Rows.Add((int)Math.Ceiling(numj / 2.0));
                for (int i = 0; i < numj; i++)
                {
                    dataGridConectados.Rows[i / 2].Cells[i % 2].Value = datos[i];     // Nombre del jugador
                }

                dataGridConectados.ClearSelection();
                btnInvitar.Enabled = false;
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

                ColorJugador color;
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (i < datos.Length / 2)
                    {
                        nombres[i] = datos[2 * i];
                        color = (ColorJugador)Convert.ToInt32(datos[2 * i + 1]);
                        colores[i] = color;

                        dataGridJugadores.Rows[i].Cells[0].Value = datos[2 * i];
                        dataGridJugadores.Rows[i].Cells[1].Style.BackColor = Colores.DameColor(color);
                    }
                    else
                    {
                        nombres[i] = "";
                        colores[i] = (ColorJugador)(-1);
                        dataGridJugadores.Rows[i].Cells[0].Value = null;
                        dataGridJugadores.Rows[i].Cells[1].Style.BackColor = Color.White;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridConectados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridConectados.SelectedCells.Count > 0)
            {
                btnInvitar.Enabled = true;
            } else
            {
                btnInvitar.Enabled = false;
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            this.Tag = "DESCONECTAR";
            this.Hide();
        }

        public void ActualizarChat(string res)
        {
            if (res.IndexOf(":") == -1)
            {
                txtChat.SelectionFont = new Font("Segoe UI", 9, FontStyle.Italic);
                txtChat.SelectionColor = Color.SkyBlue;
            }
            else
            {
                txtChat.SelectionFont = new Font("Segoe UI", 9, FontStyle.Regular);
                txtChat.ForeColor = Color.Black;              
            }
            txtChat.SelectedText = res;
            txtChat.AppendText(Environment.NewLine);
        }

        public void RespuestaInvitacion(string res) {
            string[] trozos = res.Split("/");
            if (trozos[2] == "NO")
            {
                MessageBox.Show(trozos[1] + " ha rechazado tu invitación");
            }
        }

        public void EnviarMensaje()
        {
            if (txtMsg.Text != "")
            {
                string pet = "13/" + idP.ToString() + "/" + nombre + ": " + txtMsg.Text;
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                txtMsg.Clear();
            }
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            EnviarMensaje();
        }

        private void txtMsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                EnviarMensaje();
        }

        private void dataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 & e.RowIndex == 0)
            {
                //Pen for left and top borders
                using (var backGroundPen = new Pen(e.CellStyle.BackColor, 1))
                //Pen for bottom and right borders
                using (var gridlinePen = new Pen(dataGridJugadores.GridColor, 1))
                //Pen for selected cell borders
                using (var selectedPen = new Pen(Color.FromArgb(195, 96, 63), 1))
                {
                    var topLeftPoint = new Point(e.CellBounds.Left, e.CellBounds.Top);
                    var topRightPoint = new Point(e.CellBounds.Right - 1, e.CellBounds.Top);
                    var bottomRightPoint = new Point(e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    var bottomleftPoint = new Point(e.CellBounds.Left, e.CellBounds.Bottom - 1);
                    //Paint all parts except borders.
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                    //Draw selected cells border here
                    e.Graphics.DrawRectangle(selectedPen, new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1));

                    //Handled painting for this cell, Stop default rendering.
                    e.Handled = true;
                }
            }
        }

        private void dataGridJugadores_SelectionChanged(object sender, EventArgs e)
        {
            dataGridJugadores.ClearSelection();
        }
    }
}

