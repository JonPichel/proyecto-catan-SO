using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Net.Sockets;
using System.Resources;

namespace cliente.Partida
{
    public partial class TabTablero : TabPartida
    {
        Socket conn;
        int idP;
        string nombre;
        string turno;
        int numturnos;
        int numJugadores;
        int sumadados;

        public override ColorJugador colorJugador { get; set; }

        public string[] nombres { get; set; }
        public ColorJugador[] colores { get; set; }

        Tile[] tiles;
        TileLadron ladron;
        List<FichaVertice> fichasVertices;
        List<VerticeCoords> misPoblados;
        Puerto[] puertos;
        List<Carretera> carreteras;
        LadoCoords[] ladosTablero;
        VerticeCoords[] verticesTablero;
        LadoCoords[] ladosPosibles;
        VerticeCoords[] verticesPosibles;
        HexCoords posicionLadron;


        int zoomLevel;
        Point basePoint;
        Point oldMouse;
        
        enum Estado
        {
            Normal,
            ColocarLadron,
            ColocarPoblado,
            ColocarCiudad,
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
        PanelInfoJugador panelActualizar;

        // Recursos en tu poder
        int madera, ladrillo, oveja, trigo, piedra;
        List<Carta> cartas;
        
        public TabTablero(Socket conn, int idP, string nombre)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
        }

        public void CargarTablero(string[] trozos)
        {
            // Vaciar recursos
            this.madera = 0;
            this.ladrillo = 0;
            this.oveja = 0;
            this.trigo = 0;
            this.piedra = 0;
            this.cartas = new List<Carta>();
            
            // Construir tablero
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
                                posicionLadron = new HexCoords(coords.Q, coords.R);
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
            this.misPoblados = new List<VerticeCoords>();
        }

