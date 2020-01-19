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
        PointF A, B;
        Pen Boundary, red = Pens.Red, blue = Pens.Blue;
        Rectangle Window, ViewPort;
        int WindowWidth, WindowHeight, ViewWidth, ViewHeight;
        Point WindowLoc, ViewLoc;
        bool DrawPolygonMode, DrawWindow, MousePressed;
        List<PointF> Points;
        List<PointF> Transformed;


        public Form1()
        {
            InitializeComponent();
            A = new PointF(Canvas.Width / 2, 0);
            B = new PointF(Canvas.Width / 2, Canvas.Height);

            Boundary = new Pen(Color.Black, 2f);
            Points = new List<PointF>();
            Transformed = new List<PointF>();


            DrawPolygonMode = false;
            DrawWindow = true;
            MousePressed = false;
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawLine(Boundary, A, B);

            g.DrawRectangle(Boundary, Window);
            if (!DrawWindow)
                g.DrawRectangle(Boundary, ViewPort);

            if (DrawPolygonMode)
            {
                if (Points.Count == 1)
                {
                    g.DrawRectangle(red, Points[0].X, Points[0].Y, 1, 1);
                    g.DrawRectangle(blue, Transformed[0].X, Transformed[0].Y, 1, 1);
                }
                else if (Points.Count == 2)
                {
                    g.DrawLine(red, Points[0], Points[1]);
                    g.DrawLine(blue, Transformed[0], Transformed[1]);
                }
                else
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        if (i == Points.IndexOf(Points.Last()))
                        {
                            g.DrawLine(red, Points[i], Points[0]);
                            g.DrawLine(blue, Transformed[i], Transformed[0]);
                        }
                        else
                        {
                            g.DrawLine(red, Points[i], Points[i + 1]);
                            g.DrawLine(blue, Transformed[i], Transformed[i + 1]);
                        }
                    }
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            MousePressed = true;
            if (!DrawPolygonMode)
            {
                if (DrawWindow)
                {
                    if (e.X < A.X)
                        WindowLoc = e.Location;
                }
                else
                {
                    if (e.X > A.X)
                        ViewLoc = e.Location;
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (MousePressed)
            {
                if (!DrawPolygonMode)
                {
                    if (DrawWindow)
                    {
                        WindowWidth = e.X - Window.X;
                        WindowHeight = e.Y - Window.Y;
                        Window = new Rectangle(WindowLoc, new Size(WindowWidth, WindowHeight));
                        Canvas.Invalidate();
                    }
                    else
                    {
                        ViewWidth = e.X - ViewPort.X;
                        ViewHeight = e.Y - ViewPort.Y;
                        ViewPort = new Rectangle(ViewLoc, new Size(ViewWidth, ViewHeight));
                        Canvas.Invalidate();
                    }
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (DrawPolygonMode)
            {
                if (e.X > 0 && e.X < Canvas.Width && e.Y > 0 && e.Y < Canvas.Width)
                {
                    Points.Add(new PointF(e.X, e.Y));
                    Transformed.Add(WindowtoViewport((int)e.X, (int)e.Y, Window.X, Window.Y, Window.Width, Window.Height,
                                                                         ViewPort.X, ViewPort.Y, ViewPort.Width, ViewPort.Height));
                }
                Canvas.Invalidate();
            }
            else
            {
                if (DrawWindow) DrawWindow = false;
                else DrawPolygonMode = true;
            }
            MousePressed = false;
        }

        private PointF WindowtoViewport(int xw, int yw, int XWmin, int YWmin, int XWmax, int YWmax, int XVmin, int YVmin, int XVmax, int YVmax)
        {
            //float sx = (float)(XVmax - XVmin) / (XWmax - XWmin);
            //float sy = (float)(YVmax - YVmin) / (YWmax - YWmin);

            float sx = (float)XVmax / XWmax;
            float sy = (float)YVmax / YWmax;

            int xv = (int)(XVmin + (xw - XWmin) * sx);
            int yv = (int)(YVmin + (yw - YWmin) * sy);

            return new PointF(xv, yv);
        }
    }
}