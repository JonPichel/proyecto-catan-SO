using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cliente
{
    public partial class FormEscoger : Form
    {
        public FormEscoger(string message, string btnText1, string btnText2)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(111, 145, 111);
            lblTexto.Text = message;
            btnOpcion1.Text = btnText1;
            btnOpcion2.Text = btnText2;
        }

        private void btnOpcion1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnOpcion2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            Close();
        }
    }
}
