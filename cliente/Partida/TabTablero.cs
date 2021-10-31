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
    }

    public partial class TabTablero : Tab
    {
        Tile[] tiles;

        int zoomLevel;

        Point basePoint;
        Point oldMouse;

        public TabTablero()
        {
            InitializeComponent();
        }

        private void TabTablero_Load(object sender, EventArgs e)
        {
            this.tiles = TableroPrueba.GetTiles();
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

            Image img;
            Size size = new Size(Tile.BWIDTH / this.zoomLevel, Tile.BHEIGHT / this.zoomLevel);
            foreach (Tile tile in this.tiles)
            {
                switch (tile)
                {
                    case TileDesierto desierto:
                        img = TileDesierto.Image;
                        break;
                    case TileMar mar:
                        img = TileMar.Image;
                        break;
                    case TileMadera madera:
                        img = TileMadera.Image;
                        break;
                    case TileLadrillo ladrillo:
                        img = TileLadrillo.Image;
                        break;
                    case TileOveja oveja:
                        img = TileOveja.Image;
                        break;
                    case TileTrigo trigo:
                        img = TileTrigo.Image;
                        break;
                    case TilePiedra piedra:
                        img = TilePiedra.Image;
                        break;
                    default:
                        img = TileDesierto.Image;
                        break;
                }
                e.Graphics.DrawImage(img, new Rectangle(tile.PixelCoords(this.basePoint, this.zoomLevel), size));
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
