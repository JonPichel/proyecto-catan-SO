using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente
{
    public partial class FormMonopolio : Form
    {
        public FormMonopolio()
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
            // Si no se escoge un recurso no se puede aceptar
            if (Recurso != "")
                Close();
        }

        private void radiobtnRecurso_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton recurso = (RadioButton)sender;

            // Comprobar que recurso se escoge
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Si se cancela no se escoge ningún recurso
            Recurso = "";
            Close();
        }
    }
}
