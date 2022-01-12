using System;
using System.Drawing;
using System.Windows.Forms;

namespace cliente.Menu
{
    public partial class FormManual : Form
    {
        Panel[] paneles;

        public FormManual()
        {
            InitializeComponent();
        }

        private void FormManual_Load(object sender, EventArgs e)
        {
            this.paneles = new Panel[]{ pnlCartas, pnlLadron, pnlPuntos, pnlRecursos, pnlComercio };
            PanelShower(pnlCartas);
        }

        public void PanelShower(Panel panel)
        {
            for (int i = 0; i < paneles.Length; i++)
            {
                if (paneles[i] == panel)
                {
                    panel.Visible = true;
                    panel.Location = new Point(156, 44);
                    panel.BringToFront();
                }
                else
                {
                    paneles[i].Visible = false;
                }
                
            }
        }

        private void btnCartas_Click(object sender, EventArgs e)
        {
            PanelShower(pnlCartas);
        }

        private void btnLadron_Click(object sender, EventArgs e)
        {
            PanelShower(pnlLadron);
        }

        private void btnPuntos_Click(object sender, EventArgs e)
        {
           PanelShower(pnlPuntos);
        }

        private void bntRecursos_Click(object sender, EventArgs e)
        {
            PanelShower(pnlRecursos);
        }

        private void btnComercio_Click(object sender, EventArgs e)
        {
            PanelShower(pnlComercio);
        }
    }
}
