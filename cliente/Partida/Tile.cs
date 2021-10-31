using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace cliente.Partida
{
    public class Tile
    {
        public const int BRADIUS = 256;
        public const int BWIDTH = 443;
        public const int BHEIGHT = 512;

        public readonly int r, q;
        public readonly int? valor;

        public Point oldPos;

        public Tile(int q, int r, int? valor)
        {
            this.r = r;
            this.q = q;
            this.valor = valor;
        }

        public Point PixelCoords(Point basePoint, int zoomLevel)
        {
            basePoint.X += (int)(BRADIUS * (Math.Sqrt(3) * this.q + Math.Sqrt(3) / 2 * (this.r - 1)) / zoomLevel);
            basePoint.Y += (int)(BRADIUS * (1.5 * (this.r) - 1) / zoomLevel);
            return basePoint;
        }
    }

    class TileDesierto : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TileDesierto.png", true);

        public TileDesierto(int q, int r) : base(q, r, null)
        {
        }
    }

    class TileMar : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TileDesierto.png", true);

        public TileMar(int q, int r) : base(q, r, null)
        {
        }
    }

    class TileMadera : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TileMadera.png", true);

        public TileMadera(int q, int r, int value) : base(q, r, value)
        {
        }
    }

    class TileLadrillo : Tile
    {
        public static readonly Image Image  = Image.FromFile("../../../Media/Images/TileLadrillo.png", true);

        public TileLadrillo(int q, int r, int value) : base(q, r, value)
        {
        }
    }

    class TileOveja : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TileOveja.png", true);

        public TileOveja(int q, int r, int value) : base(q, r, value)
        {
        }
    }

    class TileTrigo : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TileTrigo.png", true);

        public TileTrigo(int q, int r, int value) : base(q, r, value)
        {
        }
    }

    class TilePiedra : Tile
    {
        public static readonly Image Image = Image.FromFile("../../../Media/Images/TilePiedra.png", true);

        public TilePiedra(int q, int r, int value) : base(q, r, value)
        {
        }
    }
}
