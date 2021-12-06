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
        string turno;
        public override ColorJugador colorJugador { get; set; }

        public string[] nombres { get; set; }
        public ColorJugador[] colores { get; set; }

        Tile[] tiles;
        List<FichaVertice> fichasVertices;
        Puerto[] puertos;
        List<Carretera> carreteras;
        LadoCoords[] ladosTablero;
        VerticeCoords[] verticesTablero;
        LadoCoords[] ladosPosibles;
        VerticeCoords[] verticesPosibles;

        int zoomLevel;
        Point basePoint;
        Point oldMouse;
        
        enum Estado
        {
            Normal,
            ClickCasilla,
            ColocarPoblado,
            ColocarCarretera,
            Comerciar
        };

        Estado estado;
        FichaVertice verticeColocar;
        Carretera carreteraColocar;

        static Bitmap[] numbers = new Bitmap[]
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

        PanelInfoJugador[] paneles;

        int madera, ladrillo, oveja, trigo, piedra;

        public TabTablero(Socket conn, int idP, string nombre)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
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
            // Calcular las posiciones posibles de carreteras y poblados
            List<VerticeCoords> vertices = new List<VerticeCoords>();
            List<LadoCoords> lados = new List<LadoCoords>();
            foreach (Tile tile in this.tiles[18..])
            {
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

            pnlTablero.Paint += TabTablero_Paint;
            pnlTablero.MouseWheel += TabTablero_MouseWheel;
            pnlTablero.MouseDown += TabTablero_MouseDown;
            pnlTablero.MouseMove += TabTablero_MouseMove;

            btnCarretera.Enabled = false;
            btnPoblado.Enabled = false;
            btnCiudad.Enabled = false;
            btnDesarrollo.Enabled = false;
            btnComercio.Enabled = false;
            btnTurno.Enabled = false;

            btnCarretera.MouseHover += BtnConstruir_MouseHover;
            btnPoblado.MouseHover += BtnConstruir_MouseHover;
            btnCiudad.MouseHover += BtnConstruir_MouseHover;
            btnDesarrollo.MouseHover += BtnConstruir_MouseHover;

            // Paneles
            this.paneles = new PanelInfoJugador[] { pnlJugador1, pnlJugador2, pnlJugador3, pnlJugador4 };
            
            for (int i = 0; i < 4; i++)
            {
                if (i < nombres.Length)
                {
                    paneles[i].Nombre = nombres[i];
                    paneles[i].Color = colores[i];
                    paneles[i].Caballeros = 0;
                    paneles[i].Carreteras = 0;
                    paneles[i].Recursos = 0;
                    paneles[i].Desarrollo = 0;
                    paneles[i].Puntos = 0;
                    paneles[i].Show();
                }
                else
                {
                    paneles[i].Hide();
                }
            }

        
                }

        private void TabTablero_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Gainsboro);

            Bitmap bmp;
            Size size;
            
            /* Dibujar mar */
            size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles[0..18])
            {
                Rectangle rect = new Rectangle(tile.HexToPixel(this.basePoint, this.zoomLevel), size);
                e.Graphics.DrawImage(tile.Bitmap, rect);
            }

            /* Dibujar puertos */
            size = new Size((Tile.BWIDTH + 2 * Puerto.DX) / zoomLevel, (Tile.BHEIGHT + 2 * Puerto.DY) / zoomLevel);
            foreach (Puerto puerto in puertos)
            {
                e.Graphics.DrawImage(puerto.Bitmap, new Rectangle(puerto.LadoToPixel(basePoint, zoomLevel), size));
            }

            /* Dibujar tierra */
            size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles[18..])
            {
                Rectangle rect = new Rectangle(tile.HexToPixel(this.basePoint, this.zoomLevel), size);
                e.Graphics.DrawImage(tile.Bitmap, rect);
                if (tile.Valor != null)
                {
                    e.Graphics.DrawImage(numbers[(int)tile.Valor - 1], rect);
                }
            }

            /* Dibujar carreteras */
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

            /* Dibujar poblados */
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

            pnlTablero.Refresh();
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
                        pnlTablero.Refresh();
                    }
                    break;
                case Estado.ColocarCarretera:
                    carreteraColocar.Coords = LadoCoords.PixelToLado(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                case Estado.ColocarPoblado:
                    verticeColocar.Coords = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                default:
                    return;
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

        public void CambiarTurno(string nombre)
        {
            lblTurno.Text = "Turno: " + nombre;
            this.turno = nombre;
            if (turno == this.nombre)
            {
                btnCarretera.Enabled = true;
                btnPoblado.Enabled = true;
                btnCiudad.Enabled = true;
                btnDesarrollo.Enabled = true;
                btnComercio.Enabled = true;
                btnTurno.Text = "Tirar dados";
                btnTurno.Tag = "DADOS";
                btnTurno.Enabled = true;
            }
        }
        public void TirarDados(string dados)
        {
            string[] valores = dados.Split(",");
            int dado1 = Convert.ToInt32(valores[0]);
            int dado2 = Convert.ToInt32(valores[1]);
            if (this.turno == this.nombre)
            {
                btnTurno.Text = "Acabar turno";
                btnTurno.Tag = "ACABAR";
                btnTurno.Enabled = true;
                btnCarretera.Enabled = true;
                btnPoblado.Enabled = true;
                btnCiudad.Enabled = true;
                btnDesarrollo.Enabled = true;
                btnComercio.Enabled = true;

            }
            //Animación dados y repartir recursos
            MessageBox.Show(turno + ": " + dados);
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

        private void btnTurno_Click(object sender, EventArgs e)
        {
            string pet;
            byte[] pet_b;
            switch (btnTurno.Tag)
            {
                case "DADOS":
                    pet = "16/" + idP.ToString();
                    pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                    conn.Send(pet_b);
                    btnTurno.Enabled = false;
                    btnTurno.Tag = "";
                    btnTurno.Text = "";
                    break;
                case "ACABAR":
                    pet = "15/" + idP.ToString();
                    pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                    conn.Send(pet_b);
                    btnTurno.Enabled = false;
                    btnTurno.Tag = "";
                    btnTurno.Text = "";
                    btnCarretera.Enabled = false;
                    btnPoblado.Enabled = false;
                    btnCiudad.Enabled = false;
                    btnDesarrollo.Enabled = false;
                    btnComercio.Enabled = false;
                    break;
            }
        }

        private void BtnConstruir_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tooltipCostes.SetToolTip(btn, btn.Tag.ToString());
        }
    }
}

