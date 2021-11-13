﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace cliente.Partida
{
    public enum ColorJugador
    {
        Azul,
        Rojo,
        Naranja,
        Gris,
        Morado,
        Verde
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

        public LadoCoords[] LadosVecinos()
        {
            LadoCoords[] vecinos = new LadoCoords[3];

            switch (V)
            {
                case Vertice.Superior:
                    vecinos[0] = new LadoCoords(Q + 1, R - 1, Lado.Oeste);  // Arriba
                    vecinos[1] = new LadoCoords(Q + 1, R - 1, Lado.Sur);    // Derecha
                    vecinos[2] = new LadoCoords(Q, R, Lado.Norte);          // Izquierda
                    break;
                case Vertice.Inferior:
                    vecinos[0] = new LadoCoords(Q, R + 1, Lado.Oeste);      // Abajo
                    vecinos[1] = new LadoCoords(Q, R + 1, Lado.Norte);      // Derecha
                    vecinos[2] = new LadoCoords(Q, R, Lado.Sur);            // Izquierda
                    break;
            }
            return vecinos;
        }

        public VerticeCoords[] VerticesHex()
        {
            VerticeCoords[] vertices = new VerticeCoords[6];

            // Empezamos a contar desde el vértice superior hacia la derecha
            vertices[0] = new VerticeCoords(Q, R, Vertice.Superior);            // N
            vertices[1] = new VerticeCoords(Q + 1, R - 1, Vertice.Inferior);    // NE
            vertices[2] = new VerticeCoords(Q, R + 1, Vertice.Superior);        // SE
            vertices[3] = new VerticeCoords(Q, R, Vertice.Inferior);            // S
            vertices[4] = new VerticeCoords(Q - 1, R + 1, Vertice.Superior);    // SO
            vertices[5] = new VerticeCoords(Q, R - 1, Vertice.Inferior);        // NO

            return vertices;
        }

        public Point VerticeToPixel(Point basePoint, int zoomLevel)
        {
            switch (V)
            {
                case Vertice.Superior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * R) - FichaVertice.BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * R - 1) - FichaVertice.BHALFSIDE) / zoomLevel);
                    break;
                case Vertice.Inferior:
                    basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * R) - FichaVertice.BHALFSIDE) / zoomLevel);
                    basePoint.Y += (int)((Tile.BRADIUS * (1.5 * R + 1) - FichaVertice.BHALFSIDE) / zoomLevel);
                    break;
            }
            return basePoint;
        }

        public override String ToString()
        {
            return String.Format("VerticeCoords({0}, {1}, {2})", Q, R, V);
        }
    }

    public class FichaVertice
    {
        public const int BHALFSIDE = 162 / 2;

        public VerticeCoords Coords;
        public ColorJugador Color;
        virtual public Bitmap Bitmap { get => cliente.Properties.Resources.PobladoAzul; }

        public FichaVertice(int q, int r, Vertice v, ColorJugador color)
        {
            this.Coords.HexCoords = new HexCoords(q, r); 
            this.Coords.V = v;
            this.Color = color;
        }

        public Point VerticeToPixel(Point basePoint, int zoomLevel)
        {
            return Coords.VerticeToPixel(basePoint, zoomLevel);
        }
    }

    public sealed class FichaPoblado : FichaVertice
    {
        override public Bitmap Bitmap
        {
            get
            {
                switch (Color)
                {
                    case ColorJugador.Azul:
                        return cliente.Properties.Resources.PobladoAzul;
                    case ColorJugador.Rojo:
                        return cliente.Properties.Resources.PobladoRojo;
                    case ColorJugador.Naranja:
                        return cliente.Properties.Resources.PobladoNaranja;
                    case ColorJugador.Gris:
                        return cliente.Properties.Resources.PobladoGris;
                    case ColorJugador.Morado:
                        return cliente.Properties.Resources.PobladoMorado;
                    case ColorJugador.Verde:
                        return cliente.Properties.Resources.PobladoVerde;
                    default:
                        return cliente.Properties.Resources.PobladoAzul;
                }
            }
        }
        public FichaPoblado(int q, int r, Vertice v, ColorJugador color) : base(q, r, v, color)
        {
        }
    }

    public sealed class FichaCiudad : FichaVertice
    {
        override public Bitmap Bitmap
        {
            get
            {
                switch (Color)
                {
                    case ColorJugador.Azul:
                        return cliente.Properties.Resources.CiudadAzul;
                    case ColorJugador.Rojo:
                        return cliente.Properties.Resources.CiudadRojo;
                    case ColorJugador.Naranja:
                        return cliente.Properties.Resources.CiudadNaranja;
                    case ColorJugador.Gris:
                        return cliente.Properties.Resources.CiudadGris;
                    case ColorJugador.Morado:
                        return cliente.Properties.Resources.CiudadMorado;
                    case ColorJugador.Verde:
                        return cliente.Properties.Resources.CiudadVerde;
                    default:
                        return cliente.Properties.Resources.CiudadAzul;
                }
            }
        }

        public FichaCiudad(int q, int r, Vertice v, ColorJugador color) : base(q, r, v, color)
        {
        }
    }

    public enum Lado
    {
        Norte,
        Oeste,
        Sur
    }
    public struct LadoCoords
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
        public Lado L;

        public LadoCoords(int q, int r, Lado l)
        {
            this.HexCoords = new HexCoords(q, r);
            this.L = l;
        }

        public LadoCoords(HexCoords hexCoords, Lado l)
        {
            this.HexCoords = hexCoords;
            this.L = l;
        }

        public VerticeCoords[] VerticesExtremos()
        {
            VerticeCoords[] extremos = new VerticeCoords[2];

            switch (L)
            {
                case Lado.Norte:
                    extremos[0] = new VerticeCoords(Q, R, Vertice.Superior);
                    extremos[1] = new VerticeCoords(Q, R - 1, Vertice.Inferior);
                    break;
                case Lado.Oeste:
                    extremos[0] = new VerticeCoords(Q, R - 1, Vertice.Inferior);
                    extremos[1] = new VerticeCoords(Q - 1, R + 1, Vertice.Superior);
                    break;
                case Lado.Sur:
                    extremos[0] = new VerticeCoords(Q - 1, R + 1, Vertice.Superior);
                    extremos[1] = new VerticeCoords(Q, R, Vertice.Inferior);
                    break;
            }

            return extremos;
        }

        public LadoCoords[] LadosVecinos()
        {
            LadoCoords[] vecinos = new LadoCoords[4];
            switch (L)
            {
                case Lado.Norte:
                    vecinos[0] = new LadoCoords(Q + 1, R - 1, Lado.Oeste);  // Derecha arriba
                    vecinos[1] = new LadoCoords(Q + 1, R - 1, Lado.Sur);    // Derecha abajo
                    vecinos[2] = new LadoCoords(Q, R - 1, Lado.Sur);        // Izquierda arriba
                    vecinos[3] = new LadoCoords(Q, R, Lado.Oeste);          // Izquierda abajo
                    break;
                case Lado.Oeste:
                    vecinos[0] = new LadoCoords(Q, R, Lado.Norte);            // Derecha arriba
                    vecinos[1] = new LadoCoords(Q, R, Lado.Sur);              // Derecha abajo
                    vecinos[2] = new LadoCoords(Q - 1, R, Lado.Norte);        // Izquierda arriba
                    vecinos[3] = new LadoCoords(Q - 1, R, Lado.Sur);          // Izquierda abajo
                    break;
                case Lado.Sur:
                    vecinos[0] = new LadoCoords(Q, R + 1, Lado.Norte);        // Derecha arriba
                    vecinos[1] = new LadoCoords(Q, R + 1, Lado.Oeste);        // Derecha abajo
                    vecinos[2] = new LadoCoords(Q, R, Lado.Oeste);            // Izquierda arriba
                    vecinos[3] = new LadoCoords(Q - 1, R + 1, Lado.Norte);    // Izquierda abajo
                    break;
            }
            return vecinos;
        }

        public static LadoCoords PixelToLado(Point pixelCoords, Point basePoint, int zoomLevel)
        {
            HexCoords tile = HexCoords.PixelToHex(pixelCoords, basePoint, zoomLevel);
            Point tileCenter = tile.HexToPixel(basePoint, zoomLevel);
            tileCenter.X += Tile.BWIDTH / 2 / zoomLevel;
            tileCenter.Y += Tile.BHEIGHT / 2 / zoomLevel;
            double angle = Math.Atan2(tileCenter.X - pixelCoords.X, tileCenter.Y - pixelCoords.Y);
            if (angle <= -2.0/3 * Math.PI)
            {
                return new LadoCoords(tile.Q, tile.R + 1, Lado.Norte);
            } else if (angle <= -Math.PI / 3)
            {
                return new LadoCoords(tile.Q + 1, tile.R, Lado.Oeste);
            } else if (angle <= 0)
            {
                return new LadoCoords(tile.Q + 1, tile.R - 1, Lado.Sur);
            } else if (angle <= Math.PI / 3)
            {
                return new LadoCoords(tile, Lado.Norte);
            } else if (angle <= 2.0/3 * Math.PI)
            {
                return new LadoCoords(tile, Lado.Oeste);
            } else
            {
                return new LadoCoords(tile, Lado.Sur);
            }
        }

        public Point LadoToPixel(Point basePoint, int zoomLevel, int dx, int dy)
        {
            basePoint.X += (int)((Tile.BRADIUS * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * (R - 1)) - dx) / zoomLevel);
            basePoint.Y += (int)((Tile.BRADIUS * (1.5 * R - 1) - dy) / zoomLevel);
            return basePoint;
        }

        public override String ToString()
        {
            return String.Format("LadoCoords({0}, {1}, {2})", Q, R, L);
        }
    }

    public class Carretera
    {
        public const int DX = 40;
        public const int DY = 20;

        public LadoCoords Coords;
        public ColorJugador Color;
        public Bitmap Bitmap
        {
            get
            {
                switch (Coords.L)
                {
                    case Lado.Norte when Color == ColorJugador.Azul:
                        return cliente.Properties.Resources.CarreteraNorteAzul;
                    case Lado.Norte when Color == ColorJugador.Rojo:
                        return cliente.Properties.Resources.CarreteraNorteRojo;
                    case Lado.Norte when Color == ColorJugador.Naranja:
                        return cliente.Properties.Resources.CarreteraNorteNaranja;
                    case Lado.Norte when Color == ColorJugador.Gris:
                        return cliente.Properties.Resources.CarreteraNorteGris;
                    case Lado.Norte when Color == ColorJugador.Morado:
                        return cliente.Properties.Resources.CarreteraNorteMorado;
                    case Lado.Norte when Color == ColorJugador.Verde:
                        return cliente.Properties.Resources.CarreteraNorteVerde;
                    case Lado.Oeste when Color == ColorJugador.Azul:
                        return cliente.Properties.Resources.CarreteraOesteAzul;
                    case Lado.Oeste when Color == ColorJugador.Rojo:
                        return cliente.Properties.Resources.CarreteraOesteRojo;
                    case Lado.Oeste when Color == ColorJugador.Naranja:
                        return cliente.Properties.Resources.CarreteraOesteNaranja;
                    case Lado.Oeste when Color == ColorJugador.Gris:
                        return cliente.Properties.Resources.CarreteraOesteGris;
                    case Lado.Oeste when Color == ColorJugador.Morado:
                        return cliente.Properties.Resources.CarreteraOesteMorado;
                    case Lado.Oeste when Color == ColorJugador.Verde:
                        return cliente.Properties.Resources.CarreteraOesteVerde;
                    case Lado.Sur when Color == ColorJugador.Azul:
                        return cliente.Properties.Resources.CarreteraSurAzul;
                    case Lado.Sur when Color == ColorJugador.Rojo:
                        return cliente.Properties.Resources.CarreteraSurRojo;
                    case Lado.Sur when Color == ColorJugador.Naranja:
                        return cliente.Properties.Resources.CarreteraSurNaranja;
                    case Lado.Sur when Color == ColorJugador.Gris:
                        return cliente.Properties.Resources.CarreteraSurGris;
                    case Lado.Sur when Color == ColorJugador.Morado:
                        return cliente.Properties.Resources.CarreteraSurMorado;
                    case Lado.Sur when Color == ColorJugador.Verde:
                        return cliente.Properties.Resources.CarreteraSurVerde;
                    default:
                        return cliente.Properties.Resources.CarreteraNorteAzul;
                }
            }
        }

        public Carretera(int q, int r, Lado l, ColorJugador color)
        {
            this.Coords.HexCoords = new HexCoords(q, r);
            this.Coords.L = l;
            this.Color = color;
        }

        public Point LadoToPixel(Point basePoint, int zoomLevel)
        {
            return Coords.LadoToPixel(basePoint, zoomLevel, DX, DY);
        }
    }

}
