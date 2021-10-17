
namespace cliente.Menu
{
    partial class FormInfoPartida
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfoPartida));
            this.lblIdPartida = new System.Windows.Forms.Label();
            this.dataGridResultadoPartida = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultadoPartida)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIdPartida
            // 
            this.lblIdPartida.AutoSize = true;
            this.lblIdPartida.BackColor = System.Drawing.Color.Transparent;
            this.lblIdPartida.Font = new System.Drawing.Font("Palatino Linotype", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lblIdPartida.Location = new System.Drawing.Point(32, 17);
            this.lblIdPartida.Name = "lblIdPartida";
            this.lblIdPartida.Size = new System.Drawing.Size(286, 35);
            this.lblIdPartida.TabIndex = 4;
            this.lblIdPartida.Text = "Datos de la partida: X";
            // 
            // dataGridResultadoPartida
            // 
            this.dataGridResultadoPartida.AllowUserToAddRows = false;
            this.dataGridResultadoPartida.AllowUserToDeleteRows = false;
            this.dataGridResultadoPartida.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResultadoPartida.Location = new System.Drawing.Point(32, 80);
            this.dataGridResultadoPartida.Name = "dataGridResultadoPartida";
            this.dataGridResultadoPartida.ReadOnly = true;
            this.dataGridResultadoPartida.Size = new System.Drawing.Size(534, 290);
            this.dataGridResultadoPartida.TabIndex = 5;
            this.dataGridResultadoPartida.Text = "dataGridView1";
            // 
            // FormInfoPartida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(609, 401);
            this.Controls.Add(this.dataGridResultadoPartida);
            this.Controls.Add(this.lblIdPartida);
            this.Name = "FormInfoPartida";
            this.Text = "FormInfoPartida";
            this.Load += new System.EventHandler(this.FormInfoPartida_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultadoPartida)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIdPartida;
        private System.Windows.Forms.DataGridView dataGridResultadoPartida;
    }
}