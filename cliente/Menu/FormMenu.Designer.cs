
namespace cliente.Menu
{
    partial class FormMenu
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenu));
            this.btnPartidas = new System.Windows.Forms.Button();
            this.btnMedia = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.dataGridPartidas = new System.Windows.Forms.DataGridView();
            this.lblMedia = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPartidas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPartidas
            // 
            this.btnPartidas.BackColor = System.Drawing.Color.Sienna;
            this.btnPartidas.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnPartidas.FlatAppearance.BorderSize = 2;
            this.btnPartidas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPartidas.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPartidas.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPartidas.Location = new System.Drawing.Point(32, 97);
            this.btnPartidas.Name = "btnPartidas";
            this.btnPartidas.Size = new System.Drawing.Size(150, 40);
            this.btnPartidas.TabIndex = 0;
            this.btnPartidas.Text = "Mis Partidas";
            this.btnPartidas.UseVisualStyleBackColor = false;
            this.btnPartidas.Click += new System.EventHandler(this.btnPartidas_Click);
            // 
            // btnMedia
            // 
            this.btnMedia.BackColor = System.Drawing.Color.Sienna;
            this.btnMedia.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnMedia.FlatAppearance.BorderSize = 2;
            this.btnMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMedia.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMedia.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnMedia.Location = new System.Drawing.Point(32, 151);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(150, 40);
            this.btnMedia.TabIndex = 1;
            this.btnMedia.Text = "Puntuación media";
            this.btnMedia.UseVisualStyleBackColor = false;
            this.btnMedia.Click += new System.EventHandler(this.btnMedia_Click);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.Sienna;
            this.btnDesconectar.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnDesconectar.FlatAppearance.BorderSize = 2;
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesconectar.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDesconectar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDesconectar.Location = new System.Drawing.Point(32, 205);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(150, 40);
            this.btnDesconectar.TabIndex = 2;
            this.btnDesconectar.Text = "Cerrar Sesión";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // dataGridPartidas
            // 
            this.dataGridPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPartidas.Location = new System.Drawing.Point(213, 97);
            this.dataGridPartidas.Name = "dataGridPartidas";
            this.dataGridPartidas.Size = new System.Drawing.Size(371, 150);
            this.dataGridPartidas.TabIndex = 3;
            // 
            // lblMedia
            // 
            this.lblMedia.AutoSize = true;
            this.lblMedia.BackColor = System.Drawing.Color.Transparent;
            this.lblMedia.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMedia.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMedia.Location = new System.Drawing.Point(213, 250);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(143, 19);
            this.lblMedia.TabIndex = 4;
            this.lblMedia.Text = "Puntuación media: 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(32, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 35);
            this.label1.TabIndex = 4;
            this.label1.Text = "Menú del usuario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(213, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 22);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mis partidas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(30, 306);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ayuda:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(30, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(480, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "- Presione el botón \'Mis Partidas\' para cargar el registro de partidas del usuari" +
    "o";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(32, 342);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(383, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "- Presione una fila de la tabla para obtener datos de esa partida";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(32, 360);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(525, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "- Presione el botón \'Puntuación Media\' para mostrar el promedio de puntos del usu" +
    "ario";
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(609, 401);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMedia);
            this.Controls.Add(this.dataGridPartidas);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnMedia);
            this.Controls.Add(this.btnPartidas);
            this.Name = "FormMenu";
            this.Text = "FormMenu";
            this.Load += new System.EventHandler(this.FormMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPartidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPartidas;
        private System.Windows.Forms.Button btnMedia;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.DataGridView dataGridPartidas;
        private System.Windows.Forms.Label lblMedia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}