using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Net.Sockets;
using System.Resources;

namespace cliente.Partida
{
    public partial class TabPartidaGanada : TabPartida
    {

        Label[] lblsPuntos;        
        Label[] lblsNombres;
        PictureBox[] pboxsColores;
        PictureBox[] pboxs;
        public string[] nombres;
        public ColorJugador[] colores;

        int[] puntos;

        public TabPartidaGanada()
        {
            InitializeComponent();
        }

        private void TabPartidaGanada_Load(object sender, EventArgs e)
        {
            //Definimos listas de elementos
            lblsPuntos = new Label[] { lblNum1, lblNum2, lblNum3, lblNum4 };
            lblsNombres = new Label[] { lbl1, lbl2, lbl3, lbl4 };
            pboxsColores = new PictureBox[] { pboxC1, pboxC2, pboxC3, pboxC4 };
            pboxs = new PictureBox[] { pbox1, pbox2, pbox3, pbox4 };
            puntos = new int[] { 0, 0, 0, 0 };

            //Entramos en el form con todo oculto
            foreach(Label lbl in lblsPuntos)
                lbl.Visible = false;
            foreach (Label lbl in lblsNombres)
                lbl.Visible = false;
            foreach (PictureBox pbox in pboxsColores)
                pbox.Visible = false;
            foreach (PictureBox pbox in pboxs)
                pbox.Visible = false;
        }

        public void ActualizarRanking(string mensaje)
        {
            string[] trozos = mensaje.Split(',');
            int Puntos = Convert.ToInt32(trozos[1]);
            string Nombre = trozos[0];
            Bitmap Figura = cliente.Properties.Resources.JugadorAzul;

            int i = 0;
            while (i < colores.Length)
            {
                if (Nombre == nombres[i])
                {
                    switch (colores[i])
                    {

                        case ColorJugador.Azul:
                            Figura = cliente.Properties.Resources.JugadorAzul;
                            break;
                        case ColorJugador.Rojo:
                            Figura = cliente.Properties.Resources.JugadorRojo;
                            break;
                        case ColorJugador.Naranja:
                            Figura = cliente.Properties.Resources.JugadorNaranja;
                            break;
                        case ColorJugador.Gris:
                            Figura = cliente.Properties.Resources.JugadorGris;
                            break;
                        case ColorJugador.Morado:
                            Figura = cliente.Properties.Resources.JugadorMorado;
                            break;
                        case ColorJugador.Verde:
                            Figura = cliente.Properties.Resources.JugadorVerde;
                            break;
                        default:
                            Figura = cliente.Properties.Resources.JugadorAzul;
                            break;
                    }
                }
                i++;
            }

            i = 0;
            while (i < puntos.Length)
            {
                if (Puntos > puntos[i] && puntos[i] == 0)
                {
                    puntos[i] = Puntos;
                    lblsPuntos[i].Text = Convert.ToString(Puntos);
                    lblsPuntos[i].Visible = true;
                    lblsNombres[i].Text = Nombre;
                    lblsNombres[i].Visible = true;
                    pboxsColores[i].Image = Figura;
                    pboxsColores[i].Visible = true;
                    pboxs[i].Visible = true;
                    break;
                }
                else if (Puntos > puntos[i] && puntos[i] != 0)
                {
                    int a = 0;
                    foreach (int punto in puntos)
                    {
                        if (punto == 0)
                            break;
                        a++;
                    }
                    while (a > i)
                    {
                        if (puntos[a] == 0)
                        {
                            lblsPuntos[a].Visible = true;
                            lblsNombres[a].Visible = true;
                            pboxsColores[a].Visible = true;
                            pboxs[a].Visible = true;
                        }
                        puntos[a] = puntos[a - 1];
                        lblsPuntos[a].Text = lblsPuntos[a - 1].Text;
                        lblsNombres[a].Text = lblsNombres[a - 1].Text;
                        pboxsColores[a].Image = pboxsColores[a - 1].Image;
                        a--;
                    }
                    puntos[i] = Puntos;
                    lblsPuntos[i].Text = Convert.ToString(Puntos);
                    lblsPuntos[i].Visible = true;
                    lblsNombres[i].Text = Nombre;
                    lblsNombres[i].Visible = true;
                    pboxsColores[i].Image = Figura;
                    pboxsColores[i].Visible = true;
                    pboxs[i].Visible = true;
                    break;
                }
                i++;
            }
        }
    }
}
