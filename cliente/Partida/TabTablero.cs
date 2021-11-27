using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace cliente.Partida
{
    public partial class TabTablero : TabPartida
    {
        Tile[] tiles;
        List<FichaVertice> fichasVertices;
        List<Puerto> puertos;
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

        public TabTablero()
        {
            InitializeComponent();
        }

        private void TabTablero_Load(object sender, EventArgs e)
        {
            this.tiles = TableroPrueba.GetTiles();
            this.fichasVertices = TableroPrueba.GetFichasVertices();
            this.carreteras = TableroPrueba.GetCarreteras();
            this.puertos = TableroPrueba.GetPuertos();
            // Calcular las posiciones posibles de carreteras y poblados
            List<VerticeCoords> vertices = new List<VerticeCoords>();
            List<LadoCoords> lados = new List<LadoCoords>();
            foreach (Tile tile in this.tiles)
            {
                if (tile is TileMar)
                    continue;
                lados.AddRange(tile.Coords.Lados());
                vertices.AddRange(tile.Coords.Vertices());
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
            e.Graphics.Clear(Color.Gray);

            Bitmap bmp;
            Size size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles)
            {
                switch (tile)
                {
                    case TileDesierto desierto:
                        bmp = TileDesierto.Bitmap;
                        break;
                    case TileMar mar:
                        bmp = TileMar.Bitmap;
                        break;
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
            // Dibujar Puertos
            size = new Size((Tile.BWIDTH + 2 * Puerto.DX) / zoomLevel, (Tile.BHEIGHT + 2 * Puerto.DY) / zoomLevel);
            foreach (Puerto puerto in puertos)
            {
                e.Graphics.DrawImage(puerto.Bitmap, new Rectangle(puerto.LadoToPixel(basePoint, zoomLevel), size));
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
    }
}
