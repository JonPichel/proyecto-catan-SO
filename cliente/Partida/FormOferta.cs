using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Linq;

namespace cliente.Partida
{
    public partial class FormOferta : Form
    {
        //Informacion partida necesaria
        string nombre;
        string turno;
        ColorJugador colorturno;
        Socket conn;
        int idP;

        //Recursos en tu poder
        int[] recursos;
        string oferta;

        Label[] lblsO;          //lbls ofrecer
        Label[] lblsP;          //lbls pedir

        public FormOferta(Socket conn, int idP, string nombre, ColorJugador colorturno, string oferta,
            int madera, int ladrillo, int oveja, int trigo, int piedra)
        {
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
            this.colorturno = colorturno;
            this.recursos = new int[]
            {
                madera,
                ladrillo,
                oveja,
                trigo,
                piedra
            };

            this.oferta = oferta;
            InitializeComponent();
        }

        private void Comerciar_Load(object sender, EventArgs e)
        {
            //labels informando de recursos disponibles para comerciar
            lblMadera.Text = "(" + Convert.ToString(recursos[0]) + ")";
            lblLadrillo.Text = "(" + Convert.ToString(recursos[1]) + ")";
            lblOveja.Text = "(" + Convert.ToString(recursos[2]) + ")";
            lblTrigo.Text = "(" + Convert.ToString(recursos[3]) + ")";
            lblPiedra.Text = "(" + Convert.ToString(recursos[4]) + ")";

            lblsO = new Label[] { lblMaderaO, lblLadrilloO, lblOvejaO, lblTrigoO, lblPiedraO };
            lblsP = new Label[] { lblMaderaP, lblLadrilloP, lblOvejaP, lblTrigoP, lblPiedraP };
            lblOfertaNP.Visible = false;

            string[] trozos = oferta.Split("/")[2].Split(",");
            for (int i = 0; i < 5; i++)
            {
                lblsO[i].Text = trozos[i];
            }
            for (int i = 5; i < 10; i++)
            {
                if (recursos[i-5] < Convert.ToInt32(trozos[i]))
                {
                    btnAceptar.Enabled = false;
                    lblOfertaNP.Visible = true;
                }
                lblsP[i-5].Text = trozos[i];
            }

            switch (colorturno)
            {
                case ColorJugador.Azul:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorAzul;
                    break;
                case ColorJugador.Rojo:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorRojo;
                    break;
                case ColorJugador.Naranja:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorNaranja;
                    break;
                case ColorJugador.Gris:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorGris;
                    break;
                case ColorJugador.Morado:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorMorado;
                    break;
                case ColorJugador.Verde:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorVerde;
                    break;
                default:
                    pboxTurno.Image = cliente.Properties.Resources.JugadorAzul;
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pet = "28/" + this.idP + "/" + this.nombre + "/" + "SI";
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);

            this.Close();
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            string pet = "28/" + this.idP + "/" + this.nombre + "/" + "NO";
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);

            this.Close();
        }
    }
}
