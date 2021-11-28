using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Net.Sockets;

namespace cliente.Partida
{
    public partial class TabTablero : TabPartida
    {
        Socket conn;
        int idP;
        string nombre;
        public string[] nombres;
        public ColorJugador[] colores;
        int numMonopolios = 2;
        int numInventos = 3;

        Tile[] tiles;
        List<FichaVertice> fichasVertices;
        Puerto[] puertos;
        List<Carretera> carreteras;
        LadoCoords[] ladosTablero;
        VerticeCoords[] verticesTablero;
        LadoCoords[] ladosPosibles;
        VerticeCoords[] verticesPosibles;

        int Ronda;
        public ColorJugador colorJugador;

        int zoomLevel;
        Point basePoint;
        Point oldMouse;
        
        enum Estado
        {
            Normal,
            ClickCasilla,
            ColocarPoblado,
            ColocarCarretera
        };

        Estado estado;
        FichaVertice verticeColocar;
        Carretera carreteraColocar;

        Bitmap[] numbers = new Bitmap[]
        {
            cliente.Properties.Resources._1,
            cliente.Properties.Resources._2,
            cliente.Properties.Resources._3,
            cliente.Properties.Resources._4,
            cliente.Properties.Resources._5,
            cliente.Properties.Resources._6,
            cliente.Properties.Resources._7,
            cliente.Properties.Resources._8,
            cliente.Properties.Resources._9,
            cliente.Properties.Resources._10,
            cliente.Properties.Resources._11,
            cliente.Properties.Resources._12,
        };

        public TabTablero(Socket conn, int idP, string nombre)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;

            nombres = new string[4];
            colores = new ColorJugador[4] { (ColorJugador)(-1), (ColorJugador)(-1), (ColorJugador)(-1), (ColorJugador)(-1) };
        }

