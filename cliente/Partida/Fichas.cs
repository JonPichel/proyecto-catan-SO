using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace cliente.Partida
{
    public enum ColorJugador
    {
        Rojo,
        Verde,
        Azul,
        Amarillo,
        Naranja,
        Morado
    };
            public enum Vertice
        {
            Superior,
            Inferior
        };

    public struct VerticeCoords
    {
        public HexCoords HexCoords;
        public int Q
        {
            get => HexCoords.Q;
        }
        public int R
        {
            get => HexCoords.R;
        }
        public Vertice V;

        public VerticeCoords(int q, int r, Vertice v)
        {
            this.HexCoords = new HexCoords(q, r);
            this.V = v;
        }

        public VerticeCoords(HexCoords hexCoords, Vertice v)
        {
            this.HexCoords = hexCoords;
            this.V = v;
        }

        public static VerticeCoords PixelToVertice(Point pixelCoords, Point basePoint, int zoomLevel)
        {
            // Grid centrada en vertice superior
            Point displacedSuperior = new Point(pixelCoords.X, pixelCoords.Y + Tile.BRADIUS / zoomLevel);
            // Grid centrada en vertice inferior
            Point displacedInferior = new Point(pixelCoords.X - (int)(Math.Sqrt(3) / 2 * Tile.BRADIUS / zoomLevel),
                pixelCoords.Y + Tile.BRADIUS / 2 / zoomLevel);
            HexCoords hexSuperior = HexCoords.PixelToHex(displacedSuperior, basePoint, zoomLevel);
            HexCoords hexInferior = HexCoords.PixelToHex(displacedInferior, basePoint, zoomLevel);
            Point pixelSuperior = hexSuperior.HexToPixel(basePoint, zoomLevel);
            pixelSuperior.X += Tile.BWIDTH / 2 / zoomLevel;
            pixelSuperior.Y += Tile.BHEIGHT / 2 / zoomLevel;
            Point pixelInferior = hexInferior.HexToPixel(basePoint, zoomLevel);
            pixelInferior.X += Tile.BWIDTH / 2 / zoomLevel;
            pixelInferior.Y += Tile.BHEIGHT / 2 / zoomLevel;
            int dxS = pixelSuperior.X - displacedSuperior.X;
            int dyS = pixelSuperior.Y - displacedSuperior.Y;
            int dxI = pixelInferior.X - displacedInferior.X;
            int dyI = pixelInferior.Y - displacedInferior.Y;
            if ((dxS * dxS + dyS * dyS) < (dxI * dxI + dyI * dyI))
            {
                return new VerticeCoords(hexSuperior, Vertice.Superior);
            } else
            {
                return new VerticeCoords(hexInferior.Q + 1, hexInferior.R - 1, Vertice.Inferior);
            }
        }

        public override String ToString()
        {
            return String.Format("VerticeCoords({0}, {1}, {2})", Q, R, V);
        }
    }

    public class FichaVertice
    {
        public const int BHALFSIDE = 130 / 2;

        public VerticeCoords Coords;
        public ColorJugador Color;

        public FichaVertice(int q, int r, Vertice v, ColorJugador color)
        {
            this.Coords.HexCoords = new HexCoords(q, r); 
            this.Coords.V = v;
            this.Color = color;
        }

        public Point HexToPixel(Point basePoint, int zoomLevel)
        {
            switch (Coords.V)
            {
                case Vertice.Superior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * Coords.Q + Math.Sqrt(3) / 2 * Coords.R) - BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * Coords.R - 1) - BHALFSIDE) / zoomLevel);
                    break;
                case Vertice.Inferior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * Coords.Q + Math.Sqrt(3) / 2 * Coords.R) - BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * Coords.R + 1) - BHALFSIDE) / zoomLevel);
                    break;
            }
            return basePoint;
        }
    }

    public sealed class FichaPoblado : FichaVertice
    {
        public static readonly Bitmap Bitmap = new Bitmap(cliente.Properties.Resources.PobladoBmp);
        public FichaPoblado(int q, int r, Vertice v, ColorJugador color) : base(q, r, v, color)
        {
        }
    }

    public sealed class FichaCiudad : FichaVertice
    {
        public static readonly Bitmap Bitmap = new Bitmap(cliente.Properties.Resources.CiudadBmp);
        public FichaCiudad(int q, int r, Vertice v, ColorJugador color) : base(q, r, v, color)
        {
        }
    }
}
