using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class Carta : Button
    {
        public enum TipoCarta
        {
            Monopolio,
            Invento,
            Caballero,
            Carreteras,
            Punto
        }

        public TipoCarta Tipo;

        public Carta(TipoCarta tipo)
        {
            InitializeComponent();
            this.Tipo = tipo;
            this.AutoSize = false;
            this.Size = cliente.Properties.Resources.CartaMonopolio.Size;
            this.Enabled = true;
            switch (this.Tipo)
            {
                case TipoCarta.Monopolio:
                    this.Image = cliente.Properties.Resources.CartaMonopolio;
                    break;
                case TipoCarta.Invento:
                    this.Image = cliente.Properties.Resources.CartaInvento;
                    break;
                case TipoCarta.Caballero:
                    this.Image = cliente.Properties.Resources.CartaCaballero;
                    break;
                case TipoCarta.Carreteras:
                    this.Image = cliente.Properties.Resources.CartaCarreteras;
                    break;
                case TipoCarta.Punto:
                    this.Image = cliente.Properties.Resources.CartaPunto;
                    break;
            }
        }
    }
}
