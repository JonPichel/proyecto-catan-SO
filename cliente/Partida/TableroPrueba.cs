using System;
using System.Collections.Generic;
using System.Text;

namespace cliente.Partida
{
    static class TableroPrueba {
        public static Tile[] GetTiles()
        {
            return new Tile[]
            {
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
                //new FichaPoblado(1, -1, Vertice.Superior, ColorJugador.Rojo),
               // new FichaCiudad(-1, 1, Vertice.Inferior, ColorJugador.Azul)
            };
        }

        public static List<Carretera> GetCarreteras()
        {
            return new List<Carretera>()
            {
               // new Carretera(2, 0, Lado.Norte, ColorJugador.Rojo),
               // new Carretera(-1, 1, Lado.Oeste, ColorJugador.Rojo),
               // new Carretera(1, -1, Lado.Sur, ColorJugador.Azul)
            };
        }
    }
}
