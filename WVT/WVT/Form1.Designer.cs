namespace WVT
{
    partial class Form1
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
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.btn_ResetWnVp = new System.Windows.Forms.Button();
            this.btn_ResetPoly = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.Color.White;
            this.Canvas.Location = new System.Drawing.Point(13, 13);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(700, 400);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // btn_ResetWnVp
            // 
            this.btn_ResetWnVp.Location = new System.Drawing.Point(13, 419);
            this.btn_ResetWnVp.Name = "btn_ResetWnVp";
            this.btn_ResetWnVp.Size = new System.Drawing.Size(179, 23);
            this.btn_ResetWnVp.TabIndex = 1;
            this.btn_ResetWnVp.Text = "Reset Window and ViewPort";
            this.btn_ResetWnVp.UseVisualStyleBackColor = true;
            this.btn_ResetWnVp.Click += new System.EventHandler(this.ResetWnV_Click);
            // 
            // btn_ResetPoly
            // 
            this.btn_ResetPoly.Location = new System.Drawing.Point(198, 419);
            this.btn_ResetPoly.Name = "btn_ResetPoly";
            this.btn_ResetPoly.Size = new System.Drawing.Size(111, 23);
            this.btn_ResetPoly.TabIndex = 2;
            this.btn_ResetPoly.Text = "Reset Polygons";
            this.btn_ResetPoly.UseVisualStyleBackColor = true;
            this.btn_ResetPoly.Click += new System.EventHandler(this.ResetPoly_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(721, 449);
            this.Controls.Add(this.btn_ResetPoly);
            this.Controls.Add(this.btn_ResetWnVp);
            this.Controls.Add(this.Canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Window to ViewPort Transformation - Sutherland-Hodgman algorythm";
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Button btn_ResetWnVp;
        private System.Windows.Forms.Button btn_ResetPoly;
    }
}

