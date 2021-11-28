using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class TabPartida : UserControl
    {

        public virtual ColorJugador colorJugador { get; set; }

        public TabPartida()
        {
            InitializeComponent();
        }
    }
}
