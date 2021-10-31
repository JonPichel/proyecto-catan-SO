using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class FormPartida : Form
    {
        List<Tab> tabs = new List<Tab>();

        public FormPartida()
        {
            InitializeComponent();
        }

        private void FormPartida_Load(object sender, EventArgs e)
        {
            // Preparar pantallas
            tabs.Add(new TabTablero());

            foreach (Tab tab in this.tabs)
            {
                tab.Hide();
                tab.Location = new Point(0, 0);
                this.Controls.Add(tab);
            }
            // tabs[0].VisibleChanged += this.tabLogin_VisibleChanged;
            tabs[0].Show();
        }
    }
}
