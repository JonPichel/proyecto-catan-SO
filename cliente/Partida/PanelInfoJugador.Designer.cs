
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelInfoJugador));
            this.lblCaballeros = new System.Windows.Forms.Label();
            this.lblCarreteras = new System.Windows.Forms.Label();
            this.lblRecursos = new System.Windows.Forms.Label();
            this.lblDesarrollo = new System.Windows.Forms.Label();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.panelColor = new System.Windows.Forms.Panel();
            this.lblMadera = new System.Windows.Forms.Label();
            this.lblLadrillo = new System.Windows.Forms.Label();
            this.lblOveja = new System.Windows.Forms.Label();
            this.lblTrigo = new System.Windows.Forms.Label();
            this.lblPiedra = new System.Windows.Forms.Label();
            this.panelColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaballeros
            // 
            this.lblCaballeros.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCaballeros.ForeColor = System.Drawing.Color.White;
            this.lblCaballeros.Image = ((System.Drawing.Image)(resources.GetObject("lblCaballeros.Image")));
            this.lblCaballeros.Location = new System.Drawing.Point(3, 24);
            this.lblCaballeros.Name = "lblCaballeros";
            this.lblCaballeros.Size = new System.Drawing.Size(39, 30);
            this.lblCaballeros.TabIndex = 5;
            this.lblCaballeros.Text = "0";
            this.lblCaballeros.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCarreteras
            // 
            this.lblCarreteras.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCarreteras.ForeColor = System.Drawing.Color.White;
            this.lblCarreteras.Image = ((System.Drawing.Image)(resources.GetObject("lblCarreteras.Image")));
            this.lblCarreteras.Location = new System.Drawing.Point(39, 24);
            this.lblCarreteras.Name = "lblCarreteras";
            this.lblCarreteras.Size = new System.Drawing.Size(37, 30);
            this.lblCarreteras.TabIndex = 6;
            this.lblCarreteras.Text = "0";
            this.lblCarreteras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRecursos
            // 
            this.lblRecursos.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRecursos.ForeColor = System.Drawing.Color.White;
            this.lblRecursos.Image = ((System.Drawing.Image)(resources.GetObject("lblRecursos.Image")));
            this.lblRecursos.Location = new System.Drawing.Point(75, 24);
            this.lblRecursos.Name = "lblRecursos";
            this.lblRecursos.Size = new System.Drawing.Size(37, 30);
            this.lblRecursos.TabIndex = 7;
            this.lblRecursos.Text = "0";
            this.lblRecursos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDesarrollo
            // 
            this.lblDesarrollo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDesarrollo.ForeColor = System.Drawing.Color.White;
            this.lblDesarrollo.Image = ((System.Drawing.Image)(resources.GetObject("lblDesarrollo.Image")));
            this.lblDesarrollo.Location = new System.Drawing.Point(111, 24);
            this.lblDesarrollo.Name = "lblDesarrollo";
            this.lblDesarrollo.Size = new System.Drawing.Size(39, 30);
            this.lblDesarrollo.TabIndex = 8;
            this.lblDesarrollo.Text = "0";
            this.lblDesarrollo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPuntos
            // 
            this.lblPuntos.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPuntos.ForeColor = System.Drawing.Color.White;
            this.lblPuntos.Image = ((System.Drawing.Image)(resources.GetObject("lblPuntos.Image")));
            this.lblPuntos.Location = new System.Drawing.Point(144, 9);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(48, 45);
            this.lblPuntos.TabIndex = 9;
            this.lblPuntos.Text = "0";
            this.lblPuntos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNombre.ForeColor = System.Drawing.Color.White;
            this.lblNombre.Location = new System.Drawing.Point(3, 1);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(73, 21);
            this.lblNombre.TabIndex = 10;
            this.lblNombre.Text = "Nombre";
            // 
            // panelColor
            // 
            this.panelColor.BackColor = System.Drawing.Color.DarkRed;
            this.panelColor.Controls.Add(this.lblRecursos);
            this.panelColor.Controls.Add(this.lblNombre);
            this.panelColor.Controls.Add(this.lblCaballeros);
            this.panelColor.Controls.Add(this.lblPuntos);
            this.panelColor.Controls.Add(this.lblCarreteras);
            this.panelColor.Controls.Add(this.lblDesarrollo);
            this.panelColor.Location = new System.Drawing.Point(0, 0);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(195, 60);
            this.panelColor.TabIndex = 11;
            // 
            // lblMadera
            // 
            this.lblMadera.BackColor = System.Drawing.Color.Gray;
            this.lblMadera.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMadera.ForeColor = System.Drawing.Color.White;
            this.lblMadera.Location = new System.Drawing.Point(3, 63);
            this.lblMadera.Name = "lblMadera";
            this.lblMadera.Size = new System.Drawing.Size(32, 32);
            this.lblMadera.TabIndex = 8;
            this.lblMadera.Text = "0";
            this.lblMadera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLadrillo
            // 
            this.lblLadrillo.BackColor = System.Drawing.Color.Gray;
            this.lblLadrillo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLadrillo.ForeColor = System.Drawing.Color.White;
            this.lblLadrillo.Location = new System.Drawing.Point(38, 63);
            this.lblLadrillo.Name = "lblLadrillo";
            this.lblLadrillo.Size = new System.Drawing.Size(32, 32);
            this.lblLadrillo.TabIndex = 8;
            this.lblLadrillo.Text = "0";
            this.lblLadrillo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOveja
            // 
            this.lblOveja.BackColor = System.Drawing.Color.Gray;
            this.lblOveja.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblOveja.ForeColor = System.Drawing.Color.White;
            this.lblOveja.Location = new System.Drawing.Point(73, 63);
            this.lblOveja.Name = "lblOveja";
            this.lblOveja.Size = new System.Drawing.Size(32, 32);
            this.lblOveja.TabIndex = 8;
            this.lblOveja.Text = "0";
            this.lblOveja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrigo
            // 
            this.lblTrigo.BackColor = System.Drawing.Color.Gray;
            this.lblTrigo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrigo.ForeColor = System.Drawing.Color.White;
            this.lblTrigo.Location = new System.Drawing.Point(108, 63);
            this.lblTrigo.Name = "lblTrigo";
            this.lblTrigo.Size = new System.Drawing.Size(32, 32);
            this.lblTrigo.TabIndex = 8;
            this.lblTrigo.Text = "0";
            this.lblTrigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPiedra
            // 
            this.lblPiedra.BackColor = System.Drawing.Color.Gray;
            this.lblPiedra.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPiedra.ForeColor = System.Drawing.Color.White;
            this.lblPiedra.Location = new System.Drawing.Point(143, 63);
            this.lblPiedra.Name = "lblPiedra";
            this.lblPiedra.Size = new System.Drawing.Size(32, 32);
            this.lblPiedra.TabIndex = 8;
            this.lblPiedra.Text = "0";
            this.lblPiedra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelInfoJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblPiedra);
            this.Controls.Add(this.lblTrigo);
            this.Controls.Add(this.lblOveja);
            this.Controls.Add(this.lblLadrillo);
            this.Controls.Add(this.lblMadera);
            this.Controls.Add(this.panelColor);
            this.Name = "PanelInfoJugador";
            this.Size = new System.Drawing.Size(195, 100);
            this.panelColor.ResumeLayout(false);
            this.panelColor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCaballeros;
        private System.Windows.Forms.Label lblCarreteras;
        private System.Windows.Forms.Label lblRecursos;
        private System.Windows.Forms.Label lblDesarrollo;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.Label lblMadera;
        private System.Windows.Forms.Label lblLadrillo;
        private System.Windows.Forms.Label lblOveja;
        private System.Windows.Forms.Label lblTrigo;
        private System.Windows.Forms.Label lblPiedra;
    }
}
