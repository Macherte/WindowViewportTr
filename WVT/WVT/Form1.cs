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


            float vxmin = C.X + 100; float vymin = 30; float vxmax = 150; float vymax = 300;
            g.DrawRectangle(Boundary, vxmin, vymin, vxmax, vymax);
            /*
            PointF wx = new PointF(80, 110);
            g.DrawRectangle(Boundary, wx.X, wx.Y, 1, 1);
            float a = (wx.X - Wmin.X) / (Wmax.X - Wmin.X);*/
        }
    }
}
