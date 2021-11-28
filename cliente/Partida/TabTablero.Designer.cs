
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
            this.btnCiudad = new System.Windows.Forms.Button();
            this.btnCasilla = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridJugadores = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnCaballero = new System.Windows.Forms.Button();
            this.btnInvento = new System.Windows.Forms.Button();
            this.btnMonopolio = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPoblado
            // 
            this.btnPoblado.Location = new System.Drawing.Point(560, 522);
            this.btnPoblado.Name = "btnPoblado";
            this.btnPoblado.Size = new System.Drawing.Size(75, 75);
            this.btnPoblado.TabIndex = 0;
            this.btnPoblado.Text = "Colocar Poblado";
            this.btnPoblado.UseVisualStyleBackColor = true;
            this.btnPoblado.Click += new System.EventHandler(this.btnPoblado_Click);
            // 
            // btnCarretera
            // 
            this.btnCarretera.Location = new System.Drawing.Point(722, 522);
            this.btnCarretera.Name = "btnCarretera";
            this.btnCarretera.Size = new System.Drawing.Size(75, 75);
            this.btnCarretera.TabIndex = 1;
            this.btnCarretera.Text = "Colocar Carretera";
            this.btnCarretera.UseVisualStyleBackColor = true;
            this.btnCarretera.Click += new System.EventHandler(this.btnCarretera_Click);
            // 
            // btnCiudad
            // 
            this.btnCiudad.Location = new System.Drawing.Point(641, 522);
            this.btnCiudad.Name = "btnCiudad";
            this.btnCiudad.Size = new System.Drawing.Size(75, 75);
            this.btnCiudad.TabIndex = 2;
            this.btnCiudad.Text = "Colocar Ciudad";
            this.btnCiudad.UseVisualStyleBackColor = true;
            this.btnCiudad.Click += new System.EventHandler(this.btnCiudad_Click);
            // 
            // btnCasilla
            // 
            this.btnCasilla.Location = new System.Drawing.Point(479, 522);
            this.btnCasilla.Name = "btnCasilla";
            this.btnCasilla.Size = new System.Drawing.Size(75, 75);
            this.btnCasilla.TabIndex = 3;
            this.btnCasilla.Text = "Click Casilla";
            this.btnCasilla.UseVisualStyleBackColor = true;
            this.btnCasilla.Click += new System.EventHandler(this.btnCasilla_Click);
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
            this.panel1.Controls.Add(this.dataGridJugadores);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 221);
            this.panel1.TabIndex = 4;
            // 
            // dataGridJugadores
            // 
            this.dataGridJugadores.AllowUserToAddRows = false;
            this.dataGridJugadores.AllowUserToDeleteRows = false;
            this.dataGridJugadores.AllowUserToResizeColumns = false;
            this.dataGridJugadores.AllowUserToResizeRows = false;
            this.dataGridJugadores.BackgroundColor = System.Drawing.Color.White;
            this.dataGridJugadores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridJugadores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridJugadores.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridJugadores.Location = new System.Drawing.Point(16, 49);
            this.dataGridJugadores.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridJugadores.Name = "dataGridJugadores";
            this.dataGridJugadores.ReadOnly = true;
            this.dataGridJugadores.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridJugadores.RowHeadersWidth = 62;
            this.dataGridJugadores.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridJugadores.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridJugadores.Size = new System.Drawing.Size(190, 162);
            this.dataGridJugadores.TabIndex = 3;
            this.dataGridJugadores.Text = "dataGridView1";
            this.dataGridJugadores.SelectionChanged += new System.EventHandler(this.dataGridJugadores_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(61, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Jugadores";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.btnCaballero);
            this.panel2.Controls.Add(this.btnInvento);
            this.panel2.Controls.Add(this.btnMonopolio);
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 149);
            this.panel2.TabIndex = 5;
            // 
            // panel8
            // 
            this.panel8.Location = new System.Drawing.Point(4, 115);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(158, 25);
            this.panel8.TabIndex = 6;
            // 
            // panel7
            // 
            this.panel7.Location = new System.Drawing.Point(4, 88);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(158, 25);
            this.panel7.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(4, 62);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(158, 25);
            this.panel6.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(4, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(158, 25);
            this.panel5.TabIndex = 6;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(4, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(158, 25);
            this.panel4.TabIndex = 6;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // btnCaballero
            // 
            this.btnCaballero.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCaballero.Location = new System.Drawing.Point(168, 62);
            this.btnCaballero.Name = "btnCaballero";
            this.btnCaballero.Size = new System.Drawing.Size(49, 23);
            this.btnCaballero.TabIndex = 6;
            this.btnCaballero.Text = "Usar";
            this.btnCaballero.UseVisualStyleBackColor = true;
            // 
            // btnInvento
            // 
            this.btnInvento.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInvento.Location = new System.Drawing.Point(168, 35);
            this.btnInvento.Name = "btnInvento";
            this.btnInvento.Size = new System.Drawing.Size(49, 23);
            this.btnInvento.TabIndex = 6;
            this.btnInvento.Text = "Usar";
            this.btnInvento.UseVisualStyleBackColor = true;
            // 
            // btnMonopolio
            // 
            this.btnMonopolio.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMonopolio.Location = new System.Drawing.Point(168, 9);
            this.btnMonopolio.Name = "btnMonopolio";
            this.btnMonopolio.Size = new System.Drawing.Size(49, 23);
            this.btnMonopolio.TabIndex = 6;
            this.btnMonopolio.Text = "Usar";
            this.btnMonopolio.UseVisualStyleBackColor = true;
            // 
            // TabTablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnCasilla);
            this.Controls.Add(this.btnCiudad);
            this.Controls.Add(this.btnCarretera);
            this.Controls.Add(this.btnPoblado);
            this.Name = "TabTablero";
            this.Load += new System.EventHandler(this.TabTablero_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPoblado;
        private System.Windows.Forms.Button btnCarretera;
        private System.Windows.Forms.Button btnCiudad;
        private System.Windows.Forms.Button btnCasilla;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox txtChat;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridJugadores;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnCaballero;
        private System.Windows.Forms.Button btnInvento;
        private System.Windows.Forms.Button btnMonopolio;
    }
}
