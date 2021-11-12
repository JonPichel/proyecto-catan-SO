
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
            // TabTablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCasilla);
            this.Controls.Add(this.btnCiudad);
            this.Controls.Add(this.btnCarretera);
            this.Controls.Add(this.btnPoblado);
            this.Name = "TabTablero";
            this.Load += new System.EventHandler(this.TabTablero_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPoblado;
        private System.Windows.Forms.Button btnCarretera;
        private System.Windows.Forms.Button btnCiudad;
        private System.Windows.Forms.Button btnCasilla;
    }
}
