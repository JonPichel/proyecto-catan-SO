using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class PanelInfoJugador : UserControl
    {
      
        public PanelInfoJugador()
        {
            InitializeComponent();
        }
  
        int caballeros, carreteras, recursos, desarrollo, puntos;
        int madera, ladrillo, oveja, trigo, piedra;
        ColorJugador color;

        public string Nombre
        {
            get => this.lblNombre.Text;
            set
            {
                this.lblNombre.Text = value;
            }
        }
        public ColorJugador ColorJ
        {
            get => this.color;
            set
            {
                this.color = value;
                this.panelColor.BackColor = Colores.DameColor(value);
            }
        }
        public int Caballeros
        {
            get => this.caballeros;
            set
            {
                this.caballeros = value;
                this.lblCaballeros.Text = value.ToString();
            }
        }
        public int Carreteras
        {
            get => this.carreteras;
            set
            {
                this.carreteras = value;
                this.lblCarreteras.Text = value.ToString();
            }
        }

        public int Recursos
        {
            get => this.recursos;
            set
            {
                this.recursos = value;
                this.lblRecursos.Text = value.ToString();
            }
        }

        public int Desarrollo
        {
            get => this.desarrollo;
            set
            {
                this.desarrollo = value;
                this.lblDesarrollo.Text = value.ToString();
            }
        }
        public int Puntos
        {
            get => this.puntos;
            set
            {
                this.puntos = value;
                this.lblPuntos.Text = value.ToString();
            }
        }
        public int Madera
        {
            get => this.madera;
            set
            {
                this.madera = value;
                if(this.madera == 0)
                {
                    this.lblMadera.Visible = false;
                }
                else if (this.madera > 0)
                {
                    this.lblMadera.Visible = true;
                    this.lblMadera.BackColor = Color.Green;
                    this.lblMadera.Text = "+" + value.ToString();
                }
                else
                {
                    this.lblMadera.Visible = true;
                    this.lblMadera.BackColor = Color.Red;
                    this.lblMadera.Text = "-" + value.ToString();
                }
            }
        }
        public int Ladrillo
        {
            get => this.ladrillo;
            set
            {
                this.ladrillo = value;
                if (this.ladrillo == 0)
                {
                    this.lblLadrillo.Visible = false;
                }
                else if (this.ladrillo > 0)
                {
                    this.lblLadrillo.Visible = true;
                    this.lblLadrillo.BackColor = Color.Green;
                    this.lblLadrillo.Text = "+ " + value.ToString();
                }
                else
                {
                    this.lblLadrillo.Visible = true;
                    this.lblLadrillo.BackColor = Color.Red;
                    this.lblLadrillo.Text = "- " + value.ToString();
                }
            }
        }
        public int Oveja
        {
            get => this.oveja;
            set
            {
                this.oveja = value;
                if (this.oveja == 0)
                {
                    this.lblOveja.Visible = false;
                }
                else if (this.oveja > 0)
                {
                    this.lblOveja.Visible = true;
                    this.lblOveja.BackColor = Color.Green;
                    this.lblOveja.Text = "+ " + value.ToString();
                }
                else
                {
                    this.lblOveja.Visible = true;
                    this.lblOveja.BackColor = Color.Red;
                    this.lblOveja.Text = "- " + value.ToString();
                }
            }
        }
        public int Trigo
        {
            get => this.trigo;
            set
            {
                this.trigo = value;
                if (this.trigo == 0)
                {
                    this.lblTrigo.Visible = false;
                }
                else if (this.trigo > 0)
                {
                    this.lblTrigo.Visible = true;
                    this.lblTrigo.BackColor = Color.Green;
                    this.lblTrigo.Text = "+ " + value.ToString();
                }
                else
                {
                    this.lblTrigo.Visible = true;
                    this.lblTrigo.BackColor = Color.Red;
                    this.lblTrigo.Text = "- " + value.ToString();
                }
            }
        }
        public int Piedra
        {
            get => this.piedra;
            set
            {
                this.piedra = value;
                if (this.piedra == 0)
                {
                    this.lblPiedra.Visible = false;
                }
                else if (this.piedra > 0)
                {
                    this.lblPiedra.Visible = true;
                    this.lblPiedra.BackColor = Color.Green;
                    this.lblPiedra.Text = "+ " + value.ToString();
                }
                else
                {
                    this.lblPiedra.Visible = true;
                    this.lblPiedra.BackColor = Color.Red;
                    this.lblPiedra.Text = "- " + value.ToString();
                }
            }
        }



    }
}
