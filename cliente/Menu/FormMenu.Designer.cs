
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
            this.btnPartidas = new System.Windows.Forms.Button();
            this.btnMedia = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.dataGridPartidas = new System.Windows.Forms.DataGridView();
            this.lblMedia = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPartidas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPartidas
            // 
            this.btnPartidas.Location = new System.Drawing.Point(34, 30);
            this.btnPartidas.Name = "btnPartidas";
            this.btnPartidas.Size = new System.Drawing.Size(125, 48);
            this.btnPartidas.TabIndex = 0;
            this.btnPartidas.Text = "MIS PARTIDAS";
            this.btnPartidas.UseVisualStyleBackColor = true;
            // 
            // btnMedia
            // 
            this.btnMedia.Location = new System.Drawing.Point(34, 84);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(125, 48);
            this.btnMedia.TabIndex = 1;
            this.btnMedia.Text = "Puntuacion media";
            this.btnMedia.UseVisualStyleBackColor = true;
            this.btnMedia.Click += new System.EventHandler(this.btnMedia_Click);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.Location = new System.Drawing.Point(34, 138);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(125, 48);
            this.btnDesconectar.TabIndex = 2;
            this.btnDesconectar.Text = "Desconectarse";
            this.btnDesconectar.UseVisualStyleBackColor = true;
            // 
            // dataGridPartidas
            // 
            this.dataGridPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPartidas.Location = new System.Drawing.Point(289, 12);
            this.dataGridPartidas.Name = "dataGridPartidas";
            this.dataGridPartidas.RowTemplate.Height = 25;
            this.dataGridPartidas.Size = new System.Drawing.Size(240, 150);
            this.dataGridPartidas.TabIndex = 3;
            // 
            // lblMedia
            // 
            this.lblMedia.AutoSize = true;
            this.lblMedia.Location = new System.Drawing.Point(289, 165);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(116, 15);
            this.lblMedia.TabIndex = 4;
            this.lblMedia.Text = "Puntuación media: 0";
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblMedia);
            this.Controls.Add(this.dataGridPartidas);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnMedia);
            this.Controls.Add(this.btnPartidas);
            this.Name = "FormMenu";
            this.Text = "FormMenu";
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
    }
}