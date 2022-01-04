using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Linq;

namespace cliente.Partida
{
    public partial class FormLadron : Form
    {
        //Recursos en tu poder
        string nombre;
        int[] recursos;
        int idP;
        int todos;
        int obligatorios;
        int ofrecidos;
        Socket conn;

        Button[] btns;          //btns ofrecer
        Button[] btnsComercio;  //btns comercio
        Label[] lblsO;          //lbls ofrecer

        public FormLadron(Socket conn, int idP, string nombre, int madera, 
            int ladrillo, int oveja, int trigo, int piedra)
        {
            InitializeComponent();
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
            this.recursos = new int[]
            {
                madera,
                ladrillo,
                oveja,
                trigo,
                piedra
            };
        }

        private void FormLadron_Load(object sender, EventArgs e)
        {
            //labels informando de recursos disponibles para comerciar
            lblMadera.Text = "(" + Convert.ToString(recursos[0]) + ")";
            lblLadrillo.Text = "(" + Convert.ToString(recursos[1]) + ")";
            lblOveja.Text = "(" + Convert.ToString(recursos[2]) + ")";
            lblTrigo.Text = "(" + Convert.ToString(recursos[3]) + ")";
            lblPiedra.Text = "(" + Convert.ToString(recursos[4]) + ")";
            todos = recursos[0] + recursos[1] + recursos[2] + recursos[3] + recursos[4];
            double aDar = todos / 2;
            obligatorios = (int)Math.Floor(aDar);
            ofrecidos = 0;

            btns = new Button[] { btnMasMadera, btnMenosMadera, btnMasLadrillo, btnMenosLadrillo, btnMasOveja,
                btnMenosOveja, btnMasTrigo, btnMenosTrigo, btnMasPiedra, btnMenosPiedra };
            lblsO = new Label[] { lblMaderaO, lblLadrilloO, lblOvejaO, lblTrigoO, lblPiedraO };

            btnAceptar.Enabled = false;

            foreach (Button btn in btns)
            {
                btn.Enabled = false;
                btn.Click += this.btnOfrecer_Click;
            }

            //Solo se activan los botones de aquellos recursos disponibles
            if (recursos[0] > 0)
                btnMasMadera.Enabled = true;
            if (recursos[1] > 0)
                btnMasLadrillo.Enabled = true;
            if (recursos[2] > 0)
                btnMasOveja.Enabled = true;
            if (recursos[3] > 0)
                btnMasTrigo.Enabled = true;
            if (recursos[4] > 0)
                btnMasPiedra.Enabled = true;
        }
        private void btnOfrecer_Click(object sender, EventArgs e)
        {
            Button btnClicked = (Button)sender;
            string[] recursosComparar = new string[] { "Madera", "Ladrillo", "Oveja", "Trigo", "Piedra" };

            int counter = 0;
            foreach (string recurso in recursosComparar)
            {
                if (btnClicked.Name.Contains(recurso))
                {
                    foreach (Label lbl in lblsO)
                    {
                        if (lbl.Name.Contains(recurso))
                        {
                            if (btnClicked.Name.Contains("Mas"))
                            {
                                if (ofrecidos == obligatorios)
                                {
                                    return;
                                }

                                lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) + 1);
                                this.recursos[counter] = this.recursos[counter] - 1;
                                ofrecidos++;
             
                                btns[counter * 2 + 1].Enabled = true;
                                if (recursos[counter] == 0)
                                    btnClicked.Enabled = false;
                            }
                            else
                            {
                                lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - 1);
                                this.recursos[counter] = this.recursos[counter] + 1;
                                ofrecidos--;

                                btns[counter * 2].Enabled = true;
                                if (Convert.ToInt32(lbl.Text) == 0)
                                    btnClicked.Enabled = false;
                            }
                            break;
                        }
                    }
                    break;
                }
                counter = counter + 1;
            }

            if (ofrecidos == obligatorios)
            {
                btnAceptar.Enabled = true;
            }
            else
                btnAceptar.Enabled = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pet = "31/" + this.idP + "/" + nombre + "/" +
                lblMaderaO.Text + "," + lblLadrilloO.Text + "," + lblOvejaO.Text + "," + lblTrigoO.Text + "," + lblPiedraO.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
            this.Close();
        }

    }
}