        public int Madera
        {
            get => this.madera;
            set
            {
                this.madera = value;
                this.lblMadera.Text = value.ToString();
            }
        }
        public int Ladrillo
        {
            get => this.ladrillo;
            set
            {
                this.ladrillo = value;
                this.lblLadrillo.Text = value.ToString();
            }
        }
        public int Oveja
        {
            get => this.oveja;
            set
            {
                this.oveja = value;
                this.lblOveja.Text = value.ToString();
            }
        }
        public int Trigo
        {
            get => this.trigo;
            set
            {
                this.trigo = value;
                this.lblTrigo.Text = value.ToString();
            }
        }
        public int Piedra
        {
            get => this.piedra;
            set
            {
                this.piedra = value;
                this.lblPiedra.Text = value.ToString();
            }
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
            this.lblUndo.Parent = pnlTablero;
            this.lblUndo.Visible = false;
            this.lblUndo.Location = new Point(522, 355);
            this.lblDado1.Parent = pnlTablero;
            this.lblDado1.Location = new Point(10, 355);
            this.lblDado2.Parent = pnlTablero;
            this.lblDado2.Location = new Point(60, 355);
            this.numturnos = 0;

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
            this.numJugadores = nombres.Length;
            for (int i = 0; i < 4; i++)
            {
                if (i < numJugadores)
                {
                    paneles[i].Nombre = nombres[i];
                    paneles[i].ColorJ = colores[i];
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
            this.panelActualizar = new PanelInfoJugador();
        }

        private void TabTablero_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.PowderBlue);

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
            if(estado == Estado.ColocarCiudad)
            {
                foreach (VerticeCoords vertice in misPoblados)
                {
                    e.Graphics.DrawImage(cliente.Properties.Resources.VerticeContorno, new Rectangle(vertice.VerticeToPixel(basePoint, zoomLevel), size));
                }
                e.Graphics.DrawImage(verticeColocar.Bitmap, new Rectangle(verticeColocar.VerticeToPixel(basePoint, zoomLevel), size));
            }
            
            /* Dibujar ladron */
            size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            ladron = new TileLadron(posicionLadron.Q, posicionLadron.R);
            Rectangle ladr = new Rectangle(ladron.HexToPixel(this.basePoint, this.zoomLevel), size);
            e.Graphics.DrawImage(ladron.Bitmap, ladr);

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
                        pnlTablero.Refresh();
                        break;
                    case Estado.ColocarLadron:
                        posicionLadron = HexCoords.PixelToHex(e.Location, basePoint, zoomLevel);
                        string pet = "17/" + idP.ToString() + "/" + posicionLadron.R.ToString() + "," +
                                posicionLadron.Q.ToString();
                        byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                        conn.Send(pet_b);
                        estado = Estado.Normal;
                        break;
                    case Estado.ColocarCarretera:
                        if (ComprobarCarretera(carreteraColocar.Coords))
                        {
                            carreteras.Add(carreteraColocar);
                            pet = "20/" + idP.ToString() + "/" + carreteraColocar.Coords.R.ToString() + "," +
                                carreteraColocar.Coords.Q.ToString() + "," + (int)carreteraColocar.Coords.L;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            RecalcularLadosPosibles();
                            RecalcularVerticesPosibles();
                            estado = Estado.Normal;
                            this.lblUndo.Visible = false;
                            panelActualizar.Carreteras = panelActualizar.Carreteras + 1;
                            if (numturnos <= (numJugadores*2))
                            {
                                btnCarretera.Enabled = false;

                                //Peticion acabar turno automatica
                                pet = "15/" + idP.ToString();
                                pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                                conn.Send(pet_b);
                            }
                            else
                            {
                                Madera--;
                                Ladrillo--;
                                panelActualizar.Madera--;
                                panelActualizar.Ladrillo--;
                                panelActualizar.Recursos -= 2;
                                RefreshBotones();
                            }
                            timerRecursos.Start();
                        }
                        break;
                    case Estado.ColocarPoblado:
                        if (ComprobarFichaVertice(verticeColocar.Coords, verticesPosibles))
                        {
                            fichasVertices.Add(verticeColocar);
                            misPoblados.Add(verticeColocar.Coords);
                            pet = "18/" + idP.ToString() + "/" + verticeColocar.Coords.R.ToString() + "," +
                                verticeColocar.Coords.Q.ToString() + "," + (int)verticeColocar.Coords.V;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            RecalcularLadosPosibles();
                            RecalcularVerticesPosibles();
                            estado = Estado.Normal;
                            this.lblUndo.Visible = false;
                            panelActualizar.Puntos = panelActualizar.Puntos + 1;
                            if (numturnos <= (numJugadores*2))
                            {
                                btnPoblado.Enabled = false;
                                btnCarretera.Enabled = true;
                            }
                            else
                            {
                                Madera--;
                                Ladrillo--;
                                Oveja--;
                                Trigo--;
                                panelActualizar.Madera--;
                                panelActualizar.Ladrillo--;
                                panelActualizar.Oveja--;
                                panelActualizar.Trigo--;
                                panelActualizar.Recursos -= 4;
                                RefreshBotones();
                            }
                            timerRecursos.Start();
                        }
                        break;
                    case Estado.ColocarCiudad:
                        if (ComprobarFichaVertice(verticeColocar.Coords, misPoblados.ToArray()))
                        {
                            foreach (FichaVertice ficha in fichasVertices)
                            {
                                if (ficha.Coords == verticeColocar.Coords)
                                {
                                    fichasVertices.Remove(ficha);
                                    break;
                                }
                            }
                            fichasVertices.Add(verticeColocar);
                            misPoblados.Remove(verticeColocar.Coords);
                            pet = "19/" + idP.ToString() + "/" + verticeColocar.Coords.R.ToString() + "," +
                                verticeColocar.Coords.Q.ToString() + "," + (int)verticeColocar.Coords.V;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            estado = Estado.Normal;
                            this.lblUndo.Visible = false;
                            panelActualizar.Puntos = panelActualizar.Puntos + 1;
                            trigo -= 2;
                            piedra -= 3;
                            panelActualizar.Trigo -= 2;
                            panelActualizar.Piedra -= 3;
                            panelActualizar.Recursos -= 5;
                            timerRecursos.Start();
                            RefreshBotones();
                        }
                        break;
                }
                pnlTablero.Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                this.lblUndo.Visible = false;
                estado = Estado.Normal;
                pnlTablero.Refresh();
            }
        }

        private void TabTablero_MouseMove(object sender, MouseEventArgs e)
        {
            switch (estado)
            {
                case Estado.Normal:
                    // Panning
                    if (e.Button == MouseButtons.Left && timerRecursos.Enabled == false)
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
                case Estado.ColocarCiudad:
                    verticeColocar.Coords = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                default:
                    return;
            }
        }

        private void btnCarretera_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            estado = Estado.ColocarCarretera;
            carreteraColocar = new Carretera(0, 0, Lado.Oeste, this.colorJugador);
        }

        private void btnPoblado_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            verticeColocar = new FichaPoblado(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarPoblado;
        }

        private void btnCiudad_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            verticeColocar = new FichaCiudad(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarCiudad;
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
        public bool ComprobarFichaVertice(VerticeCoords coords, VerticeCoords[] ListaVertices)
        {
            foreach (VerticeCoords posible in ListaVertices)
            {
                if (posible == coords)
                    return true;
            }
            return false;
        }

        public void RecalcularLadosPosibles()
        {
            List<LadoCoords> lados = new List<LadoCoords>();
            if (numturnos > numJugadores * 2)
            {
                foreach (Carretera carretera in carreteras)
                {
                    if (carretera.Color != this.colorJugador)
                        continue;
                    VerticeCoords[] extremos = carretera.Coords.VerticesExtremos();

                    foreach (LadoCoords lado in carretera.Coords.LadosVecinos())
                    {
                        bool posible = true;
                        VerticeCoords[] extremos2 = lado.VerticesExtremos();
                        foreach (VerticeCoords extremo in extremos)
                        {
                            foreach (VerticeCoords extremo2 in extremos2)
                            {
                                if (extremo == extremo2)
                                {
                                    foreach (FichaVertice ficha in fichasVertices)
                                    {
                                        if (ficha.Coords == extremo && ficha.Color != this.colorJugador)
                                            posible = false;
                                    }
                                }
                            }
                        }
                        // Comprobar que sea una posición "construible"
                        foreach (LadoCoords opcion in this.ladosTablero)
                        {
                            if (lado == opcion && posible == true)
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
            }
            else
            {
                if (numturnos > 0)
                {
                    foreach (LadoCoords lado in fichasVertices.Last().Coords.LadosVecinos())
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
            this.numturnos = numturnos + 1;
            if(fichasVertices.Count != 0)
            {
                RecalcularLadosPosibles();
            }
            if (turno == this.nombre)
            {
                if (numturnos <= (numJugadores * 2))
                    btnPoblado.Enabled = true;
                else
                {
                    btnCiudad.Enabled = false;
                    btnCarretera.Enabled = false;
                    btnPoblado.Enabled = false;
                    btnComercio.Enabled = false;
                    btnDesarrollo.Enabled = false;
                    btnTurno.Enabled = true;
                    btnTurno.Text = "Tirar dados";
                    btnTurno.Tag = "DADOS";
                }
            }
            foreach (PanelInfoJugador panel in paneles)
            {
                if (panel.Nombre == this.turno)
                {
                    panelActualizar = panel;
                    //panel.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                    panel.BorderStyle = BorderStyle.None;
            }
        }

        public void RefreshBotones()
        {
            // Comprobar construir carretera
            if (madera < 1 || ladrillo < 1)
                btnCarretera.Enabled = false;
            else
                btnCarretera.Enabled = true;

            // Comprobar construir poblado
            if (madera < 1 || ladrillo < 1 || trigo < 1 || oveja < 1)
                btnPoblado.Enabled = false;
            else
                btnPoblado.Enabled = true;

            // Comprobar contruir ciudad
            if (trigo < 2 || piedra < 3)
                btnCiudad.Enabled = false;
            else
                btnCiudad.Enabled = true;

            // Comprobar comprar carta
            if (trigo < 1 || oveja < 1 || piedra < 1)
                btnDesarrollo.Enabled = false;
            else
                btnDesarrollo.Enabled = true;

            btnComercio.Enabled = true;            
            btnTurno.Enabled = true;
        }

        public void TirarDados(string dados)
        {
            string[] valores = dados.Split(",");
            int dado1 = Convert.ToInt32(valores[0]);
            int dado2 = Convert.ToInt32(valores[1]);
            
            //Animación dados y repartir recursos
            this.sumadados = dado1 + dado2;

            string dado1location = "Dado" + dado1.ToString();
            string dado2location = "Dado" + dado2.ToString();

            lblDado1.Image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(dado1location), new Size(40, 40));
            lblDado2.Image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(dado2location), new Size(40, 40));


            /* Sumar recursos */
            foreach (Tile casilla in tiles[18..])
            {
                if ((casilla.Valor ?? 0) == sumadados && posicionLadron != casilla.Coords)
                {
                    foreach (VerticeCoords vertice in casilla.Coords.Vertices)
                    {
                        foreach (FichaVertice ficha in fichasVertices)
                        {
                            if (ficha.Coords == vertice)
                            {
                                int suma = 1;
                                switch (ficha)
                                {
                                    case FichaPoblado _:
                                        suma = 1;
                                        break;
                                    case FichaCiudad _:
                                        suma = 2;
                                        break;
                                }
                                // Sumatelo a los recursos
                                if (ficha.Color == this.colorJugador)
                                {
                                    switch (casilla)
                                    {
                                        case TileMadera _:
                                            madera += suma;
                                            break;
                                        case TileLadrillo _:
                                            ladrillo += suma;
                                            break;
                                        case TileOveja _:
                                            oveja += suma;
                                            break;
                                        case TileTrigo _:
                                            trigo += suma;
                                            break;
                                        case TilePiedra _:
                                            piedra += suma;
                                            break;
                                    }
                                }
                                // Actualizar gráficamente
                                for (int i = 0; i < numJugadores; i++)
                                {
                                    if (colores[i] == ficha.Color)
                                    {
                                        paneles[i].Recursos += suma;
                                        switch (casilla)
                                        {
                                            case TileMadera _:
                                                paneles[i].Madera += suma;
                                                break;
                                            case TileLadrillo _:
                                                paneles[i].Ladrillo += suma;
                                                break;
                                            case TileOveja _:
                                                paneles[i].Oveja += suma;
                                                break;
                                            case TileTrigo _:
                                                paneles[i].Trigo += suma;
                                                break;
                                            case TilePiedra _:
                                                paneles[i].Piedra += suma;
                                                break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                // Refrescar panel
                Madera = madera;
                Ladrillo = ladrillo;
                Oveja = oveja;
                Trigo = trigo;
                Piedra = piedra;
                RefreshBotones();
                //Activar timer para mostrar cambios en los recursos
                timerRecursos.Start();
                btnTurno.Text = "Acabar turno";
                btnTurno.Tag = "ACABAR";

                if (sumadados == 7 && this.turno == this.nombre)
                {
                    //FALTA AMPLIAR
                    estado = Estado.ColocarLadron;
                }
            }
        }

        private void btnDesarrollo_Click(object sender, EventArgs e)
        {
            string pet = "21/" + idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
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

        private void btnComercio_Click(object sender, EventArgs e)
        {
            if (this.nombre != this.turno)
                return;

            FormComerciar form = new FormComerciar(this.conn, this.idP, this.nombre, this.nombres, this.colores, 2, 3, 0, 1, 4, 4, 4, 4, 4, 4); //Socket conn, int idP, string nombre, string[] nombres, ColorJugador[] colores, int Madera, int Ladrillo, int Oveja, int Trigo, int Piedra, int puertomadera, int puertoladrillo, int puertooveja, int puertotrigo, int puertopiedra
            form.ShowDialog();
        }

        private void lblUndo_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = false;
            estado = Estado.Normal;
            pnlTablero.Refresh();
        }

        private void BtnConstruir_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tooltipCostes.SetToolTip(btn, btn.Tag.ToString());
        }

        private void timerRecursos_Tick(object sender, EventArgs e)
        {
            foreach(PanelInfoJugador panel in paneles)
            {
                panel.Madera = 0;
                panel.Ladrillo = 0;
                panel.Oveja = 0;
                panel.Trigo = 0;
                panel.Piedra = 0;
            }
            timerRecursos.Stop();
        }

        public void ComprarCarta(string mensaje)
        {
            if (Convert.ToInt32(mensaje) == -1)
            {
                btnDesarrollo.Enabled = false;
                return;
            }
            panelActualizar.Desarrollo++;
            if ((Carta.TipoCarta)Convert.ToInt32(mensaje) == Carta.TipoCarta.Punto)
                panelActualizar.Puntos++;

            if (this.nombre != this.turno)
                return;

            Carta carta = new Carta((Carta.TipoCarta)Convert.ToInt32(mensaje));
            int num = 0;
            foreach(Carta otra in this.cartas)
            {
                if (otra.Tipo == carta.Tipo)
                {
                    if (carta.Tipo != Carta.TipoCarta.Punto)
                        otra.Enabled = false;
                    num++;
                }
            }
            cartas.Add(carta);
            int x = 20 * num + 5;
            int y = cliente.Properties.Resources.CartaMonopolio.Size.Height * (int)carta.Tipo + 5;

            carta.Click += Carta_Click;
            pnlCartas.Controls.Add(carta);
            carta.Location = new Point(x, y);
            carta.BringToFront();
        }

        private void Carta_Click(object sender, EventArgs e)
        {
            if (this.nombre != this.turno)
                return;

            Carta carta = (Carta)sender;
            
            if ((int)carta.Tipo == 4)
                return;
            cartas.Remove(carta);
            pnlCartas.Controls.Remove(carta);
            for (int i = cartas.Count - 1; i >= 0; i--)
            {
                if (cartas[i].Tipo == carta.Tipo)
                {
                    cartas[i].Enabled = true;
                    break;
                }
            }
            string pet;
            byte[] pet_b;
            switch ((int)carta.Tipo)
            {
                case 0:
                    FormMonopolio form = new FormMonopolio();
                    form.ShowDialog();
                    string recurso = form.Recurso;
                    pet = "25/" + idP.ToString() + "/" + recurso;
                    break;
                case 1:
                    pet = "24/" + idP.ToString();
                    break;
                case 2:
                    pet = "22/" + idP.ToString();
                    panelActualizar.Caballeros = panelActualizar.Caballeros + 1;
                    estado = Estado.ColocarLadron;
                    break;
                case 3:
                    pet = "23/" + idP.ToString();
                    break;
                default:
                    return;
            }
            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }
        public void UsarCarta(string mensaje)
        {
            panelActualizar.Desarrollo--;

            if (this.nombre == this.turno)
                return;
                
            string[] trozos = mensaje.Split('/');
            int codigo = Convert.ToInt32(trozos[0]);

            switch (codigo)
            {
                case 22:
                    MessageBox.Show(this.turno + " ha usado carta de desarrollo: Caballero");
                    panelActualizar.Caballeros++;
                    break;
                case 23:
                    MessageBox.Show(this.turno + " ha usado carta de desarrollo: Carreteras" );
                    break;
                case 24:
                    MessageBox.Show(this.turno + " ha usado carta de desarrollo: Invento" );
                    break;
                case 25:
                    string recurso = trozos[2];
                    MessageBox.Show(this.turno + " ha usado carta de desarrollo: Monopolio \n Quiere recurso: " + recurso);
                    break;
            }
        }
        public void Colocar(string mensaje)
        {
            if (this.nombre == this.turno)
                return;

            string[] trozos = mensaje.Split('/');
            int codigo = Convert.ToInt32(trozos[0]);
            string[] coordenadas = trozos[2].Split(',');

            ColorJugador Color = this.colorJugador;

            for (int i = 0; i < numJugadores; i++)
            {
                if (nombres[i] == turno)
                    Color = colores[i]; 
            }

            switch (codigo)
            {
                case 17:
                    posicionLadron = new HexCoords(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]));
                    pnlTablero.Refresh();
                    break;
                case 18:
                    verticeColocar = new FichaPoblado(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Vertice)Convert.ToInt32(coordenadas[2]), Color);
                    fichasVertices.Add(verticeColocar);
                    pnlTablero.Refresh();
                    panelActualizar.Puntos = panelActualizar.Puntos + 1;
                    if (numturnos > (numJugadores * 2))
                    {
                        panelActualizar.Madera--;
                        panelActualizar.Ladrillo--;
                        panelActualizar.Oveja--;
                        panelActualizar.Trigo--;
                        panelActualizar.Recursos -= 4;
                        timerRecursos.Start();
                    }
                    break;
                case 19:
                    verticeColocar = new FichaCiudad(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Vertice)Convert.ToInt32(coordenadas[2]), Color);
                    fichasVertices.Add(verticeColocar);
                    pnlTablero.Refresh();
                    panelActualizar.Puntos = panelActualizar.Puntos + 1;
                    if (numturnos > (numJugadores * 2))
                    {
                        panelActualizar.Trigo -= 2;
                        panelActualizar.Piedra -= 3;
                        panelActualizar.Recursos -= 5;
                        timerRecursos.Start();
                    }
                    break;
                case 20:
                    carreteraColocar = new Carretera(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Lado)Convert.ToInt32(coordenadas[2]), Color);
                    carreteras.Add(carreteraColocar);
                    pnlTablero.Refresh();
                    panelActualizar.Carreteras = panelActualizar.Carreteras + 1;
                    if (numturnos > (numJugadores * 2))
                    {
                        panelActualizar.Madera--;
                        panelActualizar.Ladrillo--;
                        panelActualizar.Recursos -= 2;
                        timerRecursos.Start();
                    }
                    break;
            }
            RecalcularLadosPosibles();
            RecalcularVerticesPosibles();
        }
    }
}

