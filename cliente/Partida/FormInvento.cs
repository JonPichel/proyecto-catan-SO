using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class FormInvento : Form
    {
        RadioButton[] btnsR1;
        RadioButton[] btnsR2;
        public string[] recursos;

        public FormInvento()
        {
            InitializeComponent();
        }

        private void FormInvento_Load(object sender, EventArgs e)
        {
            // Preparar valores de selección por defecto
            btnsR1 = new RadioButton[] { radiobtnMadera1, radiobtnLadrillo1, radiobtnOveja1, radiobtnTrigo1, radiobtnPiedra1 };
            btnsR2 = new RadioButton[] { radiobtnMadera2, radiobtnLadrillo2, radiobtnOveja2, radiobtnTrigo2, radiobtnPiedra2 };
            recursos = new string[] { "", "" };

            foreach (RadioButton btn in btnsR1)
            {
                btn.Checked = false;
                btn.Click += this.radiobtnRecurso1_CheckedChanged;
            }
            foreach (RadioButton btn in btnsR2)
            {
                btn.Checked = false;
                btn.Click += this.radiobtnRecurso2_CheckedChanged;
            }
            btnEscoger.Enabled = false;
        }

        private void btnEscoger_Click(object sender, EventArgs e)
        {
            // Si no se han elegido 2 recursos no se puede aceptar
            if ((recursos[0] != "") && (recursos[1] != ""))
                Close();
        }

        private void radiobtnRecurso1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton recurso = (RadioButton)sender;

            // Comprobar el primer recurso escogido
            switch (recurso.Name)
            {
                case "radiobtnMadera1":
                    this.recursos[0] = "Madera";
                    break;
                case "radiobtnLadrillo1":
                    this.recursos[0] = "Ladrillo";
                    break;
                case "radiobtnOveja1":
                    this.recursos[0] = "Oveja";
                    break;
                case "radiobtnTrigo1":
                    this.recursos[0] = "Trigo";
                    break;
                case "radiobtnPiedra1":
                    this.recursos[0] = "Piedra";
                    break;
            }
            if (this.recursos[1] != "")
                btnEscoger.Enabled = true;
        }

        private void radiobtnRecurso2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton recurso = (RadioButton)sender;

            // Comprobar el segundo recurso escogido
            switch (recurso.Name)
            {
                case "radiobtnMadera2":
                    this.recursos[1] = "Madera";
                    break;
                case "radiobtnLadrillo2":
                    this.recursos[1] = "Ladrillo";
                    break;
                case "radiobtnOveja2":
                    this.recursos[1] = "Oveja";
                    break;
                case "radiobtnTrigo2":
                    this.recursos[1] = "Trigo";
                    break;
                case "radiobtnPiedra2":
                    this.recursos[1] = "Piedra";
                    break;
            }
            if (this.recursos[0] != "")
                btnEscoger.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // No se escoge ningún recurso
            recursos[0] = "";
            recursos[1] = "";
            Close();
        }
    }
}
