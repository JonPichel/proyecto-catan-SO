using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace cliente.Menu
{
    public partial class FormMenu : Form
    {
        Socket conn;
        int idJ;

        public FormMenu(Socket conn, int idJ)
        {
            InitializeComponent();
            this.conn = conn;
            this.idJ = idJ;
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            string pet = "5/" + idJ.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);

            byte[] res_b = new byte[512];
            conn.Receive(res_b);
            string res = Encoding.ASCII.GetString(res_b).Split('\0')[0];

            lblMedia.Text = "Puntuación media: " + res;
        }
    }
}
