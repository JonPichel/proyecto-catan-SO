
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
            this.btnCiudad = new System.Windows.Forms.Button();
            this.btnComercio = new System.Windows.Forms.Button();
            this.pnlTablero = new cliente.Partida.PanelTablero();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPoblado
            // 
            this.btnPoblado.Location = new System.Drawing.Point(545, 532);
            this.btnPoblado.Name = "btnPoblado";
            this.btnPoblado.Size = new System.Drawing.Size(60, 30);
            this.btnPoblado.TabIndex = 0;
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
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 221);
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
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(228, 532);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(311, 65);
            this.panel4.TabIndex = 7;
            // 
            // btnCiudad
            // 
            this.btnCiudad.Location = new System.Drawing.Point(545, 567);
            this.btnCiudad.Name = "btnCiudad";
            this.btnCiudad.Size = new System.Drawing.Size(60, 30);
            this.btnCiudad.TabIndex = 8;
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
            this.pnlTablero.Location = new System.Drawing.Point(229, 3);
            this.pnlTablero.Name = "pnlTablero";
            this.pnlTablero.Size = new System.Drawing.Size(568, 488);
            this.pnlTablero.TabIndex = 10;
            // 
            // TabTablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
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
            this.Name = "TabTablero";
            this.Load += new System.EventHandler(this.TabTablero_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

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
    }
}
