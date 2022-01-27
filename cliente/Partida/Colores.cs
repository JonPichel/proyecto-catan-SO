using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace cliente.Partida
{
    static class Colores
    {
        // Colores
        public static Color DameColor(ColorJugador color)
        {
            switch (color)
            {
                case ColorJugador.Azul:
                    return Color.FromArgb(95, 171, 200);
                case ColorJugador.Rojo:
                    return Color.FromArgb(160, 44, 44);
                case ColorJugador.Naranja:
                    return Color.FromArgb(225, 132, 13);
                case ColorJugador.Gris:
                    return Color.FromArgb(200, 190, 183);
                case ColorJugador.Morado:
                    return Color.FromArgb(178, 95, 211);
                case ColorJugador.Verde:
                    return Color.FromArgb(111, 145, 111);
                default:
                    return Color.FromArgb(95, 171, 200);
            }
        }

        // Colores de jugador
        public static ColorJugador DameColorJugador(Color color)
        {
            if (color == Color.FromArgb(111, 145, 111))
            {
                return ColorJugador.Verde;
            }
            else if (color == Color.FromArgb(178, 95, 211))
            {
                return ColorJugador.Morado;
            }
            else if (color == Color.FromArgb(200, 190, 183))
            {
                return ColorJugador.Gris;
            }
            else if (color == Color.FromArgb(225, 132, 13))
            {
                return ColorJugador.Naranja;
            }
            else if (color == Color.FromArgb(160, 44, 44))
            {
                return ColorJugador.Rojo;
            }
            else if (color == Color.FromArgb(95, 171, 200))
            {
                return ColorJugador.Azul;
            }
            else
                return ColorJugador.Gris;
        }

    }
}
