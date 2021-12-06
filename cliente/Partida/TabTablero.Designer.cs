
namespace cliente.Partida
{
    partial class TabTablero
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabTablero));
            this.btnPoblado = new System.Windows.Forms.Button();
            this.btnCarretera = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTurno = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblOveja = new System.Windows.Forms.Label();
            this.lblTrigo = new System.Windows.Forms.Label();
            this.lblLadrillo = new System.Windows.Forms.Label();
            this.lblPiedra = new System.Windows.Forms.Label();
            this.lblMadera = new System.Windows.Forms.Label();
            this.btnCiudad = new System.Windows.Forms.Button();
            this.btnComercio = new System.Windows.Forms.Button();
            this.pnlTablero = new cliente.Partida.PanelTablero();
            this.lblTurno = new System.Windows.Forms.Label();
            this.btnDesarrollo = new System.Windows.Forms.Button();
            this.tooltipCostes = new System.Windows.Forms.ToolTip(this.components);
            this.pnlJugador1 = new cliente.Partida.PanelInfoJugador();
            this.pnlJugador2 = new cliente.Partida.PanelInfoJugador();
            this.pnlJugador3 = new cliente.Partida.PanelInfoJugador();
            this.pnlJugador4 = new cliente.Partida.PanelInfoJugador();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPoblado
            // 
            this.btnPoblado.Location = new System.Drawing.Point(545, 532);
            this.btnPoblado.Name = "btnPoblado";
            this.btnPoblado.Size = new System.Drawing.Size(60, 30);
            this.btnPoblado.TabIndex = 0;
            this.btnPoblado.Tag = "Madera,Ladrillo,Oveja,Paja";
            this.btnPoblado.Text = "Pueblo";
            this.btnPoblado.UseVisualStyleBackColor = true;
            this.btnPoblado.Click += new System.EventHandler(this.btnPoblado_Click);
            // 
            // btnCarretera
            // 
            this.btnCarretera.Location = new System.Drawing.Point(545, 497);
            this.btnCarretera.Name = "btnCarretera";
            this.btnCarretera.Size = new System.Drawing.Size(60, 30);
            this.btnCarretera.TabIndex = 1;
            this.btnCarretera.Tag = "Madera,Ladrillo";
            this.btnCarretera.Text = "Camino";
            this.btnCarretera.UseVisualStyleBackColor = true;
            this.btnCarretera.Click += new System.EventHandler(this.btnCarretera_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(126, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Invitar";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.White;
            this.txtChat.Location = new System.Drawing.Point(4, 4);
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(213, 151);
            this.txtChat.TabIndex = 4;
            this.txtChat.Text = "";
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(79, 190);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(60, 23);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(4, 161);
            this.txtMsg.MaxLength = 400;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(213, 23);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMsg_KeyPress);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtChat);
            this.panel3.Controls.Add(this.btnEnviar);
            this.panel3.Controls.Add(this.txtMsg);
            this.panel3.Location = new System.Drawing.Point(0, 378);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(223, 219);
            this.panel3.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(0, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 150);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 149);
            this.panel2.TabIndex = 5;
            // 
            // btnTurno
            // 
            this.btnTurno.Location = new System.Drawing.Point(697, 497);
            this.btnTurno.Name = "btnTurno";
            this.btnTurno.Size = new System.Drawing.Size(100, 100);
            this.btnTurno.TabIndex = 6;
            this.btnTurno.Text = "Tirar dado";
            this.btnTurno.UseVisualStyleBackColor = true;
            this.btnTurno.Click += new System.EventHandler(this.btnTurno_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.lblOveja);
            this.panel4.Controls.Add(this.lblTrigo);
            this.panel4.Controls.Add(this.lblLadrillo);
            this.panel4.Controls.Add(this.lblPiedra);
            this.panel4.Controls.Add(this.lblMadera);
            this.panel4.Location = new System.Drawing.Point(228, 532);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(311, 65);
            this.panel4.TabIndex = 7;
            // 
            // lblOveja
            // 
            this.lblOveja.BackColor = System.Drawing.Color.Transparent;
            this.lblOveja.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblOveja.ForeColor = System.Drawing.Color.Black;
            this.lblOveja.Image = ((System.Drawing.Image)(resources.GetObject("lblOveja.Image")));
            this.lblOveja.Location = new System.Drawing.Point(130, 7);
            this.lblOveja.Name = "lblOveja";
            this.lblOveja.Size = new System.Drawing.Size(50, 50);
            this.lblOveja.TabIndex = 0;
            this.lblOveja.Text = "0";
            this.lblOveja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrigo
            // 
            this.lblTrigo.BackColor = System.Drawing.Color.Transparent;
            this.lblTrigo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrigo.ForeColor = System.Drawing.Color.White;
            this.lblTrigo.Image = ((System.Drawing.Image)(resources.GetObject("lblTrigo.Image")));
            this.lblTrigo.Location = new System.Drawing.Point(190, 7);
            this.lblTrigo.Name = "lblTrigo";
            this.lblTrigo.Size = new System.Drawing.Size(50, 50);
            this.lblTrigo.TabIndex = 0;
            this.lblTrigo.Text = "0";
            this.lblTrigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLadrillo
            // 
            this.lblLadrillo.BackColor = System.Drawing.Color.Transparent;
            this.lblLadrillo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLadrillo.ForeColor = System.Drawing.Color.White;
            this.lblLadrillo.Image = ((System.Drawing.Image)(resources.GetObject("lblLadrillo.Image")));
            this.lblLadrillo.Location = new System.Drawing.Point(70, 7);
            this.lblLadrillo.Name = "lblLadrillo";
            this.lblLadrillo.Size = new System.Drawing.Size(50, 50);
            this.lblLadrillo.TabIndex = 0;
            this.lblLadrillo.Text = "0";
            this.lblLadrillo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPiedra
            // 
            this.lblPiedra.BackColor = System.Drawing.Color.Transparent;
            this.lblPiedra.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPiedra.ForeColor = System.Drawing.Color.White;
            this.lblPiedra.Image = ((System.Drawing.Image)(resources.GetObject("lblPiedra.Image")));
            this.lblPiedra.Location = new System.Drawing.Point(250, 7);
            this.lblPiedra.Name = "lblPiedra";
            this.lblPiedra.Size = new System.Drawing.Size(50, 50);
            this.lblPiedra.TabIndex = 0;
            this.lblPiedra.Text = "0";
            this.lblPiedra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMadera
            // 
            this.lblMadera.BackColor = System.Drawing.Color.Transparent;
            this.lblMadera.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMadera.ForeColor = System.Drawing.Color.White;
            this.lblMadera.Image = ((System.Drawing.Image)(resources.GetObject("lblMadera.Image")));
            this.lblMadera.Location = new System.Drawing.Point(10, 7);
            this.lblMadera.Name = "lblMadera";
            this.lblMadera.Size = new System.Drawing.Size(50, 50);
            this.lblMadera.TabIndex = 0;
            this.lblMadera.Text = "0";
            this.lblMadera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCiudad
            // 
            this.btnCiudad.Location = new System.Drawing.Point(545, 567);
            this.btnCiudad.Name = "btnCiudad";
            this.btnCiudad.Size = new System.Drawing.Size(60, 30);
            this.btnCiudad.TabIndex = 8;
            this.btnCiudad.Tag = "Paja(2),Piedra(3)";
            this.btnCiudad.Text = "Ciudad";
            this.btnCiudad.UseVisualStyleBackColor = true;
            // 
            // btnComercio
            // 
            this.btnComercio.Location = new System.Drawing.Point(611, 497);
            this.btnComercio.Name = "btnComercio";
            this.btnComercio.Size = new System.Drawing.Size(80, 100);
            this.btnComercio.TabIndex = 9;
            this.btnComercio.Text = "Comerciar";
            this.btnComercio.UseVisualStyleBackColor = true;
            // 
            // pnlTablero
            // 
            this.pnlTablero.Location = new System.Drawing.Point(229, 71);
            this.pnlTablero.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTablero.Name = "pnlTablero";
            this.pnlTablero.Size = new System.Drawing.Size(568, 420);
            this.pnlTablero.TabIndex = 10;
            // 
            // lblTurno
            // 
            this.lblTurno.AutoSize = true;
            this.lblTurno.Location = new System.Drawing.Point(229, 497);
            this.lblTurno.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTurno.Name = "lblTurno";
            this.lblTurno.Size = new System.Drawing.Size(44, 15);
            this.lblTurno.TabIndex = 11;
            this.lblTurno.Text = "Turno: ";
            // 
            // btnDesarrollo
            // 
            this.btnDesarrollo.Location = new System.Drawing.Point(479, 497);
            this.btnDesarrollo.Name = "btnDesarrollo";
            this.btnDesarrollo.Size = new System.Drawing.Size(60, 30);
            this.btnDesarrollo.TabIndex = 12;
            this.btnDesarrollo.Tag = "Oveja,Paja,Piedra";
            this.btnDesarrollo.Text = "Carta";
            this.btnDesarrollo.UseVisualStyleBackColor = true;
            // 
            // pnlJugador1
            // 
            this.pnlJugador1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(171)))), ((int)(((byte)(200)))));
            this.pnlJugador1.Caballeros = 0;
            this.pnlJugador1.Carreteras = 0;
            this.pnlJugador1.Color = cliente.Partida.ColorJugador.Azul;
            this.pnlJugador1.Desarrollo = 0;
            this.pnlJugador1.Location = new System.Drawing.Point(4, 5);
            this.pnlJugador1.Name = "pnlJugador1";
            this.pnlJugador1.Nombre = "Nombre";
            this.pnlJugador1.Puntos = 0;
            this.pnlJugador1.Recursos = 0;
            this.pnlJugador1.Size = new System.Drawing.Size(195, 60);
            this.pnlJugador1.TabIndex = 13;
            // 
            // pnlJugador2
            // 
            this.pnlJugador2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(171)))), ((int)(((byte)(200)))));
            this.pnlJugador2.Caballeros = 0;
            this.pnlJugador2.Carreteras = 0;
            this.pnlJugador2.Color = cliente.Partida.ColorJugador.Azul;
            this.pnlJugador2.Desarrollo = 0;
            this.pnlJugador2.Location = new System.Drawing.Point(203, 5);
            this.pnlJugador2.Name = "pnlJugador2";
            this.pnlJugador2.Nombre = "Nombre";
            this.pnlJugador2.Puntos = 0;
            this.pnlJugador2.Recursos = 0;
            this.pnlJugador2.Size = new System.Drawing.Size(195, 60);
            this.pnlJugador2.TabIndex = 13;
            // 
            // pnlJugador3
            // 
            this.pnlJugador3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(171)))), ((int)(((byte)(200)))));
            this.pnlJugador3.Caballeros = 0;
            this.pnlJugador3.Carreteras = 0;
            this.pnlJugador3.Color = cliente.Partida.ColorJugador.Azul;
            this.pnlJugador3.Desarrollo = 0;
            this.pnlJugador3.Location = new System.Drawing.Point(402, 5);
            this.pnlJugador3.Name = "pnlJugador3";
            this.pnlJugador3.Nombre = "Nombre";
            this.pnlJugador3.Puntos = 0;
            this.pnlJugador3.Recursos = 0;
            this.pnlJugador3.Size = new System.Drawing.Size(195, 60);
            this.pnlJugador3.TabIndex = 13;
            // 
            // pnlJugador4
            // 
            this.pnlJugador4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(171)))), ((int)(((byte)(200)))));
            this.pnlJugador4.Caballeros = 0;
            this.pnlJugador4.Carreteras = 0;
            this.pnlJugador4.Color = cliente.Partida.ColorJugador.Azul;
            this.pnlJugador4.Desarrollo = 0;
            this.pnlJugador4.Location = new System.Drawing.Point(601, 5);
            this.pnlJugador4.Name = "pnlJugador4";
            this.pnlJugador4.Nombre = "Nombre";
            this.pnlJugador4.Puntos = 0;
            this.pnlJugador4.Recursos = 0;
            this.pnlJugador4.Size = new System.Drawing.Size(195, 60);
            this.pnlJugador4.TabIndex = 13;
            // 
            // TabTablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlJugador4);
            this.Controls.Add(this.pnlJugador3);
            this.Controls.Add(this.pnlJugador2);
            this.Controls.Add(this.pnlJugador1);
            this.Controls.Add(this.btnDesarrollo);
            this.Controls.Add(this.lblTurno);
            this.Controls.Add(this.pnlTablero);
            this.Controls.Add(this.btnComercio);
            this.Controls.Add(this.btnCiudad);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.btnTurno);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnCarretera);
            this.Controls.Add(this.btnPoblado);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TabTablero";
            this.Load += new System.EventHandler(this.TabTablero_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPoblado;
        private System.Windows.Forms.Button btnCarretera;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox txtChat;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnTurno;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnCiudad;
        private System.Windows.Forms.Button btnComercio;
        private PanelTablero pnlTablero;
        private System.Windows.Forms.Label lblTurno;
        private System.Windows.Forms.Button btnDesarrollo;
        private System.Windows.Forms.ToolTip tooltipCostes;
        private PanelInfoJugador pnlJugador1;
        private PanelInfoJugador pnlJugador2;
        private PanelInfoJugador pnlJugador3;
        private PanelInfoJugador pnlJugador4;
        private System.Windows.Forms.Label lblOveja;
        private System.Windows.Forms.Label lblTrigo;
        private System.Windows.Forms.Label lblLadrillo;
        private System.Windows.Forms.Label lblPiedra;
        private System.Windows.Forms.Label lblMadera;
    }
}
