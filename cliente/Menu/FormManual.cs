using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente.Menu
{
    public partial class FormManual : Form
    {

        public FormManual()
        {
            InitializeComponent();
        }

        private void FormManual_Load(object sender, EventArgs e)
        {
            Cartas();
        }

        public void Cartas()
        {
            lblSeccion.Text = "Cartas de desarrollo";

            picBox1.Image = global::cliente.Properties.Resources.CartaInvento;
            picBox2.Image = global::cliente.Properties.Resources.CartaMonopolio;
            picBox3.Image = global::cliente.Properties.Resources.CartaCaballero;
            picBox4.Image = global::cliente.Properties.Resources.CartaCarreteras;
            picBox5.Image = global::cliente.Properties.Resources.CartaPunto;

            picBox2.Visible = true;
            picBox3.Visible = true;
            picBox4.Visible = true;
            picBox5.Visible = true;

            lbl1.Text = "El jugador recibe dos recursos a su libre elección.";
            lbl2.Text = "El jugador recibe todos los recursos del tipo seleccionado de todos los otros jugadores.";
            lbl3.Text = "Activa el Ladrón*, permitiendo al jugador colocarlo donde quiera y robando un recurso si es el caso. \n" +
                "*Ver sección de Ladrón";
            lbl4.Text = "El jugador colocará 2 carreteras sin coste alguno.";
            lbl5.Text = "El jugador recibe un punto de victoria extra, solo el propietario puede ver su punto sumado y se muestra al resto al final de la partida\n" +
                "(Carta acumulativa y no utilizable)";
            lbl6.Text = "Nota: Solo se puede usar una carta de desarrollo por turno.";

            lbl1.Size = new Size(280, 46);
            lbl2.Visible = true;
            lbl3.Visible = true;
            lbl4.Visible = true;
            lbl5.Visible = true;
            lbl6.Visible = true;
        }

        private void btnCartas_Click(object sender, EventArgs e)
        {
            Cartas();
        }

        private void btnLadron_Click(object sender, EventArgs e)
        {
            lblSeccion.Text = "Ladrón";

            picBox1.Image = global::cliente.Properties.Resources.Ladron1;
            picBox2.Visible = false;
            picBox3.Visible = false;
            picBox4.Visible = false;
            picBox5.Visible = false;

            lbl1.Text = "La ficha inicialmente se coloca en el Desierto y solo se mueve de hexágono en dos casos: \n" +
                "\t 1. Al tirar los dados sale un 7* \n " +
                "\t 2. Un jugador usa la carta de desarrollo Caballero \n\n" +
                "En los dos casos el jugador que haya tirado los dados o usado la carta después de mover la ficha del Ladrón a un nuevo hexágono " +
                "deberá ha escoger a que pueblo o ciudad de otro jugador colocado en los vértices desea robar un recurso aleatorio. \n\n" + 
                "* En el caso que se active el Ladrón mediante los dados todos los jugadores que dispongan de más de 7 recursos deberán entregar la mitad de ellos.";

            lbl1.Size = new Size(280, 300);
            lbl2.Visible = false;
            lbl3.Visible = false;
            lbl4.Visible = false;
            lbl5.Visible = false;
            lbl6.Visible = false;

        }
    }
}
