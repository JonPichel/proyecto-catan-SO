
namespace cliente.Menu
{
    partial class TabInfoPartida
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
            this.dataGridResultadoPartida = new System.Windows.Forms.DataGridView();
            this.lblIdPartida = new System.Windows.Forms.Label();
            this.btnAtras = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultadoPartida)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridResultadoPartida
            // 
            this.dataGridResultadoPartida.AllowUserToAddRows = false;
            this.dataGridResultadoPartida.AllowUserToDeleteRows = false;
            this.dataGridResultadoPartida.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResultadoPartida.Location = new System.Drawing.Point(45, 107);
            this.dataGridResultadoPartida.Name = "dataGridResultadoPartida";
            this.dataGridResultadoPartida.ReadOnly = true;
            this.dataGridResultadoPartida.Size = new System.Drawing.Size(534, 290);
            this.dataGridResultadoPartida.TabIndex = 7;
            this.dataGridResultadoPartida.Text = "dataGridView1";
            // 
            // lblIdPartida
            // 
            this.lblIdPartida.AutoSize = true;
            this.lblIdPartida.BackColor = System.Drawing.Color.Transparent;
            this.lblIdPartida.Font = new System.Drawing.Font("Palatino Linotype", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lblIdPartida.Location = new System.Drawing.Point(45, 44);
            this.lblIdPartida.Name = "lblIdPartida";
            this.lblIdPartida.Size = new System.Drawing.Size(286, 35);
            this.lblIdPartida.TabIndex = 6;
            this.lblIdPartida.Text = "Datos de la partida: X";
            // 
            // btnAtras
            // 
            this.btnAtras.Location = new System.Drawing.Point(504, 56);
            this.btnAtras.Name = "btnAtras";
            this.btnAtras.Size = new System.Drawing.Size(75, 23);
            this.btnAtras.TabIndex = 8;
            this.btnAtras.Text = "Atrás";
            this.btnAtras.UseVisualStyleBackColor = true;
            this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
            // 
            // TabInfoPartida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAtras);
            this.Controls.Add(this.dataGridResultadoPartida);
            this.Controls.Add(this.lblIdPartida);
            this.Name = "TabInfoPartida";
            this.Load += new System.EventHandler(this.TabInfoPartida_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultadoPartida)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridResultadoPartida;
        private System.Windows.Forms.Label lblIdPartida;
        private System.Windows.Forms.Button btnAtras;
    }
}
