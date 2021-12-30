using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Partida
{
    public partial class FormComerciar : Form
    {
        //Recursos en tu poder
        int madera, ladrillo, oveja, trigo, piedra;

        int Comercio = 0; // 0: comercio con otros jugadores, 1: comercio marítimo  

        public FormComerciar(int Madera, int Ladrillo, int Oveja, int Trigo, int Piedra)
        {
            InitializeComponent();
            this.madera = Madera;
            this.ladrillo = Ladrillo;
            this.oveja = Oveja;
            this.trigo = Trigo;
            this.piedra = Piedra;
        }

        private void Comerciar_Load(object sender, EventArgs e)
        {
            //labels informando de recursos disponibles para comerciar
            lblMadera.Text = "(" + Convert.ToString(madera) + ")";
            lblLadrillo.Text = "(" + Convert.ToString(ladrillo) + ")";
            lblOveja.Text = "(" + Convert.ToString(oveja) + ")";
            lblTrigo.Text = "(" + Convert.ToString(trigo) + ")";
            lblPiedra.Text = "(" + Convert.ToString(piedra) + ")";

            //Solo se activan los botones de aquellos recursos disponibles
            if (madera > 0)
                btnMaderaO.Enabled = true;
            if (ladrillo > 0)
                btnLadrilloO.Enabled = true;
            if (oveja > 0)
                btnOvejaO.Enabled = true;
            if (trigo > 0)
                btnTrigoO.Enabled = true;
            if (piedra > 0)
                btnPiedraO.Enabled = true;
        }

        private void btnComercioJ_Click(object sender, EventArgs e)
        {
            //Comercio interno con otros jugadores

            btnComercioJ.BackColor = Color.Linen;
            btnComercioM.BackColor = Color.White;
            this.Comercio = 0;
        }

        private void btnComercioM_Click(object sender, EventArgs e)
        {
            //Comercio marítimo

            btnComercioM.BackColor = Color.Linen;
            btnComercioJ.BackColor = Color.White;
            this.Comercio = 1;
        }
        private void btnComerciar_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnMaderaP_Click(object sender, EventArgs e)
        {
            lblMaderaP.Text = Convert.ToString(Convert.ToInt32(lblMaderaP.Text) + 1);
        }

        private void btnLadrilloP_Click(object sender, EventArgs e)
        {
            lblLadrilloP.Text = Convert.ToString(Convert.ToInt32(lblLadrilloP.Text) + 1);
        }

        private void btnOvejaP_Click(object sender, EventArgs e)
        {
            lblOvejaP.Text = Convert.ToString(Convert.ToInt32(lblOvejaP.Text) + 1);
        }

        private void btnTrigoP_Click(object sender, EventArgs e)
        {
            lblTrigoP.Text = Convert.ToString(Convert.ToInt32(lblTrigoP.Text) + 1);
        }

        private void btnPiedraP_Click(object sender, EventArgs e)
        {
            lblPiedraP.Text = Convert.ToString(Convert.ToInt32(lblPiedraP.Text) + 1);
        }

        private void btnMaderaO_Click(object sender, EventArgs e)
        {
            lblMaderaO.Text = Convert.ToString(Convert.ToInt32(lblMaderaO.Text) + 1);
            madera = madera - 1;
            if (madera == 0)
                btnMaderaO.Enabled = false;
        }

        private void btnLadrilloO_Click(object sender, EventArgs e)
        {
            lblLadrilloO.Text = Convert.ToString(Convert.ToInt32(lblLadrilloO.Text) + 1);
            ladrillo = ladrillo - 1;
            if (ladrillo == 0)
                btnLadrilloO.Enabled = false;
        }

        private void btnOvejaO_Click(object sender, EventArgs e)
        {
            lblOvejaO.Text = Convert.ToString(Convert.ToInt32(lblOvejaO.Text) + 1);
            oveja = oveja - 1;
            if (oveja == 0)
                btnOvejaO.Enabled = false;
        }

        private void btnTrigoO_Click(object sender, EventArgs e)
        {
            lblTrigoO.Text = Convert.ToString(Convert.ToInt32(lblTrigoO.Text) + 1);
            trigo = trigo - 1;
            if (trigo == 0)
                btnTrigoO.Enabled = false;
        }

        private void btnPiedraO_Click(object sender, EventArgs e)
        {
            lblPiedraO.Text = Convert.ToString(Convert.ToInt32(lblPiedraO.Text) + 1);
            piedra = piedra - 1;
            if (piedra == 0)
                btnPiedraO.Enabled = false;
        }
    }
}
