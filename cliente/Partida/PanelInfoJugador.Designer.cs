
namespace cliente.Partida
{
    partial class PanelInfoJugador
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
            this.lblCaballeros = new System.Windows.Forms.Label();
            this.lblCarreteras = new System.Windows.Forms.Label();
            this.lblRecursos = new System.Windows.Forms.Label();
            this.lblDesarrollo = new System.Windows.Forms.Label();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCaballeros
            // 
            this.lblCaballeros.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCaballeros.ForeColor = System.Drawing.Color.White;
            this.lblCaballeros.Image = global::cliente.Properties.Resources.IconoCaballeros;
            this.lblCaballeros.Location = new System.Drawing.Point(3, 26);
            this.lblCaballeros.Name = "lblCaballeros";
            this.lblCaballeros.Size = new System.Drawing.Size(30, 30);
            this.lblCaballeros.TabIndex = 5;
            this.lblCaballeros.Text = "0";
            this.lblCaballeros.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCarreteras
            // 
            this.lblCarreteras.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCarreteras.ForeColor = System.Drawing.Color.White;
            this.lblCarreteras.Image = global::cliente.Properties.Resources.IconoCarreteras;
            this.lblCarreteras.Location = new System.Drawing.Point(39, 26);
            this.lblCarreteras.Name = "lblCarreteras";
            this.lblCarreteras.Size = new System.Drawing.Size(30, 30);
            this.lblCarreteras.TabIndex = 6;
            this.lblCarreteras.Text = "0";
            this.lblCarreteras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRecursos
            // 
            this.lblRecursos.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRecursos.ForeColor = System.Drawing.Color.White;
            this.lblRecursos.Image = global::cliente.Properties.Resources.IconoRecursos;
            this.lblRecursos.Location = new System.Drawing.Point(75, 26);
            this.lblRecursos.Name = "lblRecursos";
            this.lblRecursos.Size = new System.Drawing.Size(30, 30);
            this.lblRecursos.TabIndex = 7;
            this.lblRecursos.Text = "0";
            this.lblRecursos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDesarrollo
            // 
            this.lblDesarrollo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDesarrollo.ForeColor = System.Drawing.Color.White;
            this.lblDesarrollo.Image = global::cliente.Properties.Resources.IconoDesarrollo;
            this.lblDesarrollo.Location = new System.Drawing.Point(111, 26);
            this.lblDesarrollo.Name = "lblDesarrollo";
            this.lblDesarrollo.Size = new System.Drawing.Size(30, 30);
            this.lblDesarrollo.TabIndex = 8;
            this.lblDesarrollo.Text = "0";
            this.lblDesarrollo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPuntos
            // 
            this.lblPuntos.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPuntos.ForeColor = System.Drawing.Color.White;
            this.lblPuntos.Image = global::cliente.Properties.Resources.IconoPuntos;
            this.lblPuntos.Location = new System.Drawing.Point(147, 11);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(45, 45);
            this.lblPuntos.TabIndex = 9;
            this.lblPuntos.Text = "0";
            this.lblPuntos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNombre.ForeColor = System.Drawing.Color.White;
            this.lblNombre.Location = new System.Drawing.Point(3, 3);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(73, 21);
            this.lblNombre.TabIndex = 10;
            this.lblNombre.Text = "Nombre";
            // 
            // PanelInfoJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkRed;
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblPuntos);
            this.Controls.Add(this.lblDesarrollo);
            this.Controls.Add(this.lblRecursos);
            this.Controls.Add(this.lblCarreteras);
            this.Controls.Add(this.lblCaballeros);
            this.Name = "PanelInfoJugador";
            this.Size = new System.Drawing.Size(195, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaballeros;
        private System.Windows.Forms.Label lblCarreteras;
        private System.Windows.Forms.Label lblRecursos;
        private System.Windows.Forms.Label lblDesarrollo;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.Label lblNombre;
    }
}
