using System;
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

        public string[] nombres;
        public ColorJugador[] colores;

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

        private void btnCambioColor_Click(object sender, EventArgs e)
        {
            // Muestra los colores disponibles
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

        private void btnColor_Click(object sender, EventArgs e)
        {
            // petición de cambio de color 
            Button btn = (Button)sender;
            string pet = "12/" + idP.ToString() + "/" + ((int)Colores.DameColorJugador(btn.BackColor)).ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            // Inhabilita el boton
            foreach (Button but in btns)
            {
                but.Hide();
            }
        }

        /// <summary>
        /// Actualiza el datagrid de la lista de jugadores en el lobby
        /// </summary>
        /// <param name="res"> String de la forma: host,color,guest1,color,guest2,color,
        /// guest3,color... </param>
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

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            // Esconde el tab y cambia el tag para desconectar de la partida
            this.Tag = "DESCONECTAR";
            this.Hide();
        }

        /// <summary>
        /// Escibe en el textbox de chat los mensajes recibidos del servidor con tal fin
        /// </summary>
        /// <param name="res"> Mensaje recibido </param>
        public void ActualizarChat(string res)
        {
            // Si no empieza contiene ":" es porque es una notificación de la partida
            if (res.IndexOf(":") == -1)
            {
                txtChat.SelectionFont = new Font("Segoe UI", 9, FontStyle.Italic);
                txtChat.SelectionColor = Color.SkyBlue;
            }
            // Si lo contiene es porque es un mensaje de algún jugador
            else
            {
                txtChat.SelectionFont = new Font("Segoe UI", 9, FontStyle.Regular);
                txtChat.ForeColor = Color.Black;
            }
            txtChat.SelectedText = res;
            txtChat.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Envía mensaje del chat del jugador al servidor
        /// </summary>
        public void EnviarMensaje()
        {
            // Texto no puede estar vacío
            if (txtMsg.Text != "")
            {
                // Petición mensaje de chat
                string pet = "13/" + idP.ToString() + "/" + nombre + ": " + txtMsg.Text;
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                txtMsg.Clear();
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // Usa el método de enviar mensaje
            EnviarMensaje();
        }

        private void txtMsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si se apreta Enter se usa el método enviar mensaje
            if (e.KeyChar == (char)Keys.Return)
                EnviarMensaje();
        }
        private void dataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Configura el estilo de celda en función de la acción sobre esta
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
            // Si se elige otro jugador se reconfigura el estilo por defecto
            dataGridJugadores.ClearSelection();
        }
    }

}
