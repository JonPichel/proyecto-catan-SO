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

    public class FichaVertice
    {
        public const int BHALFSIDE = 130 / 2;

        public readonly int q, r;
        public enum Vertice
        {
            Superior,
            Inferior
        };
        public readonly Vertice v;
        public ColorJugador Color;

        public FichaVertice(int q, int r, Vertice v, ColorJugador color)
        {
            this.q = q;
            this.r = r;
            this.v = v;
            this.Color = color;
        }

        public Point PixelCoords(Point basePoint, int zoomLevel)
        {
            switch (v)
            {
                case Vertice.Superior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * this.q + Math.Sqrt(3) / 2 * this.r) - BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * this.r - 1) - BHALFSIDE) / zoomLevel);
                    break;
                case Vertice.Inferior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * this.q + Math.Sqrt(3) / 2 * this.r) - BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * this.r + 1) - BHALFSIDE) / zoomLevel);
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
