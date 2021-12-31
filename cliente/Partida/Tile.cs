using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace cliente.Partida
{
    public struct HexCoords
    {
        public int Q;
        public int R;

        public VerticeCoords[] Vertices
        {
            get
            {
                return new VerticeCoords[]
                {
                    // Empezamos a contar desde el vértice superior hacia la derecha
                    new VerticeCoords(Q, R, Vertice.Superior),            // N
                    new VerticeCoords(Q + 1, R - 1, Vertice.Inferior),    // NE
                    new VerticeCoords(Q, R + 1, Vertice.Superior),        // SE
                    new VerticeCoords(Q, R, Vertice.Inferior),            // S
                    new VerticeCoords(Q - 1, R + 1, Vertice.Superior),    // SO
                    new VerticeCoords(Q, R - 1, Vertice.Inferior)         // NO
                };
            }
        }

        public LadoCoords[] Lados
        {
            get
            {

                return new LadoCoords[]
                {
                    new LadoCoords(Q, R, Lado.Norte),
                    new LadoCoords(Q + 1, R - 1, Lado.Sur),
                    new LadoCoords(Q + 1, R, Lado.Oeste),
                    new LadoCoords(Q, R + 1, Lado.Norte),
                    new LadoCoords(Q, R, Lado.Sur),
                    new LadoCoords(Q, R, Lado.Oeste)
                };
            }
        }

        public HexCoords[] Vecinos
        {
            get
            {
                return new HexCoords[]
                {
                    new HexCoords(this.Q + 1, this.R),
                    new HexCoords(this.Q + 1, this.R - 1),
                    new HexCoords(this.Q, this.R - 1),
                    new HexCoords(this.Q - 1, this.R),
                    new HexCoords(this.Q - 1, this.R + 1),
                    new HexCoords(this.Q, this.R + 1)
                };
            }
        }

        public HexCoords(int Q, int R)
        {
            this.Q = Q;
            this.R = R;
        }

        public static HexCoords PixelToHex(Point pixelCoords, Point basePoint, int zoomLevel)
        {
            int x = pixelCoords.X - basePoint.X;
            int y = pixelCoords.Y - basePoint.Y;
            double q_frac = (Math.Sqrt(3) / 3 * x - 1.0 / 3 * y) / Tile.BRADIUS * zoomLevel;
            double r_frac = (2.0 / 3 * y) / Tile.BRADIUS * zoomLevel;
            double s_frac = -q_frac - r_frac;
            // Round in cube coordinates
            int q = (int)Math.Round(q_frac);
            int r = (int)Math.Round(r_frac);
            int s = (int)Math.Round(s_frac);
            double q_diff = Math.Abs(q - q_frac);
            double r_diff = Math.Abs(r - r_frac);
            double s_diff = Math.Abs(s - s_frac);
            if (q_diff > r_diff && r_diff > s_diff)
            {
                return new HexCoords(-r - s, r);
            }
            else if (r_diff > s_diff)
            {
                return new HexCoords(q, -q - s);
            }
            else
            {
                return new HexCoords(q, r);
            }
        }

        public Point HexToPixel(Point basePoint, int zoomLevel)
        {
            basePoint.X += (int)(Tile.BRADIUS * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * (R - 1)) / zoomLevel);
            basePoint.Y += (int)(Tile.BRADIUS * (1.5 * R - 1) / zoomLevel);
            return basePoint;
        }

        public override string ToString()
        {
            return String.Format("HexCoords({0}, {1})", Q, R);
        }
    }

    public class Tile
    {
        public const int BRADIUS = 256;
        public const int BWIDTH = 443;
        public const int BHEIGHT = 512;

        public readonly HexCoords Coords;
        public readonly int? Valor;

        public virtual Bitmap Bitmap { get; }

        public Tile(int q, int r, int? valor)
        {
            this.Coords = new HexCoords(q, r);
            this.Valor = valor;
        }

        public Point HexToPixel(Point basePoint, int zoomLevel)
        {
            return Coords.HexToPixel(basePoint, zoomLevel);
        }
    }

    class TileDesierto : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileDesiertoBmp; }

        public TileDesierto(int q, int r) : base(q, r, null)
        {
        }
    }

    class TileMar : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileMarBmp; }

        public TileMar(int q, int r) : base(q, r, null)
        {
        }
    }

    class TileMadera : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileMaderaBmp; }

        public TileMadera(int q, int r, int valor) : base(q, r, valor)
        {
        }
    }

    class TileLadrillo : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileLadrilloBmp; }

        public TileLadrillo(int q, int r, int valor) : base(q, r, valor)
        {
        }
    }

    class TileOveja : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileOvejaBmp; }

        public TileOveja(int q, int r, int valor) : base(q, r, valor)
        {
        }
    }

    class TileTrigo : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TileTrigoBmp; }

        public TileTrigo(int q, int r, int valor) : base(q, r, valor)
        {
        }
    }

    class TilePiedra : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.TilePiedraBmp; }

        public TilePiedra(int q, int r, int valor) : base(q, r, valor)
        {
        }
    }

    class TileLadron : Tile
    {
        public override Bitmap Bitmap { get => cliente.Properties.Resources.Ladron; }

        public TileLadron(int q, int r) : base(q, r, null)
        {
        }
    }
}
