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
        Pen blue = Pens.Blue;

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

            //Window
            int XWmin = 50, YWmin = 50, XWmax = 200, YWmax = 170;
            g.DrawRectangle(Boundary, XWmin, YWmin, XWmax, YWmax);

            //ViewPort
            int XVmin = 70, YVmin = 70, XVmax = 110, YVmax = 200;
            g.DrawRectangle(Boundary, C.X + XVmin, YVmin, XVmax, YVmax);

            //Point in my Window
            int xw = 90; int yw = 140;
            g.DrawRectangle(red, xw, yw, 1, 1);
            
            //Point in the ViewPort
            PointF p = WindowtoViewport(xw, yw, XWmax, YWmax, XWmin, YWmin, XVmax, YVmax, XVmin, YVmin);
            g.DrawRectangle(blue, C.X + p.X, p.Y, 1, 1);
        }

        static PointF WindowtoViewport(int xw, int yw,
                                       int XWmax, int YWmax,
                                       int XWmin, int YWmin,
                                       int XVmax, int YVmax,
                                       int XVmin, int YVmin)
        {
            float sx = (float)(XVmax - XVmin) / (XWmax - XWmin);
            float sy = (float)(YVmax - YVmin) / (YWmax - YWmin);

            int xv = (int)(XVmin + ((xw - XWmin) * sx));
            int yv = (int)(YVmin + ((yw - YWmin) * sy));

            return new PointF(xv, yv);
        }
    }
}
