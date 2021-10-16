namespace Catan
{
    partial class Registro
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
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tBoxPwd = new System.Windows.Forms.TextBox();
            this.tBoxNickname = new System.Windows.Forms.TextBox();
            this.btnCrearCuenta = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxPwd2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckBoxTerminos = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Monotype Corsiva", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(33, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "Contraseña:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Monotype Corsiva", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(33, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 18);
            this.label4.TabIndex = 14;
            this.label4.Text = "Nombre de usuario:";
            // 
            // tBoxPwd
            // 
            this.tBoxPwd.Location = new System.Drawing.Point(33, 151);
            this.tBoxPwd.MaxLength = 20;
            this.tBoxPwd.Name = "tBoxPwd";
            this.tBoxPwd.Size = new System.Drawing.Size(130, 20);
            this.tBoxPwd.TabIndex = 13;
            // 
            // tBoxNickname
            // 
            this.tBoxNickname.Location = new System.Drawing.Point(33, 105);
            this.tBoxNickname.MaxLength = 20;
            this.tBoxNickname.Name = "tBoxNickname";
            this.tBoxNickname.Size = new System.Drawing.Size(130, 20);
            this.tBoxNickname.TabIndex = 12;
            // 
            // btnCrearCuenta
            // 
            this.btnCrearCuenta.BackColor = System.Drawing.Color.Sienna;
            this.btnCrearCuenta.FlatAppearance.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnCrearCuenta.FlatAppearance.BorderSize = 2;
            this.btnCrearCuenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrearCuenta.Font = new System.Drawing.Font("Monotype Corsiva", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrearCuenta.ForeColor = System.Drawing.SystemColors.Info;
            this.btnCrearCuenta.Location = new System.Drawing.Point(33, 244);
            this.btnCrearCuenta.Name = "btnCrearCuenta";
            this.btnCrearCuenta.Size = new System.Drawing.Size(130, 40);
            this.btnCrearCuenta.TabIndex = 11;
            this.btnCrearCuenta.Text = "Crear Cuenta";
            this.btnCrearCuenta.UseVisualStyleBackColor = false;
            this.btnCrearCuenta.Click += new System.EventHandler(this.btnCrearCuenta_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Monotype Corsiva", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "Repetir contraseña:";
            // 
            // tBoxPwd2
            // 
            this.tBoxPwd2.Location = new System.Drawing.Point(33, 196);
            this.tBoxPwd2.MaxLength = 20;
            this.tBoxPwd2.Name = "tBoxPwd2";
            this.tBoxPwd2.Size = new System.Drawing.Size(130, 20);
            this.tBoxPwd2.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Monotype Corsiva", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 39);
            this.label2.TabIndex = 18;
            this.label2.Text = "Registro del usuario";
            // 
            // ckBoxTerminos
            // 
            this.ckBoxTerminos.AutoSize = true;
            this.ckBoxTerminos.BackColor = System.Drawing.Color.Transparent;
            this.ckBoxTerminos.Font = new System.Drawing.Font("Monotype Corsiva", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckBoxTerminos.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ckBoxTerminos.Location = new System.Drawing.Point(33, 290);
            this.ckBoxTerminos.Name = "ckBoxTerminos";
            this.ckBoxTerminos.Size = new System.Drawing.Size(222, 20);
            this.ckBoxTerminos.TabIndex = 19;
            this.ckBoxTerminos.Text = "Acepto los términos y condiciones de uso";
            this.ckBoxTerminos.UseVisualStyleBackColor = false;
            // 
            // Registro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Catan.Properties.Resources.Fondo_menu;
            this.ClientSize = new System.Drawing.Size(609, 401);
            this.Controls.Add(this.ckBoxTerminos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBoxPwd2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tBoxPwd);
            this.Controls.Add(this.tBoxNickname);
            this.Controls.Add(this.btnCrearCuenta);
            this.Name = "Registro";
            this.Text = "Registro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBoxPwd;
        private System.Windows.Forms.TextBox tBoxNickname;
        private System.Windows.Forms.Button btnCrearCuenta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxPwd2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckBoxTerminos;
    }
}