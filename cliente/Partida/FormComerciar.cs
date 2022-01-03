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
    public partial class FormComerciar : Form
    {
        //Informacion partida necesaria
        string nombre;
        string[] nombres;
        ColorJugador[] colores;
        Socket conn;
        int idP;
        Image Jugador2;

        //Recursos en tu poder
        int[] recursos = new int[] { 0, 0, 0, 0, 0 }; //{Madera,Ladrillo,Oveja,Trigo,Piedra}

        //Tus puertos
        int[] ratiopuertos = new int[] { 4, 4, 4, 4, 4 }; //{ratioMadera, ratioLadrillo...}

        Button[] btnsO;         //btns ofrecer
        Button[] btnsP;         //btns pedir
        Button[] btnsComercio;  //btns comercio
        Label[] lblsO;          //lbls ofrecer
        Label[] lblsP;          //lbls pedir
        PictureBox[] pboxs;     //pbox jugadores

        int Comercio = 0; // 0: comercio con otros jugadores, 1: comercio marítimo  

        public FormComerciar(Socket conn, int idP, string nombre, string[] nombres, ColorJugador[] colores,
            int Madera, int Ladrillo, int Oveja, int Trigo, int Piedra, 
            int ratiomadera, int ratioladrillo, int ratiooveja, int ratiotrigo, int ratiopiedra)
        {
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
            this.nombres = nombres;
            this.colores = colores;
            this.recursos[0] = Madera;
            this.recursos[1] = Ladrillo;
            this.recursos[2] = Oveja;
            this.recursos[3] = Trigo;
            this.recursos[4] = Piedra;
            this.ratiopuertos[0] = ratiomadera;
            this.ratiopuertos[1] = ratioladrillo;
            this.ratiopuertos[2] = ratiooveja;
            this.ratiopuertos[3] = ratiotrigo;
            this.ratiopuertos[4] = ratiopiedra;
            InitializeComponent();
        }

        private void Comerciar_Load(object sender, EventArgs e)
        {
            //labels informando de recursos disponibles para comerciar
            lblMadera.Text = "(" + Convert.ToString(recursos[0]) + ")";
            lblLadrillo.Text = "(" + Convert.ToString(recursos[1]) + ")";
            lblOveja.Text = "(" + Convert.ToString(recursos[2]) + ")";
            lblTrigo.Text = "(" + Convert.ToString(recursos[3]) + ")";
            lblPiedra.Text = "(" + Convert.ToString(recursos[4]) + ")";

            btnsO = new Button[] { btnMasMaderaO, btnMenosMaderaO, btnMasLadrilloO, btnMenosLadrilloO, btnMasOvejaO,
                btnMenosOvejaO, btnMasTrigoO, btnMenosTrigoO, btnMasPiedraO, btnMenosPiedraO };
            btnsP = new Button[] { btnMasMaderaP, btnMenosMaderaP, btnMasLadrilloP, btnMenosLadrilloP, btnMasOvejaP,
                btnMenosOvejaP, btnMasTrigoP, btnMenosTrigoP, btnMasPiedraP, btnMenosPiedraP };
            btnsComercio = new Button[] { btnComercio1, btnComercio2, btnComercio3 };
            lblsO = new Label[] { lblMaderaO, lblLadrilloO, lblOvejaO, lblTrigoO, lblPiedraO };
            lblsP = new Label[] { lblMaderaP, lblLadrilloP, lblOvejaP, lblTrigoP, lblPiedraP };
            pboxs = new PictureBox[] { pbox2, pbox3, pbox4 };


            foreach (Button btn in btnsO)
            {
                btn.Enabled = false;
                btn.Click += this.btnOfrecer_Click;
            }
            foreach (Button btn in btnsP)
            {
                btn.Click += this.btnPedir_Click;
                if (btn.Name.Contains("Menos"))
                    btn.Enabled = false;
            }

            //Solo se activan los botones de aquellos recursos disponibles
            if (recursos[0] > 0)
                btnMasMaderaO.Enabled = true;
            if (recursos[1] > 0)
                btnMasLadrilloO.Enabled = true;
            if (recursos[2] > 0)
                btnMasOvejaO.Enabled = true;
            if (recursos[3] > 0)
                btnMasTrigoO.Enabled = true;
            if (recursos[4] > 0)
                btnMasPiedraO.Enabled = true;
            btnComercio1.Visible = false;
            btnComercio2.Visible = false;
            btnComercio3.Visible = false;

            //Dibujar Jugadores 
            int i = 0;
            int b = 0;
            while (i < nombres.Length && nombres[i] != "")
            {
                Bitmap Figura;
                switch (colores[i])
                {
                    case ColorJugador.Azul:
                        Figura = cliente.Properties.Resources.JugadorAzul;
                        break;
                    case ColorJugador.Rojo:
                        Figura = cliente.Properties.Resources.JugadorRojo;
                        break;
                    case ColorJugador.Naranja:
                        Figura = cliente.Properties.Resources.JugadorNaranja;
                        break;
                    case ColorJugador.Gris:
                        Figura = cliente.Properties.Resources.JugadorGris;
                        break;
                    case ColorJugador.Morado:
                        Figura = cliente.Properties.Resources.JugadorMorado;
                        break;
                    case ColorJugador.Verde:
                        Figura = cliente.Properties.Resources.JugadorVerde;
                        break;
                    default:
                        Figura = cliente.Properties.Resources.JugadorAzul;
                        break;
                }
                if (nombres[i] == nombre)
                    pbox1.Image = Figura;
                else
                {
                    pboxs[b].Image = Figura;
                    b = b + 1;
                }
                i = i + 1;
            }
        }

        private void btnComercioJ_Click(object sender, EventArgs e)
        {
            //Comercio interno con otros jugadores

            pbox2.Image = Jugador2;
            pbox3.Visible = true;
            pbox4.Visible = true;
            btnComercioJ.BackColor = Color.Linen;
            btnComercioM.BackColor = Color.White;
            this.Comercio = 0;
            int i = 0;
            while (i < recursos.Length)
            {
                recursos[i] = recursos[i] + Convert.ToInt32(lblsO[i].Text);
                btnsO[i * 2 + 1].Enabled = false;
                btnsP[i * 2 + 1].Enabled = false;
                lblsO[i].Text = "0";
                lblsP[i].Text = "0";
                if (recursos[i] >= 1)
                    btnsO[i * 2].Enabled = true;
                else
                    btnsO[i * 2].Enabled = false;
                i = i + 1;
            }
        }

        private void btnComercioM_Click(object sender, EventArgs e)
        {
            //Comercio marítimo

            this.Jugador2 = pbox2.Image;
            pbox2.Image = cliente.Properties.Resources.Barco;
            pbox3.Visible = false;
            pbox4.Visible = false;
            btnComercioM.BackColor = Color.Linen;
            btnComercioJ.BackColor = Color.White;
            this.Comercio = 1;
            int i = 0;
            while (i < recursos.Length)
            {
                recursos[i] = recursos[i] + Convert.ToInt32(lblsO[i].Text);
                btnsO[i * 2 + 1].Enabled = false;
                btnsP[i * 2 + 1].Enabled = false;
                lblsO[i].Text = "0";
                lblsP[i].Text = "0";
                if (recursos[i] >= ratiopuertos[i])
                    btnsO[i * 2].Enabled = true;
                else
                    btnsO[i * 2].Enabled = false;
                i = i + 1;
            }
        }
        private void btnComerciar_Click(object sender, EventArgs e)
        {
            if (this.Comercio == 1)
                return;

            //Mandar Peticiones
            //string pet = "27/" + this.idP + "/" + lblMaderaO.Text + "," + lblLadrilloO.Text + "," + lblOvejaO.Text + "," +
            //lblTrigoO.Text + "," + lblPiedraO.Text + "," + lblMaderaP.Text + "," + lblLadrilloP.Text + "," + lblOvejaP.Text + "," +
            //lblTrigoP.Text + "," + lblPiedraP.Text;
            //byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            //this.conn.Send(pet_b);

            int i = 0;
            while (i < (colores.Length-1))
            {
                btnsComercio[i].Visible = true;
                i = i + 1;
            }
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
                                if (Comercio == 0)
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) + 1);
                                    this.recursos[counter] = this.recursos[counter] - 1;
                                }
                                else
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) + ratiopuertos[counter]);
                                    this.recursos[counter] = this.recursos[counter] - ratiopuertos[counter];
                                }
                                btnsO[counter * 2 + 1].Enabled = true;
                                if (recursos[counter] == 0)
                                    btnClicked.Enabled = false;
                            }
                            else
                            {
                                if (Comercio == 0)
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - 1);
                                    this.recursos[counter] = this.recursos[counter] + 1;
                                }
                                else
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - ratiopuertos[counter]);
                                    this.recursos[counter] = this.recursos[counter] + ratiopuertos[counter];
                                }
                                btnsO[counter * 2].Enabled = true;
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
        }
        private void btnPedir_Click(object sender, EventArgs e)
        {
            Button btnClicked = (Button)sender;
            string[] recursosComparar = new string[] { "Madera", "Ladrillo", "Oveja", "Trigo", "Piedra" };

            int counter = 0;
            foreach (string recurso in recursosComparar)
            {
                if (btnClicked.Name.Contains(recurso))
                {
                    foreach (Label lbl in lblsP)
                    {
                        if (lbl.Name.Contains(recurso))
                        {
                            if (btnClicked.Name.Contains("Mas"))
                            {
                                lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) + 1);
                                btnsP[counter * 2 + 1].Enabled = true;
                            }
                            else
                            {
                                lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - 1);
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
        }
    }
}
