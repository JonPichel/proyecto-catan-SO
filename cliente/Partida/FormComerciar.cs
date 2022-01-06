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
        int ofrecer;
        int pedir;

        //Recursos en tu poder
        int[] recursos;

        //Tus puertos
        int[] ratiopuertos;

        Button[] btnsO;                 //btns ofrecer
        Button[] btnsP;                 //btns pedir
        Label[] lblsO;                  //lbls ofrecer
        Label[] lblsP;                  //lbls pedir
        PictureBox[] pboxs;             //pbox jugadores
        PictureBox[] pboxsComercio;     //pbox comercio

        int Comercio = 0; // 0: comercio con otros jugadores, 1: comercio marítimo  

        public FormComerciar(Socket conn, int idP, string nombre, string[] nombres, ColorJugador[] colores,
            int madera, int ladrillo, int oveja, int trigo, int piedra, 
            int ratioMadera, int ratioLadrillo, int ratioOveja, int ratioTrigo, int ratioPiedra)
        {
            this.conn = conn;
            this.idP = idP;
            this.nombre = nombre;
            this.nombres = nombres;
            this.colores = colores;
            this.recursos = new int[]
            {
                madera,
                ladrillo,
                oveja,
                trigo,
                piedra
            };
            this.ratiopuertos = new int[]
            {
                ratioMadera,
                ratioLadrillo,
                ratioOveja,
                ratioTrigo,
                ratioPiedra
            };
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
            pboxsComercio = new PictureBox[] { pboxComercio1, pboxComercio2, pboxComercio3 };
            lblsO = new Label[] { lblMaderaO, lblLadrilloO, lblOvejaO, lblTrigoO, lblPiedraO };
            lblsP = new Label[] { lblMaderaP, lblLadrilloP, lblOvejaP, lblTrigoP, lblPiedraP };
            pboxs = new PictureBox[] { pbox2, pbox3, pbox4 };
            btnComerciar.Enabled = false;
            ofrecer = 0;
            pedir = 0;

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
            foreach (PictureBox pbox in pboxsComercio)
            {
                pbox.Visible = false;
                pbox.Click += this.pboxComercio_Click;
                pbox.MouseHover += this.pboxComercio_MouseHover;
                pbox.MouseLeave += this.pboxComercio_MouseLeave;
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
                    pboxs[b].Tag = nombres[i];
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
            string pet;
            byte[] pet_b;
            if (this.Comercio == 1)
            {
                pet = "30/" + this.idP + "/" + lblMaderaO.Text + "," + lblLadrilloO.Text + "," + lblOvejaO.Text + "," +
                    lblTrigoO.Text + "," + lblPiedraO.Text + "," + lblMaderaP.Text + "," + lblLadrilloP.Text + "," + 
                    lblOvejaP.Text + "," + lblTrigoP.Text + "," + lblPiedraP.Text;
                pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                this.conn.Send(pet_b);
                this.Close();
                return;
            }
            
            pet = "27/" + this.idP + "/" + lblMaderaO.Text + "," + lblLadrilloO.Text + "," + lblOvejaO.Text + "," +
            lblTrigoO.Text + "," + lblPiedraO.Text + "," + lblMaderaP.Text + "," + lblLadrilloP.Text + "," + lblOvejaP.Text + "," +
            lblTrigoP.Text + "," + lblPiedraP.Text;
            pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);

            for (int i = 0; i < (colores.Length - 1); i++) {
                pboxsComercio[i].Visible = true;
                pboxsComercio[i].Image = cliente.Properties.Resources.Espera;
                pboxsComercio[i].Tag = pboxs[i].Tag;
                pboxsComercio[i].Enabled = false;
            }

            foreach (Button btn in btnsO)
            {
                btn.Enabled = false;
            }
            foreach (Button btn in btnsP)
            {
                btn.Enabled = false;
            }
            btnComercioM.Enabled = false;
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
                                    ofrecer = ofrecer + 1;
                                    this.recursos[counter] = this.recursos[counter] - 1;
                                }
                                else
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) + ratiopuertos[counter]);
                                    ofrecer = ofrecer + ratiopuertos[counter];
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
                                    ofrecer = ofrecer - 1;
                                    this.recursos[counter] = this.recursos[counter] + 1;
                                }
                                else
                                {
                                    lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - ratiopuertos[counter]);
                                    ofrecer = ofrecer - ratiopuertos[counter];
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
            if (pedir > 0 && ofrecer > 0)
                btnComerciar.Enabled = true;
            else
                btnComerciar.Enabled = false;
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
                                pedir = pedir + 1;
                                btnsP[counter * 2 + 1].Enabled = true;
                            }
                            else
                            {
                                lbl.Text = Convert.ToString(Convert.ToInt32(lbl.Text) - 1);
                                pedir = pedir - 1;
                                if (Convert.ToInt32(lbl.Text) == 0)
                                    btnClicked.Enabled = false;
                            }
                            break;
                        }
                    }
                    break;
                }
                counter++;
            }
            if (pedir > 0 && ofrecer > 0)
                btnComerciar.Enabled = true;
            else
                btnComerciar.Enabled = false;
        }

        public void ActualizarRespuesta(string mensaje)
        {
            string[] trozos = mensaje.Split("/");

            foreach (PictureBox pbox in pboxsComercio)
            {
                if (pbox.Tag.ToString() == trozos[2])
                {
                    if (trozos[3] == "SI")
                    {
                        pbox.Image = cliente.Properties.Resources.Si;
                        pbox.Enabled = true;
                    }
                    else
                        pbox.Image = cliente.Properties.Resources.No;
                    break;
                }
            }
        }

        private void pboxComercio_Click(object sender, EventArgs e)
        {
            PictureBox pbox = (PictureBox)sender;
            string pet = "29/" + this.idP + "/" + pbox.Tag.ToString() + "/" +
                            lblMaderaO.Text + "," + lblLadrilloO.Text + "," + lblOvejaO.Text + "," + lblTrigoO.Text + "," + lblPiedraO.Text + "," +
                            lblMaderaP.Text + "," + lblLadrilloP.Text + "," + lblOvejaP.Text + "," + lblTrigoP.Text + "," + lblPiedraP.Text;
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            this.conn.Send(pet_b);
            this.Close();
        }

        private void pboxComercio_MouseHover(object sender, EventArgs e)
        {
            PictureBox pbox = (PictureBox)sender;
            pbox.BackColor = Color.DimGray;
        }
        private void pboxComercio_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pbox = (PictureBox)sender;
            pbox.BackColor = Color.White;
        }
    }
}
