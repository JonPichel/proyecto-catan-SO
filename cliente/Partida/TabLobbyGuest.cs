﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Linq;


namespace cliente.Partida
{
    public partial class TabLobbyGuest : TabPartida
    {
        Socket conn;
        int idP;
        string nombre;

        public ColorJugador miColor;

        string[] nombres;
        ColorJugador[] colores;

        Button[] btns;

        public TabLobbyGuest(Socket conn, int idP, string nombre)
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
            dataGridJugadores.ClearSelection();

            dataGridJugadores.Rows[0].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[1].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[2].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[3].Height = dataGridJugadores.Height / 5;

            // Enviar respuesta
            string pet = "9/" + idP.ToString() + "/SI";
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }


        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            this.Tag = "EMPEZAR";
            this.miColor = colores[0];
            this.Hide();
        }

        private void btnCambioColor_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ColorJugador color in Enum.GetValues(typeof(ColorJugador)))
            {
                if (!colores.Contains(color))
                {
                    btns[i].BackColor = DameColor(color);
                    btns[i].Show();
                    i++;
                }
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string pet = "12/" + idP.ToString() + "/" + ((int)DameColorJugador(btn.BackColor)).ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            foreach (Button but in btns)
            {
                but.Hide();
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
                        dataGridJugadores.Rows[i].Cells[1].Style.BackColor = DameColor(color);
                    } else
                    {
                        nombres[i] = "";
                        colores[i] = (ColorJugador)(-1);

                        dataGridJugadores.Rows[i].Cells[0].Value = null;
                        dataGridJugadores.Rows[i].Cells[1].Style.BackColor = Color.White;
                    }
                }
                dataGridJugadores.CellPainting += new DataGridViewCellPaintingEventHandler(this.dataGrid_CellPainting);
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha perdido la conexión con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Color DameColor(ColorJugador color)
        {
            switch (color)
            {
                case ColorJugador.Azul:
                    return Color.FromArgb(95, 171, 200);
                case ColorJugador.Rojo:
                    return Color.FromArgb(160, 44, 44);
                case ColorJugador.Naranja:
                    return Color.FromArgb(225, 132, 13);
                case ColorJugador.Gris:
                    return Color.FromArgb(200, 190, 183);
                case ColorJugador.Morado:
                    return Color.FromArgb(178, 95, 211);
                case ColorJugador.Verde:
                    return Color.FromArgb(111, 145, 111);
                default:
                    return Color.FromArgb(95, 171, 200);
            }
        }
        private ColorJugador DameColorJugador(Color color)
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
            int index = 0;
            for(int i = 0; i<nombres.Length; i++)
            {
                if (nombres[i] == nombre)
                    index = i;
            }
            if (e.ColumnIndex == 0 & e.RowIndex == index)
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