        public void CargarTablero(string[] trozos)
        {
            string[] casillas = trozos[0].Split(",");
            string[] datosPuertos = trozos[1].Split(",");

            // Poblar casillas
            List<Tile> tiles = new List<Tile>()
            {
                new TileMar(0, -3),
                new TileMar(1, -3),
                new TileMar(2, -3),
                new TileMar(3, -3),
                new TileMar(-1, -2),
                new TileMar(3, -2),
                new TileMar(-2, -1),
                new TileMar(3, -1),
                new TileMar(-3, 0),
                new TileMar(3, 0),
                new TileMar(-3, 1),
                new TileMar(2, 1),
                new TileMar(-3, 2),
                new TileMar(1, 2),
                new TileMar(-3, 3),
                new TileMar(-2, 3),
                new TileMar(-1, 3),
                new TileMar(0, 3)
            };
            
            Queue<int> valores = new Queue<int>(new int[] { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 });
            HexCoords coords;
            int i = 0;
            for (int radius = 2; radius > 0; radius--)
            {
                coords = new HexCoords(-radius, radius);
                for (int j = 0; j < 6; j++)
                {
                    for (int k = 0; k < radius; k++)
                    {
                        switch (casillas[i++])
                        {
                            case "DESIERTO":
                                tiles.Add(new TileDesierto(coords.Q, coords.R));
                                break;
                            case "MADERA":
                                tiles.Add(new TileMadera(coords.Q, coords.R, valores.Dequeue()));
                                break;
                            case "LADRILLO":
                                tiles.Add(new TileLadrillo(coords.Q, coords.R, valores.Dequeue()));
                                break;
                            case "OVEJA":
                                tiles.Add(new TileOveja(coords.Q, coords.R, valores.Dequeue()));
                                break;
                            case "TRIGO":
                                tiles.Add(new TileTrigo(coords.Q, coords.R, valores.Dequeue()));
                                break;
                            case "PIEDRA":
                                tiles.Add(new TilePiedra(coords.Q, coords.R, valores.Dequeue()));
                                break;
                        }
                        coords = coords.Vecinos[j];
                    }
                }
            }
            switch (casillas[i])
            {
                case "DESIERTO":
                    tiles.Add(new TileDesierto(0, 0));
                    break;
                case "MADERA":
                    tiles.Add(new TileMadera(0, 0, valores.Dequeue()));
                    break;
                case "LADRILLO":
                    tiles.Add(new TileLadrillo(0, 0, valores.Dequeue()));
                    break;
                case "OVEJA":
                    tiles.Add(new TileOveja(0, 0, valores.Dequeue()));
                    break;
                case "TRIGO":
                    tiles.Add(new TileTrigo(0, 0, valores.Dequeue()));
                    break;
                case "PIEDRA":
                    tiles.Add(new TilePiedra(0, 0, valores.Dequeue()));
                    break;
            }

            // Poblar puertos
            List<Puerto> puertos = new List<Puerto>();
            LadoCoords[] costa = new LadoCoords[30]
            {
                new LadoCoords(1, 2, Lado.Oeste),
                new LadoCoords(1, 2, Lado.Norte),
                new LadoCoords(2, 1, Lado.Oeste),
                new LadoCoords(2, 1, Lado.Norte),
                new LadoCoords(3, 0, Lado.Oeste),
                new LadoCoords(3, -1, Lado.Sur),
                new LadoCoords(3, -1, Lado.Oeste),
                new LadoCoords(3, -2, Lado.Sur),
                new LadoCoords(3, -2, Lado.Oeste),
                new LadoCoords(3, -3, Lado.Sur),
                new LadoCoords(2, -2, Lado.Norte),
                new LadoCoords(2, -3, Lado.Sur),
                new LadoCoords(1, -2, Lado.Norte),
                new LadoCoords(1, -3, Lado.Sur),
                new LadoCoords(0, -2, Lado.Norte),
                new LadoCoords(0, -2, Lado.Oeste),
                new LadoCoords(-1, -1, Lado.Norte),
                new LadoCoords(-1, -1, Lado.Oeste),
                new LadoCoords(-2, 0, Lado.Norte),
                new LadoCoords(-2, 0, Lado.Oeste),
                new LadoCoords(-2, 0, Lado.Sur),
                new LadoCoords(-2, 1, Lado.Oeste),
                new LadoCoords(-2, 1, Lado.Sur),
                new LadoCoords(-2, 2, Lado.Oeste),
                new LadoCoords(-2, 2, Lado.Sur),
                new LadoCoords(-2, 3, Lado.Norte),
                new LadoCoords(-1, 2, Lado.Sur),
                new LadoCoords(-1, 3, Lado.Norte),
                new LadoCoords(0, 2, Lado.Sur),
                new LadoCoords(0, 3, Lado.Norte)
            };
            for (i = 0; i < 9; i++)
            {
                int pos = Convert.ToInt32(datosPuertos[2 * i + 1]);
                switch (datosPuertos[2*i])
                {
                    case "GENERAL":
                        puertos.Add(new PuertoGeneral(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                    case "MADERA":
                        puertos.Add(new PuertoMadera(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                    case "LADRILLO":
                        puertos.Add(new PuertoLadrillo(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                    case "OVEJA":
                        puertos.Add(new PuertoOveja(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                    case "TRIGO":
                        puertos.Add(new PuertoTrigo(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                    case "PIEDRA":
                        puertos.Add(new PuertoPiedra(costa[pos].Q, costa[pos].R, costa[pos].L));
                        break;
                }
            }

            this.tiles = tiles.ToArray();
            this.puertos = puertos.ToArray();
            this.carreteras = new List<Carretera>();
            this.fichasVertices = new List<FichaVertice>();
        }

        private void TabTablero_Load(object sender, EventArgs e)
        {
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
            ListaJugadores();
            dataGridJugadores.ClearSelection();

            dataGridJugadores.Rows[0].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[1].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[2].Height = dataGridJugadores.Height / 5;
            dataGridJugadores.Rows[3].Height = dataGridJugadores.Height / 5;

            // Calcular las posiciones posibles de carreteras y poblados
            List<VerticeCoords> vertices = new List<VerticeCoords>();
            List<LadoCoords> lados = new List<LadoCoords>();
            foreach (Tile tile in this.tiles)
            {
                if (tile is TileMar)
                    continue;
                lados.AddRange(tile.Coords.Lados);
                vertices.AddRange(tile.Coords.Vertices);
            }
            ladosTablero = lados.Distinct().ToArray();
            verticesTablero = vertices.Distinct().ToArray();
            RecalcularLadosPosibles();
            RecalcularVerticesPosibles();

            // Inicializar camara
            this.zoomLevel = 5;
            this.basePoint = new Point(this.Width / 2, this.Height / 2);

            // Estado partida
            this.estado = Estado.Normal;

            this.Paint += TabTablero_Paint;
            this.MouseWheel += TabTablero_MouseWheel;
            this.MouseDown += TabTablero_MouseDown;
            this.MouseMove += TabTablero_MouseMove;
            this.MouseClick += TabTablero_Click;
            this.KeyDown += TabTablero_KeyDown;

            // Para que sea más fluido el repintado
            this.DoubleBuffered = true;
        }

        private void TabTablero_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Gainsboro);

            Bitmap bmp;
            Size size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles)
            {
                switch (tile)
                {
                    case TileMar mar:
                        bmp = TileMar.Bitmap;
                        break;
                    default:
                        continue;
                }
                Rectangle rect = new Rectangle(tile.HexToPixel(this.basePoint, this.zoomLevel), size);
                e.Graphics.DrawImage(bmp, rect);
                if (tile.Valor != null)
                {
                    e.Graphics.DrawImage(numbers[(int)tile.Valor - 1], rect);
                }
            }
            // Dibujar Puertos
            size = new Size((Tile.BWIDTH + 2 * Puerto.DX) / zoomLevel, (Tile.BHEIGHT + 2 * Puerto.DY) / zoomLevel);
            foreach (Puerto puerto in puertos)
            {
                e.Graphics.DrawImage(puerto.Bitmap, new Rectangle(puerto.LadoToPixel(basePoint, zoomLevel), size));
            }
            // Dibujar casillas de tierra
            size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles)
            {
                switch (tile)
                {
                    case TileDesierto desierto:
                        bmp = TileDesierto.Bitmap;
                        break;
                    case TileMar mar:
                        continue;
                    case TileMadera madera:
                        bmp = TileMadera.Bitmap;
                        break;
                    case TileLadrillo ladrillo:
                        bmp = TileLadrillo.Bitmap;
                        break;
                    case TileOveja oveja:
                        bmp = TileOveja.Bitmap;
                        break;
                    case TileTrigo trigo:
                        bmp = TileTrigo.Bitmap;
                        break;
                    case TilePiedra piedra:
                        bmp = TilePiedra.Bitmap;
                        break;
                    default:
                        bmp = TileDesierto.Bitmap;
                        break;
                }
                Rectangle rect = new Rectangle(tile.HexToPixel(this.basePoint, this.zoomLevel), size);
                e.Graphics.DrawImage(bmp, rect);
                if (tile.Valor != null)
                {
                    e.Graphics.DrawImage(numbers[(int)tile.Valor - 1], rect);
                }
            }
            // Dibujar Carreteras
            size = new Size((Tile.BWIDTH + 2 * Carretera.DX) / zoomLevel, (Tile.BHEIGHT + 2 * Carretera.DY) / zoomLevel);
            foreach (Carretera carretera in carreteras)
            {
                e.Graphics.DrawImage(carretera.Bitmap, new Rectangle(carretera.LadoToPixel(basePoint, zoomLevel), size));
            }
            if (estado == Estado.ColocarCarretera)
            {
                foreach (LadoCoords lado in ladosPosibles)
                {
                    switch (lado.L)
                    {
                        case Lado.Norte:
                            e.Graphics.DrawImage(cliente.Properties.Resources.CarreteraNorteContorno,
                                new Rectangle(lado.LadoToPixel(basePoint, zoomLevel, Carretera.DX, Carretera.DY), size));
                            break;
                        case Lado.Oeste:
                            e.Graphics.DrawImage(cliente.Properties.Resources.CarreteraOesteContorno,
                                new Rectangle(lado.LadoToPixel(basePoint, zoomLevel, Carretera.DX, Carretera.DY), size));
                            break;
                        case Lado.Sur:
                            e.Graphics.DrawImage(cliente.Properties.Resources.CarreteraSurContorno,
                                new Rectangle(lado.LadoToPixel(basePoint, zoomLevel, Carretera.DX, Carretera.DY), size));
                            break;
                    }
                }
                e.Graphics.DrawImage(carreteraColocar.Bitmap, new Rectangle(carreteraColocar.LadoToPixel(basePoint, zoomLevel), size));
            }
            // Dibujar poblados
            size = new Size(FichaVertice.BHALFSIDE * 2 / this.zoomLevel, FichaVertice.BHALFSIDE * 2 / this.zoomLevel);
            foreach (FichaVertice ficha in fichasVertices)
            {
                e.Graphics.DrawImage(ficha.Bitmap, new Rectangle(ficha.VerticeToPixel(basePoint, zoomLevel), size));
            }
            if (estado == Estado.ColocarPoblado)
            {
                foreach(VerticeCoords vertice in verticesPosibles)
                {
                    e.Graphics.DrawImage(cliente.Properties.Resources.VerticeContorno, new Rectangle(vertice.VerticeToPixel(basePoint, zoomLevel), size));
                }
                e.Graphics.DrawImage(verticeColocar.Bitmap, new Rectangle(verticeColocar.VerticeToPixel(basePoint, zoomLevel), size));
            }
        }

        private void TabTablero_MouseWheel(object sender, MouseEventArgs e)
        {
            int oldZoom = zoomLevel;
            if (e.Delta > 0)
            {
                this.zoomLevel = Math.Max(zoomLevel - 1, 1);

            } else if (e.Delta < 0)
            {
                this.zoomLevel = Math.Min(zoomLevel + 1, 6);
            }
            int x = e.Location.X;
            int y = e.Location.Y;

            int oldImgX = (int)(x / oldZoom);
            int oldImgY = (int)(y / oldZoom);

            // Where in the IMAGE will it be when the new zoom i made
            int newImgX = (int)(x / zoomLevel);
            int newImgY = (int)(y / zoomLevel);

            // Where to move image to keep focus on one point
            basePoint.X += newImgX - oldImgX;
            basePoint.Y += newImgY - oldImgY;

            this.Refresh();
        }
        private void TabTablero_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (estado)
                {
                    case Estado.Normal:
                        oldMouse = e.Location;
                        break;
                    case Estado.ClickCasilla:
                        MessageBox.Show(HexCoords.PixelToHex(e.Location, basePoint, zoomLevel).ToString());
                        break;
                    case Estado.ColocarCarretera:
                        if (ComprobarCarretera(carreteraColocar.Coords))
                        {
                            carreteras.Add(carreteraColocar);
                            RecalcularLadosPosibles();
                            RecalcularVerticesPosibles();
                            estado = Estado.Normal;
                        }
                        break;
                    case Estado.ColocarPoblado:
                        if (ComprobarFichaVertice(verticeColocar.Coords))
                        {
                            fichasVertices.Add(verticeColocar);
                            RecalcularLadosPosibles();
                            RecalcularVerticesPosibles();
                            estado = Estado.Normal;
                        }
                        break;
                }
               
            }
        }

        private void TabTablero_MouseMove(object sender, MouseEventArgs e)
        {
            switch (estado)
            {
                case Estado.Normal:
                    // Panning
                    if (e.Button == MouseButtons.Left)
                    {
                        basePoint.X += e.Location.X - oldMouse.X;
                        basePoint.Y += e.Location.Y - oldMouse.Y;

                        oldMouse = e.Location;
                    }
                    break;
                case Estado.ColocarCarretera:
                    carreteraColocar.Coords = LadoCoords.PixelToLado(e.Location, basePoint, zoomLevel);
                    break;
                case Estado.ColocarPoblado:
                    verticeColocar.Coords = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                    break;
                default:
                    return;
            }
            this.Refresh();
        }

        private void TabTablero_Click(object sender, MouseEventArgs e)
        {
        }

        private void TabTablero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                this.Refresh();
            }
        }

        private void btnCarretera_Click(object sender, EventArgs e)
        {
            estado = Estado.ColocarCarretera;
            carreteraColocar = new Carretera(0, 0, Lado.Oeste, this.colorJugador);
        }

        private void btnPoblado_Click(object sender, EventArgs e)
        {
            verticeColocar = new FichaPoblado(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarPoblado;
        }

        private void btnCiudad_Click(object sender, EventArgs e)
        {
            verticeColocar = new FichaCiudad(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarPoblado;
        }

        private void btnCasilla_Click(object sender, EventArgs e)
        {
            if (estado == Estado.ClickCasilla)
            {
                estado = Estado.Normal;
            } else if (estado == Estado.Normal)
            {
                estado = Estado.ClickCasilla;
            }
        }
        public bool ComprobarCarretera(LadoCoords coords)
        {
            foreach (LadoCoords posible in ladosPosibles)
            {
                if (posible == coords)
                    return true;
            }
            return false;
        }
        public bool ComprobarFichaVertice(VerticeCoords coords)
        {
            foreach (VerticeCoords posible in verticesPosibles)
            {
                if (posible == coords)
                    return true;
            }
            return false;
        }

        public void RecalcularLadosPosibles()
        {
            List<LadoCoords> lados = new List<LadoCoords>();
            foreach (Carretera carretera in carreteras)
            {
                if (carretera.Color != this.colorJugador)
                    continue;
                foreach (LadoCoords lado in carretera.Coords.LadosVecinos())
                {
                    // Comprobar que sea una posición "construible"
                    foreach (LadoCoords posible in this.ladosTablero)
                    {
                        if (lado == posible)
                        {
                            lados.Add(lado);
                            break;
                        }
                    }
                }
            }
            foreach (FichaVertice ficha in fichasVertices)
            {
                if (ficha.Color != this.colorJugador)
                    continue;
                foreach (LadoCoords lado in ficha.Coords.LadosVecinos())
                {
                    // Comprobar que sea una posición "construible"
                    foreach (LadoCoords posible in this.ladosTablero)
                    {
                        if (lado == posible)
                        {
                            lados.Add(lado);
                            break;
                        }
                    }
                }
            }
            lados = lados.Distinct().ToList();
            foreach (Carretera carretera in carreteras)
            {
                lados.Remove(carretera.Coords);
            }
            this.ladosPosibles = lados.ToArray();
        }

        public void RecalcularVerticesPosibles()
        {
            List<VerticeCoords> vertices = new List<VerticeCoords>();
            int count = 0;
            foreach (FichaVertice vertice in fichasVertices)
            {
                if (vertice.Color == this.colorJugador)
                    count++;
            }
            if (count >= 2)
            {
                foreach (Carretera carretera in this.carreteras)
                {
                    if (carretera.Color != this.colorJugador)
                        continue;
                    foreach (VerticeCoords vertice in carretera.Coords.VerticesExtremos())
                    {
                        foreach (VerticeCoords posible in this.verticesTablero)
                        {
                            if (vertice == posible)
                            {
                                vertices.Add(vertice);
                                break;
                            }
                        }
                    }
                }
                vertices = vertices.Distinct().ToList();
            } else
            {
                vertices.AddRange(verticesTablero);
            }
            foreach (FichaVertice ficha in this.fichasVertices)
            {
                vertices.Remove(ficha.Coords);
                foreach (VerticeCoords vecino in ficha.Coords.VerticesVecinos())
                {
                    vertices.Remove(vecino);
                }
            }
            verticesPosibles = vertices.ToArray();
        }
        public void ListaJugadores()
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                if (nombres[i] != "")
                {
                    dataGridJugadores.Rows[i].Cells[0].Value = nombres[i];
                    dataGridJugadores.Rows[i].Cells[1].Style.BackColor = DameColor(colores[i]);
                }
                else
                    dataGridJugadores.Rows[i].Cells[1].Style.BackColor = Color.White;

                dataGridJugadores.CellPainting += new DataGridViewCellPaintingEventHandler(this.dataGrid_CellPainting);
            }
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
            for (int i = 0; i < nombres.Length; i++)
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
        private void dataGridJugadores_SelectionChanged(object sender, EventArgs e)
        {
            dataGridJugadores.ClearSelection();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            Bitmap bmp;
            Size size = new Size(116, 25);
            bmp = Properties.Resources.Monopolio;
            bmp = new Bitmap(bmp, size);
            Point punto = new Point(0, 0);

            int i = 0;
            while (i < numMonopolios)
            {
                if (i > 0)
                    punto = new Point(punto.X + 10, punto.Y);
                e.Graphics.DrawImage(bmp, punto);
                i++;
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            Bitmap bmp;
            Size size = new Size(116, 25);
            bmp = Properties.Resources.Invento;
            bmp = new Bitmap(bmp, size);
            Point punto = new Point(0, 0);

            int i = 0;
            while (i < numInventos)
            {
                if (i > 0)
                    punto = new Point(punto.X + 10, punto.Y);
                e.Graphics.DrawImage(bmp, punto);
                i++;
            }
        }
    }
}

