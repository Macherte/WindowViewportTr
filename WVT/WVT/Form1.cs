using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WVT
{
    public partial class Form1 : Form
    {
        Graphics g;
        PointF A, B, C, D, E, F;
        Pen Boundary;
        Pen red = Pens.Red;
        public Form1()
        {
            InitializeComponent();
            A = new PointF(0, 0);
            B = new PointF(0, Canvas.Height);
            C = new PointF(Canvas.Width / 2, 0);
            D = new PointF(Canvas.Width / 2, Canvas.Height);
            E = new PointF(Canvas.Width, 0);
            F = new PointF(Canvas.Width, Canvas.Height);

            Boundary = new Pen(Color.Black, 2f);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawLine(Boundary, C, D);

            float wxmin = 50; float wymin = 50; float wxmax = 250; float wymax = 100;
            g.DrawRectangle(Boundary, wxmin, wymin, wxmax, wymax);
            float vxmin = C.X + 50; float vymin = 50; float vxmax = C.X + 250; float vymax = 100;
            g.DrawRectangle(Boundary, vxmin, vymin, 250, vymax);
            
            float wx = 80; float wy = 110;
            g.DrawRectangle(red, wx, wy, 1, 1);
            //float a = (wx-wxmin) / (wxmax - wxmin);
            float dwx = wxmax - wxmin; float dwy = wymax - wymin;
            float dvx = vxmax - vxmin; float dvy = vymax - vymin;

            float vx = (dvx / dwx) * wx + (vxmin - wxmin * (dvx / dwx));
            float vy = (dvy / dwy) * wy + (vymin - wymin * (dvy / dwy));

            g.DrawRectangle(red, vx, vy, 1, 1);
        }
    }
}
