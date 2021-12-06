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

        int caballeros, carreteras, recursos, desarrollo, puntos;
        ColorJugador color;
        public string Nombre
        {
            get => this.lblNombre.Text;
            set
            {
                this.lblNombre.Text = value;
            }
        }
        public ColorJugador Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.BackColor = Colores.DameColor(value);
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

        public PanelInfoJugador()
        {
            InitializeComponent();
        }
    }
}
