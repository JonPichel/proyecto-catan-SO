namespace cliente.Partida
{
    partial class TabLobbyHost
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
            this.btnEmpezar = new System.Windows.Forms.Button();
            this.panelChat = new System.Windows.Forms.Panel();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.dataGridJugadores = new System.Windows.Forms.DataGridView();
            this.panelJugadores = new System.Windows.Forms.Panel();
            this.panelColor = new System.Windows.Forms.Panel();
            this.btnC5 = new System.Windows.Forms.Button();
            this.btnC6 = new System.Windows.Forms.Button();
            this.btnC4 = new System.Windows.Forms.Button();
            this.btnC3 = new System.Windows.Forms.Button();
            this.btnC2 = new System.Windows.Forms.Button();
            this.btnC1 = new System.Windows.Forms.Button();
            this.btnCambioColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInvitar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridConectados = new System.Windows.Forms.DataGridView();
            this.panelConectados = new System.Windows.Forms.Panel();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.panelChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).BeginInit();
            this.panelJugadores.SuspendLayout();
            this.panelColor.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConectados)).BeginInit();
            this.panelConectados.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEmpezar
            // 
            this.btnEmpezar.BackColor = System.Drawing.Color.White;
            this.btnEmpezar.Location = new System.Drawing.Point(126, 454);
            this.btnEmpezar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmpezar.Name = "btnEmpezar";
            this.btnEmpezar.Size = new System.Drawing.Size(163, 31);
            this.btnEmpezar.TabIndex = 1;
            this.btnEmpezar.Text = "EMPEZAR LA PARTIDA";
            this.btnEmpezar.UseVisualStyleBackColor = false;
            this.btnEmpezar.Click += new System.EventHandler(this.btnEmpezar_Click);
            // 
            // panelChat
            // 
            this.panelChat.BackColor = System.Drawing.Color.White;
            this.panelChat.Controls.Add(this.txtChat);
            this.panelChat.Controls.Add(this.btnEnviar);
            this.panelChat.Controls.Add(this.txtMsg);
            this.panelChat.Location = new System.Drawing.Point(432, 265);
            this.panelChat.Margin = new System.Windows.Forms.Padding(2);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(368, 335);
            this.panelChat.TabIndex = 2;
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.White;
            this.txtChat.Location = new System.Drawing.Point(3, 3);
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(359, 274);
            this.txtChat.TabIndex = 4;
            this.txtChat.Text = "";
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(302, 280);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(60, 23);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(3, 281);
            this.txtMsg.MaxLength = 400;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(293, 23);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMsg_KeyPress);
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
            this.dataGridJugadores.Location = new System.Drawing.Point(53, 50);
            this.dataGridJugadores.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridJugadores.Name = "dataGridJugadores";
            this.dataGridJugadores.ReadOnly = true;
            this.dataGridJugadores.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridJugadores.RowHeadersWidth = 62;
            this.dataGridJugadores.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridJugadores.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridJugadores.Size = new System.Drawing.Size(190, 204);
            this.dataGridJugadores.TabIndex = 3;
            this.dataGridJugadores.Text = "dataGridView1";
            this.dataGridJugadores.SelectionChanged += new System.EventHandler(this.dataGridJugadores_SelectionChanged);
            // 
            // panelJugadores
            // 
            this.panelJugadores.BackColor = System.Drawing.Color.White;
            this.panelJugadores.Controls.Add(this.panelColor);
            this.panelJugadores.Controls.Add(this.label1);
            this.panelJugadores.Controls.Add(this.dataGridJugadores);
            this.panelJugadores.Location = new System.Drawing.Point(69, 105);
            this.panelJugadores.Margin = new System.Windows.Forms.Padding(2);
            this.panelJugadores.Name = "panelJugadores";
            this.panelJugadores.Size = new System.Drawing.Size(298, 336);
            this.panelJugadores.TabIndex = 2;
            // 
            // panelColor
            // 
            this.panelColor.BackColor = System.Drawing.Color.AntiqueWhite;
            this.panelColor.Controls.Add(this.btnC5);
            this.panelColor.Controls.Add(this.btnC6);
            this.panelColor.Controls.Add(this.btnC4);
            this.panelColor.Controls.Add(this.btnC3);
            this.panelColor.Controls.Add(this.btnC2);
            this.panelColor.Controls.Add(this.btnC1);
            this.panelColor.Controls.Add(this.btnCambioColor);
            this.panelColor.Controls.Add(this.label2);
            this.panelColor.Location = new System.Drawing.Point(17, 258);
            this.panelColor.Margin = new System.Windows.Forms.Padding(2);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(266, 62);
            this.panelColor.TabIndex = 2;
            // 
            // btnC5
            // 
            this.btnC5.Location = new System.Drawing.Point(21, 32);
            this.btnC5.Margin = new System.Windows.Forms.Padding(2);
            this.btnC5.Name = "btnC5";
            this.btnC5.Size = new System.Drawing.Size(33, 17);
            this.btnC5.TabIndex = 2;
            this.btnC5.UseVisualStyleBackColor = true;
            // 
            // btnC6
            // 
            this.btnC6.Location = new System.Drawing.Point(207, 32);
            this.btnC6.Margin = new System.Windows.Forms.Padding(2);
            this.btnC6.Name = "btnC6";
            this.btnC6.Size = new System.Drawing.Size(33, 17);
            this.btnC6.TabIndex = 2;
            this.btnC6.UseVisualStyleBackColor = true;
            // 
            // btnC4
            // 
            this.btnC4.Location = new System.Drawing.Point(170, 32);
            this.btnC4.Margin = new System.Windows.Forms.Padding(2);
            this.btnC4.Name = "btnC4";
            this.btnC4.Size = new System.Drawing.Size(33, 17);
            this.btnC4.TabIndex = 2;
            this.btnC4.UseVisualStyleBackColor = true;
            // 
            // btnC3
            // 
            this.btnC3.Location = new System.Drawing.Point(58, 32);
            this.btnC3.Margin = new System.Windows.Forms.Padding(2);
            this.btnC3.Name = "btnC3";
            this.btnC3.Size = new System.Drawing.Size(33, 17);
            this.btnC3.TabIndex = 2;
            this.btnC3.UseVisualStyleBackColor = true;
            // 
            // btnC2
            // 
            this.btnC2.Location = new System.Drawing.Point(132, 32);
            this.btnC2.Margin = new System.Windows.Forms.Padding(2);
            this.btnC2.Name = "btnC2";
            this.btnC2.Size = new System.Drawing.Size(33, 17);
            this.btnC2.TabIndex = 2;
            this.btnC2.UseVisualStyleBackColor = true;
            // 
            // btnC1
            // 
            this.btnC1.Location = new System.Drawing.Point(95, 32);
            this.btnC1.Margin = new System.Windows.Forms.Padding(2);
            this.btnC1.Name = "btnC1";
            this.btnC1.Size = new System.Drawing.Size(33, 17);
            this.btnC1.TabIndex = 2;
            this.btnC1.UseVisualStyleBackColor = true;
            // 
            // btnCambioColor
            // 
            this.btnCambioColor.BackColor = System.Drawing.Color.White;
            this.btnCambioColor.Location = new System.Drawing.Point(78, 1);
            this.btnCambioColor.Margin = new System.Windows.Forms.Padding(2);
            this.btnCambioColor.Name = "btnCambioColor";
            this.btnCambioColor.Size = new System.Drawing.Size(99, 27);
            this.btnCambioColor.TabIndex = 1;
            this.btnCambioColor.Text = "Cambiar color";
            this.btnCambioColor.UseVisualStyleBackColor = false;
            this.btnCambioColor.Click += new System.EventHandler(this.btnCambioColor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(83, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Jugadores";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.panel1.Controls.Add(this.btnInvitar);
            this.panel1.Location = new System.Drawing.Point(16, 203);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(337, 46);
            this.panel1.TabIndex = 2;
            // 
            // btnInvitar
            // 
            this.btnInvitar.BackColor = System.Drawing.Color.White;
            this.btnInvitar.Location = new System.Drawing.Point(126, 11);
            this.btnInvitar.Margin = new System.Windows.Forms.Padding(2);
            this.btnInvitar.Name = "btnInvitar";
            this.btnInvitar.Size = new System.Drawing.Size(99, 24);
            this.btnInvitar.TabIndex = 1;
            this.btnInvitar.Text = "Invitar";
            this.btnInvitar.UseVisualStyleBackColor = false;
            this.btnInvitar.Click += new System.EventHandler(this.btnInvitar_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 25);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 17);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(209, 25);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 17);
            this.button2.TabIndex = 2;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(172, 25);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 17);
            this.button3.TabIndex = 2;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(60, 25);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 17);
            this.button4.TabIndex = 2;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(134, 25);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 17);
            this.button5.TabIndex = 2;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(97, 25);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 17);
            this.button6.TabIndex = 2;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(79, 1);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(99, 20);
            this.button7.TabIndex = 1;
            this.button7.Text = "Cambiar color";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 15);
            this.label3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(106, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "Jugadores conectados";
            // 
            // dataGridConectados
            // 
            this.dataGridConectados.AllowUserToAddRows = false;
            this.dataGridConectados.AllowUserToDeleteRows = false;
            this.dataGridConectados.AllowUserToResizeColumns = false;
            this.dataGridConectados.AllowUserToResizeRows = false;
            this.dataGridConectados.BackgroundColor = System.Drawing.Color.White;
            this.dataGridConectados.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridConectados.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridConectados.Location = new System.Drawing.Point(25, 40);
            this.dataGridConectados.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridConectados.Name = "dataGridConectados";
            this.dataGridConectados.ReadOnly = true;
            this.dataGridConectados.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridConectados.RowHeadersWidth = 62;
            this.dataGridConectados.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridConectados.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridConectados.Size = new System.Drawing.Size(337, 150);
            this.dataGridConectados.TabIndex = 3;
            this.dataGridConectados.Text = "dataGridView1";
            this.dataGridConectados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridConectados_CellClick);
            // 
            // panelConectados
            // 
            this.panelConectados.BackColor = System.Drawing.Color.White;
            this.panelConectados.Controls.Add(this.panel1);
            this.panelConectados.Controls.Add(this.label4);
            this.panelConectados.Controls.Add(this.dataGridConectados);
            this.panelConectados.Location = new System.Drawing.Point(432, 1);
            this.panelConectados.Margin = new System.Windows.Forms.Padding(2);
            this.panelConectados.Name = "panelConectados";
            this.panelConectados.Size = new System.Drawing.Size(368, 260);
            this.panelConectados.TabIndex = 2;
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.White;
            this.btnDesconectar.Location = new System.Drawing.Point(126, 489);
            this.btnDesconectar.Margin = new System.Windows.Forms.Padding(2);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(163, 31);
            this.btnDesconectar.TabIndex = 3;
            this.btnDesconectar.Text = "Desconectar";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // TabLobbyHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(96)))), ((int)(((byte)(63)))));
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.panelConectados);
            this.Controls.Add(this.panelJugadores);
            this.Controls.Add(this.panelChat);
            this.Controls.Add(this.btnEmpezar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TabLobbyHost";
            this.Load += new System.EventHandler(this.TabLobbyHost_Load);
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).EndInit();
            this.panelJugadores.ResumeLayout(false);
            this.panelJugadores.PerformLayout();
            this.panelColor.ResumeLayout(false);
            this.panelColor.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConectados)).EndInit();
            this.panelConectados.ResumeLayout(false);
            this.panelConectados.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEmpezar;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Panel panelJugadores;
        private System.Windows.Forms.DataGridView dataGridJugadores;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.Button btnCambioColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnC5;
        private System.Windows.Forms.Button btnC6;
        private System.Windows.Forms.Button btnC4;
        private System.Windows.Forms.Button btnC3;
        private System.Windows.Forms.Button btnC2;
        private System.Windows.Forms.Button btnC1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridConectados;
        private System.Windows.Forms.Panel panelConectados;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnInvitar;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.RichTextBox txtChat;
    }
}
