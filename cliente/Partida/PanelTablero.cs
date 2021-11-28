using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class PanelTablero : UserControl
    {
        public PanelTablero()
        {
            InitializeComponent();
        }

        private void PanelTablero_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }
    }
}
