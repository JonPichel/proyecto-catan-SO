
namespace cliente.Menu
{
    partial class TabMenuPrincipal
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMedia = new System.Windows.Forms.Label();
            this.dataGridPartidas = new System.Windows.Forms.DataGridView();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.btnMedia = new System.Windows.Forms.Button();
            this.btnPartidas = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridJugadores = new System.Windows.Forms.DataGridView();
            this.btnCrearLobby = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPartidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(11, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "Mis partidas";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(37, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 35);
            this.label1.TabIndex = 12;
            this.label1.Text = "Menú del usuario";
            // 
            // lblMedia
            // 
            this.lblMedia.AutoSize = true;
            this.lblMedia.BackColor = System.Drawing.Color.Transparent;
            this.lblMedia.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMedia.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMedia.Location = new System.Drawing.Point(37, 315);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(136, 19);
            this.lblMedia.TabIndex = 13;
            this.lblMedia.Text = "Puntuación media: ";
            // 
            // dataGridPartidas
            // 
            this.dataGridPartidas.AllowUserToAddRows = false;
            this.dataGridPartidas.AllowUserToDeleteRows = false;
            this.dataGridPartidas.AllowUserToResizeColumns = false;
            this.dataGridPartidas.AllowUserToResizeRows = false;
            this.dataGridPartidas.BackgroundColor = System.Drawing.Color.White;
            this.dataGridPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPartidas.GridColor = System.Drawing.Color.White;
            this.dataGridPartidas.Location = new System.Drawing.Point(11, 61);
            this.dataGridPartidas.Name = "dataGridPartidas";
            this.dataGridPartidas.ReadOnly = true;
            this.dataGridPartidas.Size = new System.Drawing.Size(385, 118);
            this.dataGridPartidas.TabIndex = 11;
            this.dataGridPartidas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPartidas_CellClick);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.Sienna;
            this.btnDesconectar.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnDesconectar.FlatAppearance.BorderSize = 2;
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesconectar.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDesconectar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDesconectar.Location = new System.Drawing.Point(37, 261);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(150, 40);
            this.btnDesconectar.TabIndex = 10;
            this.btnDesconectar.Text = "Cerrar Sesión";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // btnMedia
            // 
            this.btnMedia.BackColor = System.Drawing.Color.Sienna;
            this.btnMedia.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnMedia.FlatAppearance.BorderSize = 2;
            this.btnMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMedia.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMedia.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnMedia.Location = new System.Drawing.Point(37, 169);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(150, 40);
            this.btnMedia.TabIndex = 9;
            this.btnMedia.Text = "Puntuación media";
            this.btnMedia.UseVisualStyleBackColor = false;
            this.btnMedia.Click += new System.EventHandler(this.btnMedia_Click);
            // 
            // btnPartidas
            // 
            this.btnPartidas.BackColor = System.Drawing.Color.Sienna;
            this.btnPartidas.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnPartidas.FlatAppearance.BorderSize = 2;
            this.btnPartidas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPartidas.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPartidas.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPartidas.Location = new System.Drawing.Point(37, 120);
            this.btnPartidas.Name = "btnPartidas";
            this.btnPartidas.Size = new System.Drawing.Size(150, 40);
            this.btnPartidas.TabIndex = 8;
            this.btnPartidas.Text = "Mis Partidas";
            this.btnPartidas.UseVisualStyleBackColor = false;
            this.btnPartidas.Click += new System.EventHandler(this.btnPartidas_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(399, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 22);
            this.label7.TabIndex = 14;
            this.label7.Text = "En línea";
            // 
            // dataGridJugadores
            // 
            this.dataGridJugadores.AllowUserToAddRows = false;
            this.dataGridJugadores.AllowUserToDeleteRows = false;
            this.dataGridJugadores.BackgroundColor = System.Drawing.Color.White;
            this.dataGridJugadores.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridJugadores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridJugadores.GridColor = System.Drawing.Color.White;
            this.dataGridJugadores.Location = new System.Drawing.Point(399, 49);
            this.dataGridJugadores.Name = "dataGridJugadores";
            this.dataGridJugadores.ReadOnly = true;
            this.dataGridJugadores.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridJugadores.Size = new System.Drawing.Size(81, 130);
            this.dataGridJugadores.TabIndex = 11;
            // 
            // btnCrearLobby
            // 
            this.btnCrearLobby.BackColor = System.Drawing.Color.Sienna;
            this.btnCrearLobby.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnCrearLobby.FlatAppearance.BorderSize = 2;
            this.btnCrearLobby.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrearLobby.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCrearLobby.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCrearLobby.Location = new System.Drawing.Point(37, 215);
            this.btnCrearLobby.Name = "btnCrearLobby";
            this.btnCrearLobby.Size = new System.Drawing.Size(150, 40);
            this.btnCrearLobby.TabIndex = 19;
            this.btnCrearLobby.Text = "Crear Lobby";
            this.btnCrearLobby.UseVisualStyleBackColor = false;
            this.btnCrearLobby.Click += new System.EventHandler(this.btnCrearLobby_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dataGridPartidas);
            this.panel1.Controls.Add(this.dataGridJugadores);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(203, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 214);
            this.panel1.TabIndex = 20;
            // 
            // TabMenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCrearLobby);
            this.Controls.Add(this.lblMedia);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnMedia);
            this.Controls.Add(this.btnPartidas);
            this.Name = "TabMenuPrincipal";
            this.Load += new System.EventHandler(this.TabMenuPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPartidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridJugadores)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMedia;
        private System.Windows.Forms.DataGridView dataGridPartidas;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Button btnMedia;
        private System.Windows.Forms.Button btnPartidas;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridJugadores;
        private System.Windows.Forms.Button btnCrearLobby;
        private System.Windows.Forms.Panel panel1;
    }
}
