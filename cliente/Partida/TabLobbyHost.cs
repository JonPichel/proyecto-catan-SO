using System;
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
        int numJug;

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
            numJug = 0;

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
            numJug = 0;
            int a = 0;
            // No pueden haber más de 4 jugadores
            while (a < 4)
            {
                if (dataGridJugadores.Rows[a].Cells[0].Value != null)
                    numJug++;
                a++;
            } 
            // Deben haber más de 1 jugador    
            if(numJug > 1)
            {
                // Cambiar tag y petición empezar partida
                this.Tag = "EMPEZAR";
                string pet = "14/" + idP.ToString();
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
            }
            else
            {
                MessageBox.Show("Debe haber un mínimo de 2 jugadores para jugar.");
            }
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

        private void btnInvitar_Click(object sender, EventArgs e)
        {
            // Envia una invitación al jugador seleccionado de la lista de conectados
            foreach (DataGridViewCell celda in dataGridConectados.SelectedCells)
            {
                // Si la celda esta vacía no hace nada
                if (celda.Value == null)
                {
                    dataGridConectados.ClearSelection();
                    return;
                }
                // Se tiene que invitar a alguien que no este en la partida
                if(!nombres.Contains(celda.Value.ToString()))
                {
                    // Petición de invitación
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
            // Petición de cambio de color
            Button btn = (Button)sender;
            string pet = "12/" + idP.ToString() + "/" + ((int)Colores.DameColorJugador(btn.BackColor)).ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            // Esconde el color seleccionado de los disponibles
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
            // Se debe seleccionar más de una celda para poder invitar
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
            // Cambiar tag y esconder tab para seguida desconexión de la partida
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
        /// Informa al host de si la invitación a sido aceptada o rechazada
        /// </summary>
        /// <param name="res"> String de la forma: idPartida/guest/SI o NO </param>
        public void RespuestaInvitacion(string res) {
            string[] trozos = res.Split("/");
            // Se indica si se ha rechazado
            if (trozos[2] == "NO")
            {
                MessageBox.Show(trozos[1] + " ha rechazado tu invitación");
            }
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
            // Si se apreta Enter usa el método de enviar mensaje
            if (e.KeyChar == (char)Keys.Return)
                EnviarMensaje();
        }

        private void dataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Configura el estilo de celda en función de la acción sobre esta
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
            // Si se elige otro jugador se reconfigura el estilo por defecto
            dataGridJugadores.ClearSelection();
        }
    }
}

