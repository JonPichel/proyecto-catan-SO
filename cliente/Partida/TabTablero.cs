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
        public int puntos;
        ColorJugador colorturno;
        int numturnos;
        int numJugadores;
        int sumadados;
        bool desarrolloUsada;
        int DosCarreteras;

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
        VerticeCoords fichaRobar;


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
            Robar
        };

        Estado estado;
        FichaVertice verticeColocar;
        Carretera carreteraColocar;

        // Imágenes de los números
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
        FormComerciar formComerciar = null;

        // Recursos y cartas en tu poder
        int madera, ladrillo, oveja, trigo, piedra;
        List<Carta> cartas;
        
        public TabTablero(Socket conn, int idP, string nombre)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
        }

        /// <summary>
        /// Inicializa la partida junto con la estructura del tablero
        /// </summary>
        /// <param name="res"> String de la forma: tipoHex1,tipoHex2,...,tipoHex19/tipoPuerto1,
        /// indice1,tipoPuerto2,indice2,...,tipoPuerto9,indice9 </param>  
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

        // Constructor madera
        public int Madera
        {
            get => this.madera;
            set
            {
                this.madera = value;
                this.lblMadera.Text = value.ToString();
            }
        }
        // Constructor ladrillo
        public int Ladrillo
        {
            get => this.ladrillo;
            set
            {
                this.ladrillo = value;
                this.lblLadrillo.Text = value.ToString();
            }
        }
        // Constructor oveja
        public int Oveja
        {
            get => this.oveja;
            set
            {
                this.oveja = value;
                this.lblOveja.Text = value.ToString();
            }
        }
        // Constructor trigo
        public int Trigo
        {
            get => this.trigo;
            set
            {
                this.trigo = value;
                this.lblTrigo.Text = value.ToString();
            }
        }
        // Constructor piedra
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

            // Inicializar centrado de la cámara en el tablero
            this.zoomLevel = 5;
            this.basePoint = new Point(290, 210);

            // Estado partida
            this.estado = Estado.Normal;
            this.numturnos = 0;
            this.DosCarreteras = 0;

            pnlTablero.Paint += TabTablero_Paint;
            pnlTablero.MouseWheel += TabTablero_MouseWheel;
            pnlTablero.MouseDown += TabTablero_MouseDown;
            pnlTablero.MouseMove += TabTablero_MouseMove;

            Button[] btnPrincipal = new Button[] { btnCarretera , btnPoblado , btnCiudad , btnDesarrollo, btnComercio, btnTurno };
            foreach(Button btn in btnPrincipal)
            {
                btn.Enabled = false;
                btn.EnabledChanged += btnPrincipal_EnabledChanged;
            }

            lblUndo.Visible = false;
            desarrolloUsada = false;

            btnCarretera.MouseHover += BtnConstruir_MouseHover;
            btnPoblado.MouseHover += BtnConstruir_MouseHover;
            btnCiudad.MouseHover += BtnConstruir_MouseHover;
            btnDesarrollo.MouseHover += BtnConstruir_MouseHover;

            // Paneles de datos de los jugadores
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
                    paneles[i].Ejercito = 0;
                    paneles[i].Larga = 0;
                    paneles[i].Show();
                }
                else
                {
                    paneles[i].Hide();
                }
            }
            this.panelActualizar = new PanelInfoJugador();
        }

        /// <summary>
        /// Pinta el tablero y los objetos que se desean colocar sobre él
        /// </summary>
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

        /// <summary>
        /// Aumenta o disminuye el escalado de la visión del tablero
        /// </summary>
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

        /// <summary>
        /// Realiza diferentes eventos haciendo click sobre el tablero
        /// </summary>
        private void TabTablero_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Mira los diferentes estados del jugador
                switch (estado)
                {                    
                    case Estado.Normal:
                        // No se realiza ninguna acción
                        if (timerRaton.Enabled) return;
                        oldMouse = e.Location;
                        pnlTablero.Refresh();
                        break;
                    case Estado.ColocarLadron:
                        // Guarda las coordenadas del hexágono en las que se quiere colocar el larón
                        HexCoords nuevaposicionLadron = HexCoords.PixelToHex(e.Location, basePoint, zoomLevel);

                        // Miramos si es una posición diferente a la actual
                        if (nuevaposicionLadron == posicionLadron)
                        {
                            MessageBox.Show("Debes cambiar la posicion del ladron");
                            return;
                        }
                        else
                            this.posicionLadron = nuevaposicionLadron;

                        bool encontrado = false;
                        // Miramos que sea una ficha en tierra, es decir, no en el mar
                        foreach (Tile ficha in tiles[18..])
                        {
                            if (ficha.Coords == posicionLadron)
                            {
                                encontrado = true;
                                break;
                            }
                        }

                        if (encontrado == false)
                            return;

                        string pet = "17/" + idP.ToString() + "/" + posicionLadron.R.ToString() + "," +
                                posicionLadron.Q.ToString();
                        byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                        conn.Send(pet_b);

                        // Miramos si hay alguna posición disponible en los vértices del hexagono
                        int numPosibles = 0;
                        foreach (VerticeCoords vecinos in posicionLadron.Vertices)
                        {
                            foreach (FichaVertice fichas in fichasVertices)
                            {
                                if (vecinos == fichas.Coords)
                                {
                                    if (fichas.Color != this.colorturno)
                                    {
                                        numPosibles++;
                                        break;
                                    }
                                }
                            }
                        }

                        // Si no hay opción de robar, ningún vecino disponible
                        // se acaba el ladrón
                        if (numPosibles == 0)
                        {
                            timerRaton.Start();
                            estado = Estado.Normal;
                            btnTurno.Text = "Acabar turno";
                            btnTurno.Tag = "ACABAR";
                            lblInfo.Text = "";
                            RefreshBotones();
                            return;
                        }

                        // Si hay la opción de robar a alquien pasamos al estado robar
                        lblInfo.Text = "Elige a quien robar dando click a un poblado cercano al ladrón";
                        estado = Estado.Robar;
                        break;
                    case Estado.Robar:
                        // Guarda las coordenadas del hexágono de la ficha a la que se quiere robar
                        fichaRobar = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                        string donante = "";

                        // Mediante el color de la ficha obtenemos el nombre del jugador al que robar
                        foreach (VerticeCoords vecinos in posicionLadron.Vertices)
                        {
                            if (vecinos == fichaRobar)
                            {
                                foreach (FichaVertice fichas in fichasVertices)
                                {
                                    if (vecinos == fichas.Coords)
                                    {
                                        for (int l = 0; l < numJugadores; l++)
                                        {
                                            if (fichas.Color == colores[l] && fichas.Color != this.colorturno)
                                            {
                                                donante = nombres[l];
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }                    
                        }

                        if (donante == "")
                            return;                       

                        // Enviamos la petición de a quien hemos escogido para robar
                        pet = "32/" + idP.ToString() + "/" + donante;
                        pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                        conn.Send(pet_b);
                        timerRaton.Start();
                        estado = Estado.Normal;
                        lblInfo.Text = "";
                        btnTurno.Text = "Acabar turno";
                        btnTurno.Tag = "ACABAR";
                        RefreshBotones();
                        break;
                    case Estado.ColocarCarretera:
                        // Se elige la posición en la que colocar la carretera
                        // Primero comporbamos que la posición sea válida
                        if (ComprobarCarretera(carreteraColocar.Coords))
                        {
                            lblInfo.Text = "";
                            pet = "20/" + idP.ToString() + "/" + carreteraColocar.Coords.R.ToString() + "," +
                               carreteraColocar.Coords.Q.ToString() + "," + (int)carreteraColocar.Coords.L;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            this.lblUndo.Visible = false;
                            //Dos primeras rondas (sin coste)
                            if (numturnos <= (numJugadores * 2))
                            {                                
                                btnCarretera.Enabled = false;
                                btnTurno.Text = "Acabar turno";
                                btnTurno.Tag = "ACABAR";
                                btnTurno.Enabled = true;
                            }
                            // Carta carreteras (sin coste)
                            else if (DosCarreteras > 0)
                            {
                                DosCarreteras--;
                                if (DosCarreteras == 2)
                                    lblInfo.Text = "Queda por colocar 1 carretera";
                            }
                            else
                            {
                                Madera--;
                                Ladrillo--;
                                estado = Estado.Normal;
                                RefreshBotones();
                            }
                            if (DosCarreteras == 1 || DosCarreteras == 0)
                            {
                                lblInfo.Text = "";
                                timerRaton.Start();
                                estado = Estado.Normal;
                            }
                        }
                        break;
                    case Estado.ColocarPoblado:
                        // Se elige la posición en la que se quiere colocar un poblado
                        // Primero comprobamos si la posición es válida
                        if (ComprobarFichaVertice(verticeColocar.Coords, verticesPosibles))
                        {
                            misPoblados.Add(verticeColocar.Coords);
                            pet = "18/" + idP.ToString() + "/" + verticeColocar.Coords.R.ToString() + "," +
                                verticeColocar.Coords.Q.ToString() + "," + (int)verticeColocar.Coords.V;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            timerRaton.Start();
                            estado = Estado.Normal;
                            this.lblUndo.Visible = false;
                            //Dos primeras rondas (sin coste)
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
                                RefreshBotones();
                            }
                        }
                        break;
                    case Estado.ColocarCiudad:
                        // Se elige la posición en la que se quiere colocar una ciudad
                        // Primero comprobamos si la posición es válida
                        if (ComprobarFichaVertice(verticeColocar.Coords, misPoblados.ToArray()))
                        {
                            pet = "19/" + idP.ToString() + "/" + verticeColocar.Coords.R.ToString() + "," +
                                verticeColocar.Coords.Q.ToString() + "," + (int)verticeColocar.Coords.V;
                            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                            conn.Send(pet_b);
                            timerRaton.Start();
                            estado = Estado.Normal;
                            this.lblUndo.Visible = false;
                            Trigo -= 2;
                            Piedra -= 3;
                            RefreshBotones();
                        }
                        break;
                }
                pnlTablero.Refresh();
            }
        }

        /// <summary>
        /// Realiza diferentes eventos moviendo el cursor sobre el tablero
        /// </summary>
        private void TabTablero_MouseMove(object sender, MouseEventArgs e)
        {
            // Miramos los diferentes estados del jugador
            switch (estado)
            {
                case Estado.Normal:
                    // Panning / Arrastrar la visión del tablero
                    if (e.Button == MouseButtons.Left && timerRaton.Enabled == false)
                    {
                        basePoint.X += e.Location.X - oldMouse.X;
                        basePoint.Y += e.Location.Y - oldMouse.Y;

                        oldMouse = e.Location;
                        pnlTablero.Refresh();
                    }
                    break;
                case Estado.ColocarCarretera:
                    // Posición del lado más próximo a donde esta el cursor
                    carreteraColocar.Coords = LadoCoords.PixelToLado(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                case Estado.ColocarPoblado:
                    // Posición del vértice más próximo a donde esta el cursor
                    verticeColocar.Coords = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                case Estado.ColocarCiudad:
                    // Posición del vertice más próximo a donde esta el cursor
                    verticeColocar.Coords = VerticeCoords.PixelToVertice(e.Location, basePoint, zoomLevel);
                    pnlTablero.Refresh();
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Permite entrar en el estado colocar carrerera
        /// </summary>
        private void btnCarretera_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            estado = Estado.ColocarCarretera;
            carreteraColocar = new Carretera(0, 0, Lado.Oeste, this.colorJugador);
        }

        /// <summary>
        /// Permite entrar en el estado colocar poblado
        /// </summary>
        private void btnPoblado_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            verticeColocar = new FichaPoblado(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarPoblado;
        }

        /// <summary>
        /// Permite entrar en el estado colocar ciudad
        /// </summary>
        private void btnCiudad_Click(object sender, EventArgs e)
        {
            this.lblUndo.Visible = true;
            verticeColocar = new FichaCiudad(0, 0, Vertice.Superior, this.colorJugador);
            estado = Estado.ColocarCiudad;
        }

        /// <summary>
        /// Comprueba si se puede colocar una carretera en la posición indicada
        /// </summary>
        /// <param name="coords"> Coordenadas lado </param>
        /// <returns> True si se puede o False si no es válida la posición </returns>
        public bool ComprobarCarretera(LadoCoords coords)
        {
            foreach (LadoCoords posible in ladosPosibles)
            {
                if (posible == coords)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Comprueba si se puede colocar una ficha de vértice (poblado o ciudad) 
        /// en la posición indicada
        /// </summary>
        /// <param name="coords"> Coordenadas vértice </param>
        /// <param name="ListaVertices"> Lista de los vértices posibles </param>
        /// <returns> True si se puede o False si no es válida la posición </returns>
        public bool ComprobarFichaVertice(VerticeCoords coords, VerticeCoords[] ListaVertices)
        {
            foreach (VerticeCoords posible in ListaVertices)
            {
                if (posible == coords)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Calcula todos los lados posibles disponibles en el tablero
        /// </summary>
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

        /// <summary>
        /// Calcula todos los vértices posibles en el tablero
        /// </summary>
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
            // Enviar siempre texto no vacío
            if (txtMsg.Text != "")
            {
                // Petición de mensaje chat
                string pet = "13/" + idP.ToString() + "/" + nombre + ": " + txtMsg.Text;
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                txtMsg.Clear();
            }
        }

        /// <summary>
        /// Configura datos de partida cuando se ha cambiado de turno
        /// </summary>
        /// <param name="nombre"> Nombre del jugador del cual es el turno </param>
        public void CambiarTurno(string nombre)
        {
            panelActualizar.BackColor = Color.White;
            this.turno = nombre;
            for (int i = 0; i < nombres.Length; i++)
                if (nombres[i] == turno)
                    this.colorturno = colores[i];
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
                // Se pinta el panel del jugador del turno
                if (panel.Nombre == this.turno)
                {
                    panelActualizar = panel;
                    panelActualizar.BackColor = Color.FromArgb(255, 255, 128);
                }
            }
        }

        /// <summary>
        /// Actualiza la viabilidad de los botones en función del estado
        /// o recursos disponibles del jugador
        /// </summary>
        public void RefreshBotones()
        {
            // Si no te toca no puedes hacer nada
            if (this.nombre != this.turno || this.estado != Estado.Normal) return;

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

            btnComercio.Enabled = this.estado == Estado.Normal;
            btnTurno.Enabled = true;
        }

        /// <summary>
        /// Muestra las imágenes de los dados con los valores sacados
        /// </summary>
        /// <param name="dados"> String de la forma dado1,dado2 </param>
        public void TirarDados(string dados)
        {
            string[] valores = dados.Split(",");
            int dado1 = Convert.ToInt32(valores[0]);
            int dado2 = Convert.ToInt32(valores[1]);
            
            //Animación dados y repartir recursos
            this.sumadados = dado1 + dado2;

            string dado1location = "Dado" + dado1.ToString();
            string dado2location = "Dado" + dado2.ToString();

            lblDado1.Image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(dado1location), new Size(35, 35));
            lblDado2.Image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(dado2location), new Size(35, 35));

            // Si la suma de los números es 7 se activa el ladrón
            if (sumadados == 7)
            {   
                // Si se tienen >7 recursos se deben entregar la mitad
                if ((madera + ladrillo + oveja + trigo + piedra) > 7)
                {
                    FormLadron form = new FormLadron(this.conn, this.idP, this.nombre,
                        madera, ladrillo, oveja, trigo, piedra);
                    form.ShowDialog();
                }
                // Si es el turno se pasa al estado colocar ladrón
                if (this.turno == this.nombre)
                {
                    lblInfo.Text = "Ha salido un siete! Mueve el ladrón a la casilla que quieras";
                    estado = Estado.ColocarLadron;
                    RefreshBotones();
                }
                else
                    lblInfo.Text = "Ha salido un siete! Esperando a que se mueva el ladrón";
                return;
            }

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
            }
        }

        /// <summary>
        /// Se envía la petición de comprar una carta de desarrollo
        /// </summary>
        private void btnDesarrollo_Click(object sender, EventArgs e)
        {
            string pet = "21/" + idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
        }

        /// <summary>
        /// Se llama al método para enviar un mensaje del chat
        /// </summary>
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            EnviarMensaje();
        }

        /// <summary>
        /// Se llama al método para enviar un mensaje del chat al apretar Enter
        /// </summary>
        private void txtMsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                EnviarMensaje();
        }

        /// <summary>
        /// Se envía la petición de pasar el turno
        /// </summary>
        private void btnTurno_Click(object sender, EventArgs e)
        {
            string pet;
            byte[] pet_b;
            puntos = panelActualizar.Puntos + panelActualizar.Larga * 2 + panelActualizar.Ejercito * 2;
            if (puntos >= 10)
            {
                pet = "33/" + idP.ToString() + DateTime.Now.ToString("/dd/MM/yyyy HH:mm");
                pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                return;
            }
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
                    estado = Estado.Normal;
                    btnTurno.Enabled = false;
                    btnTurno.Tag = "";
                    btnTurno.Text = "";
                    btnCarretera.Enabled = false;
                    btnPoblado.Enabled = false;
                    btnCiudad.Enabled = false;
                    btnDesarrollo.Enabled = false;
                    btnComercio.Enabled = false;
                    desarrolloUsada = false;
                    break;
            }
        }

        /// <summary>
        /// Clacula los ratios con o sin puertos y abre el form de comercio
        /// </summary>
        private void btnComercio_Click(object sender, EventArgs e)
        {
            // Si no es tu turno no se puede comerciar
            if (this.nombre != this.turno)
                return;

            // Ratios de intercambio por defecto
            int ratioMadera = 4;
            int ratioLadrillo = 4;
            int ratioOveja = 4;
            int ratioTrigo = 4;
            int ratioPiedra = 4;

            // Comprobar si se tiene una ficha vértice en puerto
            // y recalcalr los ratios correspondientes
            foreach (Puerto puerto in this.puertos)
            {
                foreach (VerticeCoords coords in puerto.Coords.VerticesExtremos())
                {
                    foreach (VerticeCoords poblado in this.misPoblados)
                    {
                        if (poblado == coords)
                        {
                            switch (puerto)
                            {
                                case PuertoGeneral _:
                                    if (ratioMadera == 4) ratioMadera = 3;
                                    if (ratioLadrillo == 4) ratioLadrillo = 3;
                                    if (ratioOveja == 4) ratioOveja = 3;
                                    if (ratioTrigo == 4) ratioTrigo = 3;
                                    if (ratioPiedra == 4) ratioPiedra = 3;
                                    break;
                                case PuertoMadera _:
                                    ratioMadera = 2;
                                    break;
                                case PuertoLadrillo _:
                                    ratioLadrillo = 2;
                                    break;
                                case PuertoOveja _:
                                    ratioOveja = 2;
                                    break;
                                case PuertoTrigo _:
                                    ratioTrigo = 2;
                                    break;
                                case PuertoPiedra _:
                                    ratioPiedra = 2;
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            
            formComerciar = new FormComerciar(this.conn, this.idP, this.nombre, this.nombres, this.colores,
                madera, ladrillo, oveja, trigo, piedra,
                ratioMadera, ratioLadrillo, ratioOveja, ratioTrigo, ratioPiedra);
            formComerciar.ShowDialog();
            formComerciar = null;
            RefreshBotones();
        }

        /// <summary>
        /// Vuelve al estado normal si se estaba en un estado de construcción
        /// </summary>
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

        /// <summary>
        /// Timer para actualizar la obtención/pérdida de recursos y limpiar notificaciones
        /// de acciones de la partida
        /// </summary>
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
            lblInfo.Text = "";
        }

        /// <summary>
        /// Actualiza datos relativos a la obtención de una carta de desarrollo
        /// </summary>
        /// <param name="mensaje"> String de la forma: tipoCarta(integer de 0 a 4) </param>
        public void ComprarCarta(string mensaje)
        {
            // Ya no quedan más cartas disponibles
            if (Convert.ToInt32(mensaje) == -1)
            {
                btnDesarrollo.Enabled = false;
                return;
            }

            // Actialización de recursos 
            panelActualizar.Desarrollo++;
            panelActualizar.Recursos -= 3;
            panelActualizar.Oveja--;
            panelActualizar.Trigo--;
            panelActualizar.Piedra--;

            timerRecursos.Start();

            // Las cartas solo se muestran al jugador que las compró
            if (this.nombre != this.turno)
                return;

            // La carta de punto de victoria otorga punto extra que solo ve el jugador poseedor
            if ((Carta.TipoCarta)Convert.ToInt32(mensaje) == Carta.TipoCarta.Punto)
            {
                panelActualizar.Puntos++;
            }

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

            // Añadir cartas a la mochila del jugador
            cartas.Add(carta);
            int x = 20 * num + 5;
            int y = cliente.Properties.Resources.CartaMonopolio.Size.Height * (int)carta.Tipo + 5;

            carta.Click += Carta_Click;
            pnlCartas.Controls.Add(carta);
            carta.Location = new Point(x, y);
            carta.BringToFront();
            Oveja--;
            Trigo--;
            Piedra--;

            RefreshBotones();
        }


        /// <summary>
        /// Envia las peticiones de usar carta y activa los estados pertinentes si se requiere
        /// </summary>
        private void Carta_Click(object sender, EventArgs e)
        {
            // Si no es tu turno no puedes usar las cartas
            if (this.nombre != this.turno)
                return;
            // Solo se puede usar una carta por turno
            if (desarrolloUsada == false)
            {
                Carta carta = (Carta)sender;

                if ((int)carta.Tipo == 4)
                    return;

                string pet;
                byte[] pet_b;
                // Comprobamos la carta que se ha decidido usar
                switch ((int)carta.Tipo)
                {
                    case 0:     // Monopolio
                        // Abre el form de monopolio y se elegirá el recurso a obtener
                        FormMonopolio form1 = new FormMonopolio();
                        form1.ShowDialog();
                        string recurso = form1.Recurso;
                        if (recurso == "")
                            return;
                        pet = "25/" + idP.ToString() + "/" + recurso;
                        break;
                    case 1:     // Invento
                        // Abre el form invento y se elegiran los dos recursos a obtener
                        FormInvento form2 = new FormInvento();
                        form2.ShowDialog();
                        string[] recursos = form2.recursos;
                        if (recursos[0] == "" || recursos[1] == "")
                            return;
                        pet = "24/" + idP.ToString() + "/" + recursos[0] + "," + recursos[1];
                        break;
                    case 2:     // Caballeria
                        // Se entra en el estado colocar ladrón
                        pet = "22/" + idP.ToString();
                        estado = Estado.ColocarLadron;
                        lblInfo.Text = "Has usado el caballero! Mueve el ladrón a la casilla que quieras";
                        RefreshBotones();
                        break;
                    case 3:     // Carreteras        
                        pet = "23/" + idP.ToString();
                        lblInfo.Text = "Has usado la carta Carreteras! Coloca 2 carreteras sin coste";
                        carreteraColocar = new Carretera(0, 0, Lado.Oeste, this.colorJugador);
                        break;
                    default:
                        return;
                }
                pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                // Elimina la carta una vez usada
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
                desarrolloUsada = true;
            }
        }
        /// <summary>
        /// Actua en función de la carta usada por algun jugador
        /// </summary>
        /// <param name="mensaje"></param>
        public void UsarCarta(string mensaje)
        {
            panelActualizar.Desarrollo--;
                
            string[] trozos = mensaje.Split('/');
            int codigo = Convert.ToInt32(trozos[0]);
            int cantidad;

            // Se comprueba la carta que se ha usado
            switch (codigo)
            {
                case 22:        // Caballero
                    // Se comprueba el nuevo número de caballeros y si es el caso se
                    // cambia de portador el punto de ejercito
                    lblInfo.Text = this.turno + " ha usado la carta Caballero";
                    PanelInfoJugador panelEj = null;
                    foreach (PanelInfoJugador panel in paneles)
                    {
                        if (panel.Ejercito ==  1)
                        {
                            panelEj = panel;
                            break;
                        }
                    }
                    if (panelEj == null)
                    {
                        if (panelActualizar.Caballeros == 2)
                        {
                            panelActualizar.Ejercito = 1;
                        }
                    } else if (panelEj.Nombre != this.turno)
                    {
                        if (panelActualizar.Caballeros == panelEj.Caballeros)
                        {
                            panelEj.Ejercito = 0;
                            panelActualizar.Ejercito = 1;
                        }
                    }
                    panelActualizar.Caballeros++;
                    break;
                case 23:        // Carreteras
                    // Se inicializa variable para no descontar costes de la construcción
                    DosCarreteras = 3;
                    if (this.nombre != this.turno)
                        lblInfo.Text = this.turno + " ha usado la carta Carreteras";
                    else
                        this.estado = Estado.ColocarCarretera;
                    timerRecursos.Start();
                    break;
                case 24:        // Invento
                    // Se obtienen los dos recursos escogidos
                    UsarInvento(trozos[2]);
                    break;
                case 25:        // Monopolio
                    string recurso = trozos[2];                   
                    string pet;
                    byte[] pet_b;
                    // Pone a 0 el recurso que se ha elegido
                    switch (recurso)
                    {
                        case "Madera":
                            cantidad = this.madera;
                            Madera = 0;
                            break;
                        case "Ladrillo":
                            cantidad = this.ladrillo;
                            Ladrillo = 0;
                            break;
                        case "Oveja":
                            cantidad = this.oveja;
                            Oveja = 0;
                            break;
                        case "Trigo":
                            cantidad = this.trigo;
                            Trigo = 0;
                            break;
                        case "Piedra":
                            cantidad = this.piedra;
                            Piedra = 0;
                            break;
                        default:
                            cantidad = 0;
                            break;
                    }
                    // Si no se tiene ningún recurso no se envia la petición
                    if (cantidad == 0)
                        return;

                    // Envia la petición de los recursos a entregar
                    pet = "26/" + idP.ToString() + "/" + this.nombre + "," + recurso + "," + cantidad.ToString();
                    pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                    conn.Send(pet_b);
                    break;
            }
            RefreshBotones();
        }

        /// <summary>
        /// Coloca en tablero una ficha 
        /// </summary>
        /// <param name="mensaje"> String con las siguientes formas posibles:
        /// 17/idPartida/R,Q    --> colocar ladrón
        /// 18/idPartida/R,Q,V  --> colocar poblado
        /// 19/idPartida/R,Q,V  --> colocar ciudad
        /// 20/idPartida/R,Q,L  --> colocar carretera
        /// </param>
        public void Colocar(string mensaje)
        {
            string[] trozos = mensaje.Split('/');
            int codigo = Convert.ToInt32(trozos[0]);
            string[] coordenadas = trozos[2].Split(',');

            // La ficha será del color del jugador de ese turno
            ColorJugador Color = this.colorJugador;

            for (int i = 0; i < numJugadores; i++)
            {
                if (nombres[i] == turno)
                    Color = colores[i]; 
            }

            // Mediante el código del mensaje sabemos que se esta colocando
            switch (codigo)
            {
                case 17:            // ladron
                    posicionLadron = new HexCoords(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]));
                    if(this.nombre != this.turno)
                        lblInfo.Text = "";
                    pnlTablero.Refresh();
                    break;
                case 18:            // poblado
                    verticeColocar = new FichaPoblado(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Vertice)Convert.ToInt32(coordenadas[2]), Color);
                    fichasVertices.Add(verticeColocar);
                    pnlTablero.Refresh();
                    panelActualizar.Puntos += 1;
                    if (numturnos > (numJugadores * 2))
                    {
                        panelActualizar.Madera--;
                        panelActualizar.Ladrillo--;
                        panelActualizar.Oveja--;
                        panelActualizar.Trigo--;
                        panelActualizar.Recursos -= 4;
                        timerRecursos.Start();
                    }
                    else if (numturnos > numJugadores && numturnos <= (numJugadores * 2))
                    {
                        int suma = 0;
                        foreach (Tile Casilla in tiles[18..])
                        {
                            foreach (VerticeCoords vertice in Casilla.Coords.Vertices)
                            {
                                if (vertice == fichasVertices.Last().Coords)
                                {
                                    switch (Casilla)
                                    {
                                        case TileMadera _:
                                            if (this.nombre == this.turno)
                                                Madera++;
                                            panelActualizar.Madera++;
                                            suma++;
                                            break;
                                        case TileLadrillo _:
                                            if (this.nombre == this.turno)
                                                Ladrillo++;
                                            panelActualizar.Ladrillo++;
                                            suma++;
                                            break;
                                        case TileOveja _:
                                            if (this.nombre == this.turno)
                                                Oveja++;
                                            panelActualizar.Oveja++;
                                            suma++;
                                            break;
                                        case TileTrigo _:
                                            if (this.nombre == this.turno)
                                                Trigo++;
                                            panelActualizar.Trigo++;
                                            suma++;
                                            break;
                                        case TilePiedra _:
                                            if (this.nombre == this.turno)
                                                Piedra++;
                                            panelActualizar.Piedra++;
                                            suma++;
                                            break;
                                    }
                                }
                            }
                        }
                        panelActualizar.Recursos += suma;
                        timerRecursos.Start();
                    }
                    break;
                case 19:            // ciudad
                    verticeColocar = new FichaCiudad(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Vertice)Convert.ToInt32(coordenadas[2]), Color);
                    foreach (FichaVertice ficha in fichasVertices)
                    {
                        if (ficha.Coords == verticeColocar.Coords)
                        {
                            fichasVertices.Remove(ficha);
                            break;
                        }
                    }
                    fichasVertices.Add(verticeColocar);
                    pnlTablero.Refresh();
                    panelActualizar.Puntos += 1;
                    if (numturnos > (numJugadores * 2))
                    {
                        panelActualizar.Trigo -= 2;
                        panelActualizar.Piedra -= 3;
                        panelActualizar.Recursos -= 5;
                    }
                        timerRecursos.Start();
                    break;
                case 20:            // carretera
                    carreteraColocar = new Carretera(Convert.ToInt32(coordenadas[1]), Convert.ToInt32(coordenadas[0]), (Lado)Convert.ToInt32(coordenadas[2]), Color);
                    carreteras.Add(carreteraColocar);
                    pnlTablero.Refresh();
                    Dictionary<VerticeCoords, List<LadoCoords>> vertices = new Dictionary<VerticeCoords, List<LadoCoords>>();
                    List<LadoCoords> lados = new List<LadoCoords>();
                    foreach (Carretera c in this.carreteras)
                    {
                        if (c.Color == carreteraColocar.Color)
                        {
                            lados.Add(c.Coords);
                            foreach (VerticeCoords vertice in c.Coords.VerticesExtremos())
                            {
                                if (!vertices.ContainsKey(vertice))
                                {
                                    vertices[vertice] = new List<LadoCoords>();
                                }
                                vertices[vertice].Add(c.Coords);
                            }
                        }
                    }

                    Dictionary<LadoCoords, VerticeCoords> extremos = new Dictionary<LadoCoords, VerticeCoords>();
                    foreach (VerticeCoords v in vertices.Keys)
                    {
                        if (vertices[v].Count == 1)
                        {
                            extremos[vertices[v][0]] = v;
                        }
                    }
                    int maxCarreteras = 0;
                    foreach (LadoCoords ladoExtremo in extremos.Keys)
                    {
                        int ret = MaximaLongitud(extremos[ladoExtremo], ladoExtremo, lados, vertices);
                        if (ret > maxCarreteras)
                            maxCarreteras = ret;
                    }

                    panelActualizar.Carreteras = maxCarreteras;
                    if (maxCarreteras >= 5)
                    {
                        PanelInfoJugador panelLarga = null;
                        foreach (PanelInfoJugador panel in paneles)
                        {
                            if (panel.Larga == 1)
                            {
                                panelLarga = panel;
                                break;
                            }
                        }
                        if (panelLarga == null)
                        {
                            panelActualizar.Larga = 1;
                        }
                        else if (panelLarga.Nombre != this.turno)
                        {
                            if (maxCarreteras > panelLarga.Carreteras)
                            {
                                panelLarga.Larga = 0;
                                panelActualizar.Larga = 1;
                            }
                        }
                    }
                    if (numturnos > (numJugadores * 2) && DosCarreteras == 0)
                    {
                        panelActualizar.Madera--;
                        panelActualizar.Ladrillo--;
                        panelActualizar.Recursos -= 2;
                        timerRecursos.Start();
                    }
                    if (this.turno == this.nombre)
                    {
                        carreteraColocar = new Carretera(0, 0, Lado.Oeste, this.colorJugador);
                        if (DosCarreteras == 1)
                            DosCarreteras--;
                    }
                    break;
            }
            RecalcularLadosPosibles();
            RecalcularVerticesPosibles();
        }

        /// <summary>
        /// Envia la petición para notificar de los recursos a entregar
        /// a quien ha usado la carta monopolio
        /// </summary>
        /// <param name="mensaje"> String de la forma: nombreEntrega,recurso,cantidad </param>
        public void DarMonopolio(string mensaje)
        {
            string[] trozos = mensaje.Split(',');
            string donante = trozos[0];
            string recurso = trozos[1];
            int cantidad = Convert.ToInt32(trozos[2]);

            // Comprueba si es tu turno
            if (this.turno == this.nombre)
            {
                // Añade los recursos 
                switch (recurso)
                {
                    case "Madera":
                        Madera += cantidad;
                        break;
                    case "Ladrillo":
                        Ladrillo += cantidad;
                        break;
                    case "Oveja":
                        Oveja += cantidad;
                        break;
                    case "Trigo":
                        Trigo += cantidad;
                        break;
                    case "Piedra":
                        Piedra += cantidad;
                        break;
                }
            }
            
            // Indica el balance de recursos en el panel
            foreach (PanelInfoJugador panel in paneles)
            {
                if (panel.Nombre == donante)
                {
                    panel.Recursos -= cantidad;
                    switch (recurso)
                    {
                        case "Madera":
                            panel.Madera -= cantidad;
                            break;
                        case "Ladrillo":
                            panel.Ladrillo -= cantidad;
                            break;
                        case "Oveja":
                            panel.Oveja -= cantidad;
                            break;
                        case "Trigo":
                            panel.Trigo -= cantidad;
                            break;
                        case "Piedra":
                            panel.Piedra -= cantidad;
                            break;
                    }
                }
                else if(panel.Nombre == this.turno)
                {
                    panel.Recursos += cantidad;
                    switch (recurso)
                    {
                        case "Madera":
                            panel.Madera += cantidad;
                            break;
                        case "Ladrillo":
                            panel.Ladrillo += cantidad;
                            break;
                        case "Oveja":
                            panel.Oveja += cantidad;
                            break;
                        case "Trigo":
                            panel.Trigo += cantidad;
                            break;
                        case "Piedra":
                            panel.Piedra += cantidad;
                            break;
                    }
                }
            }
            lblInfo.Text = this.turno + " ha usado la carta de monopolio";
            timerRecursos.Start();
        }

        /// <summary>
        /// Se indica que el jugador del turno presenta una oferta
        /// </summary>
        /// <param name="mensaje"> String de la forma: OMadera,OLadrillo...,PMadera,PLadrillo... 
        /// Donde O representa el número de ese recurso que se ofrece y P el número de ese
        /// recurso que se quiere a cambio </param>
        public void ComercioOferta(string mensaje)
        {
            if (this.formComerciar != null) return;

            FormOferta form = new FormOferta(this.conn, this.idP, this.nombre, this.colorturno, mensaje,
                madera, ladrillo, oveja, trigo, piedra);
            form.ShowDialog();
        }

        /// <summary>
        /// Pasa el mensaje al form de comercio
        /// </summary>
        /// <param name="mensaje"> String de la forma: NombreAcepta/SI o NO </param>
        public void ComercioRespuesta(string mensaje)
        {
            // Solo se pasa el mensaje si no se ha cerrado el form
            if (this.formComerciar != null)
            {
                formComerciar.ActualizarRespuesta(mensaje);
            }
        }

        /// <summary>
        /// Actualiza los parámetros acorde a un comercio realizado
        /// </summary>
        /// <param name="mensaje"> String de la forma: NombreAcepta/OMadera,OLadrillo...,PMadera,PLadrillo... </param>
        /// Donde O representa el número de ese recurso que se ofrece y P el número de ese
        /// recurso que se quiere a cambio </param>
        public void ComercioResultado(string mensaje)
        {
            string[] trozos = mensaje.Split("/");
            int[] recursos = new int[10];
            int i = 0;
            foreach (string num in trozos[3].Split(","))
            {
                recursos[i++] = Convert.ToInt32(num);
            }
            
            // Actualización del balance de recursos
            if (this.nombre == this.turno)
            {
                Madera += recursos[5] - recursos[0];
                Ladrillo += recursos[6] - recursos[1];
                Oveja += recursos[7] - recursos[2];
                Trigo += recursos[8] - recursos[3];
                Piedra += recursos[9] - recursos[4];
                RefreshBotones();
            } else if (this.nombre == trozos[2])
            {
                Madera -= recursos[5] - recursos[0];
                Ladrillo -= recursos[6] - recursos[1];
                Oveja -= recursos[7] - recursos[2];
                Trigo -= recursos[8] - recursos[3];
                Piedra -= recursos[9] - recursos[4];
            }

            panelActualizar.Recursos += recursos[5..].Sum() - recursos[..5].Sum();
            panelActualizar.Madera += recursos[5] - recursos[0];
            panelActualizar.Ladrillo += recursos[6] - recursos[1];
            panelActualizar.Oveja += recursos[7] - recursos[2];
            panelActualizar.Trigo += recursos[8] - recursos[3];
            panelActualizar.Piedra += recursos[9] - recursos[4];
            foreach (PanelInfoJugador panel in paneles)
            {
                if (panel.Nombre == trozos[2])
                {
                    panel.Recursos -= recursos[5..].Sum() - recursos[..5].Sum();
                    panel.Madera -= recursos[5] - recursos[0];
                    panel.Ladrillo -= recursos[6] - recursos[1];
                    panel.Oveja -= recursos[7] - recursos[2];
                    panel.Trigo -= recursos[8] - recursos[3];
                    panel.Piedra -= recursos[9] - recursos[4];
                    break;
                }
            }
            timerRecursos.Start();
        }

        /// <summary>
        /// Evento de Tick del reloj
        /// </summary>
        private void timerRaton_Tick(object sender, EventArgs e)
        {
            timerRaton.Stop();
        }

        /// <summary>
        /// Se indica que se ha producido un comercio de un jugador con la banca/puerto
        /// </summary>
        /// <param name="mensaje"> String de la forma: OMadera,OLadrillo...,PMadera,PLadrillo... </param>
        /// Donde O representa el número de ese recurso que se ofrece y P el número de ese
        /// recurso que se quiere a cambio </param> 
        public void ComercioMaritimo(string mensaje)
        {
            string[] trozos = mensaje.Split("/");
            int[] recursos = new int[10];
            int i = 0;

            // Actualizar balance de los recursos
            foreach (string num in trozos[2].Split(","))
            {
                recursos[i++] = Convert.ToInt32(num);
            }

            if (this.nombre == this.turno)
            {
                Madera += recursos[5] - recursos[0];
                Ladrillo += recursos[6] - recursos[1];
                Oveja += recursos[7] - recursos[2];
                Trigo += recursos[8] - recursos[3];
                Piedra += recursos[9] - recursos[4];
                RefreshBotones();
            }

            panelActualizar.Recursos += recursos[5..].Sum() - recursos[..5].Sum();
            panelActualizar.Madera += recursos[5] - recursos[0];
            panelActualizar.Ladrillo += recursos[6] - recursos[1];
            panelActualizar.Oveja += recursos[7] - recursos[2];
            panelActualizar.Trigo += recursos[8] - recursos[3];
            panelActualizar.Piedra += recursos[9] - recursos[4];
            timerRecursos.Start();
        }

        /// <summary>
        /// Indica que un jugador a entregado al ladrón el recurso robado
        /// </summary>
        /// <param name="mensaje"></param>
        public void DarLadron(string mensaje)
        {
            string[] trozos = mensaje.Split('/');
            string donante = trozos[2];

            // Desechar recursos
            if (trozos[0] == "31")
            {
                int[] recursos = new int[5];

                int i = 0;
                foreach (string num in trozos[3].Split(","))
                {
                    recursos[i++] = Convert.ToInt32(num);
                }

                int cantidad = recursos[0] + recursos[1] + recursos[2] + recursos[3] + recursos[4];

                // Has desechado recursos o le has dado un recurso al ladron
                if (donante == this.nombre)
                {
                    Madera -= recursos[0];
                    Ladrillo -= recursos[1];
                    Oveja -= recursos[2];
                    Trigo -= recursos[3];
                    Piedra -= recursos[4];

                    if (cantidad == 1)
                        lblInfo.Text = "Has sido robado por " + this.turno;

                    if (this.turno == this.nombre)
                    {
                        RefreshBotones();
                    }
                }
                // Te entregan un recurso del robo
                if (this.turno == this.nombre && cantidad == 1)
                {
                    Madera += recursos[0];
                    Ladrillo += recursos[1];
                    Oveja += recursos[2];
                    Trigo += recursos[3];
                    Piedra += recursos[4];
                    RefreshBotones();
                }
                if (cantidad == 1 && donante != this.nombre)
                    lblInfo.Text = this.turno + " ha robado a " + donante;

                // Actualizar balance de recursos
                foreach (PanelInfoJugador panel in paneles)
                {
                    if (panel.Nombre == donante)
                    {
                        panel.Recursos -= cantidad;
                        panel.Madera -= recursos[0];
                        panel.Ladrillo -= recursos[1];
                        panel.Oveja -= recursos[2];
                        panel.Trigo -= recursos[3];
                        panel.Piedra -= recursos[4];
                    }
                    else if (panel.Nombre == this.turno && cantidad == 1)
                    {
                        panel.Recursos += cantidad;
                        panel.Madera += recursos[0];
                        panel.Ladrillo += recursos[1];
                        panel.Oveja += recursos[2];
                        panel.Trigo += recursos[3];
                        panel.Piedra += recursos[4];
                    }
                }
                timerRecursos.Start();
            }
            // Petición 32 - Robar recursos por el ladrón
            else
            {
                // Te roban
                if (donante == this.nombre && (madera + ladrillo + oveja + trigo + piedra != 0))
                {
                    string resto = "";
                    Random rnd = new Random();
                    int num = 0;

                    int[] recursos = new int[5] { madera, ladrillo, oveja, trigo, piedra };
                    int dar = 0;

                    // Das un recurso al azar
                    while (dar == 0)
                    {
                        num = rnd.Next(0, 4);
                        dar = recursos[num];
                    }

                    string pet;
                    byte[] pet_b;
                    
                    switch (num)
                    {
                        case 0:
                            resto = "1,0,0,0,0";
                            break;
                        case 1:
                            resto = "0,1,0,0,0";
                            break;
                        case 2:
                            resto = "0,0,1,0,0";
                            break;
                        case 3:
                            resto = "0,0,0,1,0";
                            break;
                        case 4:
                            resto = "0,0,0,0,1";
                            break;
                    }
                    pet = "31/" + idP.ToString() + "/" + this.nombre + "/" + resto;
                    pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                    conn.Send(pet_b);
                }
            }
        }

        /// <summary>
        /// Indica que alguien ha usado la carta invento
        /// </summary>
        /// <param name="mensaje"></param>
        private void UsarInvento(string mensaje)
        {
            if (this.nombre != this.turno)
                lblInfo.Text = this.turno + " ha usado la carta de invento";

            string[] recursos = new string[] { mensaje.Split(',')[0], mensaje.Split(',')[1] };
            int cantidad = 1;

            // Comporbar si se pide el mismo 2 veces
            if (recursos[0] == recursos[1])
            {
                cantidad = 2;
                recursos = new string[] { mensaje.Split(',')[0] };

            }
            // añadir recursos a la bolsa de recursos
            foreach (string rec in recursos)
            {
                switch (rec)
                {
                    case "Madera":
                        if (this.nombre == this.turno) 
                            Madera += cantidad;
                        panelActualizar.Madera += cantidad;
                        break;
                    case "Ladrillo":
                        if (this.nombre == this.turno)
                            Ladrillo += cantidad;
                        panelActualizar.Ladrillo += cantidad;
                        break;
                    case "Oveja":
                        if (this.nombre == this.turno)
                            Oveja += cantidad;
                        panelActualizar.Oveja += cantidad;
                        break;
                    case "Trigo":
                        if (this.nombre == this.turno)
                            Trigo += cantidad;
                        panelActualizar.Trigo += cantidad;
                        break;
                    case "Piedra":
                        if (this.nombre == this.turno)
                            Piedra += cantidad;
                        panelActualizar.Piedra += cantidad;
                        break;
                }
                timerRecursos.Start();
            }
            panelActualizar.Recursos += 2;
        }

        /// <summary>
        /// Calcula recursivamente el numero máximo de carreteras conectadas
        /// </summary>
        /// <param name="s"> Coordenadas vertice </param>
        /// <param name="lado"> Coordenadas lado </param>
        /// <param name="lados"> </param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        private int MaximaLongitud(VerticeCoords s, LadoCoords lado, List<LadoCoords> lados,
            Dictionary<VerticeCoords, List<LadoCoords>> vertices)
        {
            foreach (VerticeCoords v in lado.VerticesExtremos())
            {
                if (s != v)
                {
                    // Si se queda en 0, este es un lado extremo
                    int max = 0;
                    foreach (LadoCoords l in vertices[v])
                    {
                        if (l != lado)
                        {
                            int ret = MaximaLongitud(v, l, lados, vertices);
                            if (ret > max)
                                max = ret;
                        }
                    }
                    return max + 1;
                }
            }
            // Nunca ocurrirá
            return -10000;
        }

        /// <summary>
        /// Cambia la apariencia de los botones disponibles
        /// </summary>
        private void btnPrincipal_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Enabled == true)
                btn.BackColor = Color.FromArgb(255, 255, 192);
            else
                btn.BackColor = Color.WhiteSmoke;
        }
    }
}

