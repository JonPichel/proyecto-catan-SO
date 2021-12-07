using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente
{
    public partial class Monopolio : Form
    {
        public Monopolio()
        {
            InitializeComponent();
        }

        public string Recurso = "";
        private void Monopolio_Load(object sender, EventArgs e)
        {
            RadioButton[] btns = new RadioButton[] { radiobtnMadera, radiobtnLadrillo, radiobtnOveja, radiobtnTrigo, radiobtnPiedra };
            foreach (RadioButton btn in btns)
            {
                btn.Checked = false;
                btn.Click += this.radiobtnRecurso_CheckedChanged;
            }
            btnEscoger.Enabled = false;
        }

        private void btnEscoger_Click(object sender, EventArgs e)
        {
            if (Recurso != "")
                Close();
        }

        private void radiobtnRecurso_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton recurso = (RadioButton)sender;
            switch (recurso.Name)
            {
                case "radiobtnMadera":
                    Recurso = "Madera";
                    break;
                case "radiobtnLadrillo":
                    Recurso = "Ladrillo";
                    break;
                case "radiobtnOveja":
                    Recurso = "Oveja";
                    break;
                case "radiobtnTrigo":
                    Recurso = "Trigo";
                    break;
                case "radiobtnPiedra":
                    Recurso = "Piedra";
                    break;
            }
            btnEscoger.Enabled = true;
        }
   
    }
}
