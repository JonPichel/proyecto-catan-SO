using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public static class TableroPrueba
    {
        public static Tile[] GetTiles()
        {
            return new Tile[] {
                new TileMar(0, -3),
                new TileMar(1, -3),
                new TileMar(2, -3),
                new TileMar(3, -3),
                new TileMar(-1, -2),
                new TileMadera(0, -2, 6),
                new TileOveja(1, -2, 3),
                new TileOveja(2, -2, 8),
                new TileMar(3, -2),
                new TileMar(-2, -1),
                new TileTrigo(-1, -1, 2),
                new TilePiedra(0, -1, 4),
                new TileTrigo(1, -1, 5),
                new TileMadera(2, -1, 10),
                new TileMar(3, -1),
                new TileMar(-3, 0),
                new TileMadera(-2, 0, 5),
                new TileLadrillo(-1, 0, 9),
                new TileDesierto(0, 0),
                new TilePiedra(1, 0, 6),
                new TileTrigo(2, 0, 9),
                new TileMar(3, 0),
                new TileMar(-3, 1),
                new TileTrigo(-2, 1, 10),
                new TilePiedra(-1, 1, 11),
                new TileMadera(0, 1, 3),
                new TileOveja(1, 1, 12),
                new TileMar(2, 1),
                new TileMar(-3, 2),
                new TileLadrillo(-2, 2, 8),
                new TileOveja(-1, 2, 4),
                new TileLadrillo(0, 2, 11),
                new TileMar(1, 2),
                new TileMar(-3, 3),
                new TileMar(-2, 3),
                new TileMar(-1, 3),
                new TileMar(0, 3)
            };
        }

        public static List<FichaVertice> GetFichasVertices()
        {
            return new List<FichaVertice>()
            {
                new FichaPoblado(1, -1, FichaVertice.Vertice.Superior, ColorJugador.Rojo),
                new FichaCiudad(-1, 1, FichaVertice.Vertice.Inferior, ColorJugador.Azul)
            };
        }
    }

    public partial class TabTablero : Tab
    {
        Tile[] tiles;
        List<FichaVertice> fichasVertices;

        int zoomLevel;

        Point basePoint;
        Point oldMouse;

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
            this.zoomLevel = 8;
            this.basePoint = new Point(this.Width / 2, this.Height / 2);

            this.Paint += TabTablero_Paint;
            this.MouseWheel += TabTablero_MouseWheel;
            this.MouseDown += TabTablero_MouseDown;
            this.MouseMove += TabTablero_MouseMove;

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
                Rectangle rect = new Rectangle(tile.PixelCoords(this.basePoint, this.zoomLevel), size);
                e.Graphics.DrawImage(bmp, rect);
                if (tile.valor != null)
                {
                    e.Graphics.DrawImage(numbers[(int)tile.valor - 1], rect);
                }
            }
            size = new Size(FichaVertice.BHALFSIDE * 2 / this.zoomLevel, FichaVertice.BHALFSIDE * 2 / this.zoomLevel);
            foreach (FichaVertice ficha in fichasVertices)
            {
                switch (ficha)
                {
                    case FichaPoblado poblado:
                        bmp = FichaPoblado.Bitmap;
                        break;
                    case FichaCiudad ciudad:
                        bmp = FichaCiudad.Bitmap;
                        break;
                    default:
                        bmp = FichaPoblado.Bitmap;
                        break;
                }
                e.Graphics.DrawImage(bmp, new Rectangle(ficha.PixelCoords(this.basePoint, this.zoomLevel), size));
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
                this.zoomLevel = Math.Min(zoomLevel + 1, 12);
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
                oldMouse = e.Location;
            }
        }

        private void TabTablero_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                basePoint.X += e.Location.X - oldMouse.X;
                basePoint.Y += e.Location.Y - oldMouse.Y;

                oldMouse = e.Location;
                this.Refresh();
            }
        }
    }
}
