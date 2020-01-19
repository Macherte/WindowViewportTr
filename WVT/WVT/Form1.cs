﻿using System;
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
        Pen PenBoundary, PenPolygon, PenInPolygon;
        Brush BrushFillPolygon;
        Rectangle Window, ViewPort;
        int WindowWidth, WindowHeight, ViewWidth, ViewHeight;
        float ax, ay;
        Point WindowLoc, ViewLoc;
        bool DrawPolygonMode, DrawWindow, MousePressed;
        List<PointF> Points, Transformed, WindowPolygonToFill, ViewPolygonToFill;

        byte LEFT = 1,
             RIGHT = 2,
             BOTTOM = 4,
             TOP = 8;

        public Form1()
        {
            InitializeComponent();
            A = new PointF(Canvas.Width / 2, 0);
            B = new PointF(Canvas.Width / 2, Canvas.Height);

            PenBoundary = new Pen(Color.Black, 3f);
            PenPolygon = new Pen(Color.Black, 2.5f);
            PenInPolygon = new Pen(Color.Green, 2.5f);
            BrushFillPolygon = new SolidBrush(Color.LightGreen);

            Points = new List<PointF>();
            Transformed = new List<PointF>();
            WindowPolygonToFill = new List<PointF>();
            ViewPolygonToFill = new List<PointF>();

            DrawPolygonMode = false;
            DrawWindow = true;
            MousePressed = false;
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawLine(PenBoundary, A, B);

            g.DrawRectangle(PenBoundary, Window);
            if (!DrawWindow)
                g.DrawRectangle(PenBoundary, ViewPort);

            if (DrawPolygonMode)
            {
                WindowPolygonToFill.Clear();
                ViewPolygonToFill.Clear();
                if (Points.Count == 1)
                {
                    g.DrawRectangle(PenPolygon, Points[0].X, Points[0].Y, 1, 1);
                    g.DrawRectangle(PenPolygon, Transformed[0].X, Transformed[0].Y, 1, 1);
                }
                else if (Points.Count == 2)
                {
                    g.DrawLine(PenPolygon, Points[0], Points[1]);
                    CohenSutherland(Points[0], Points[1], Window.X, Window.X + Window.Width, Window.Y, Window.Y + Window.Height);

                    g.DrawLine(PenPolygon, Transformed[0], Transformed[1]);
                    CohenSutherland(Transformed[0], Transformed[1], ViewPort.X, ViewPort.X + ViewPort.Width, ViewPort.Y, ViewPort.Y + ViewPort.Height);
                }
                else
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        
                        ax = (Points[i].X - Window.X) / ((Window.X + Window.Width) - Window.X);
                        ay = (Points[i].Y - Window.Y) / ((Window.Y + Window.Height) - Window.Y);
                        if (ax >= 0 && ax <= 1 && ay >= 0 && ay <= 1)
                        {
                            WindowPolygonToFill.Add(Points[i]);
                            ViewPolygonToFill.Add(Transformed[i]);
                        }

                        if (i == Points.IndexOf(Points.Last()))
                        {
                            g.DrawLine(PenPolygon, Points[i], Points[0]);
                            CohenSutherland(Points[i], Points[0], Window.X, Window.X + Window.Width, Window.Y, Window.Y + Window.Height, WindowPolygonToFill);

                            g.DrawLine(PenPolygon, Transformed[i], Transformed[0]);
                            CohenSutherland(Transformed[i], Transformed[0], ViewPort.X, ViewPort.X + ViewPort.Width, ViewPort.Y, ViewPort.Y + ViewPort.Height, ViewPolygonToFill);

                        }
                        else
                        {
                            g.DrawLine(PenPolygon, Points[i], Points[i + 1]);
                            CohenSutherland(Points[i], Points[i + 1], Window.X, Window.X + Window.Width, Window.Y, Window.Y + Window.Height, WindowPolygonToFill);


                            g.DrawLine(PenPolygon, Transformed[i], Transformed[i + 1]);
                            CohenSutherland(Transformed[i], Transformed[i + 1], ViewPort.X, ViewPort.X + ViewPort.Width, ViewPort.Y, ViewPort.Y + ViewPort.Height, ViewPolygonToFill);

                        }
                        g.FillPolygon(BrushFillPolygon, WindowPolygonToFill.ToArray());
                        g.FillPolygon(BrushFillPolygon, ViewPolygonToFill.ToArray());
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

        private void CohenSutherland(PointF Start, PointF End, int BOUND_LEFT, int BOUND_RIGHT, int BOUND_TOP, int BOUND_BOTTOM)
        {
            float x0 = Start.X, y0 = Start.Y, x1 = End.X, y1 = End.Y;
            byte outCode0 = OutCode(x0, y0, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM),
                 outCode1 = OutCode(x1, y1, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM),
                 outCode;
            bool accept = false;
            float x = 0, y = 0;

            while (true)
            {
                if ((outCode0 | outCode1) == 0)
                {
                    accept = true;
                    break;
                }
                else if ((outCode0 & outCode1) != 0)
                {
                    break;
                }
                else
                {
                    outCode = (outCode0 != 0) ? outCode0 : outCode1;

                    if ((outCode & LEFT) != 0)
                    {
                        x = BOUND_LEFT;
                        y = y0 + (y1 - y0) * (BOUND_LEFT - x0) / (x1 - x0);
                    }
                    else if ((outCode & RIGHT) != 0)
                    {
                        x = BOUND_RIGHT;
                        y = y0 + (y1 - y0) * (BOUND_RIGHT - x0) / (x1 - x0);
                    }
                    else if ((outCode & TOP) != 0)
                    {
                        x = x0 + (x1 - x0) * (BOUND_TOP - y0) / (y1 - y0);
                        y = BOUND_TOP;
                    }
                    else if ((outCode & BOTTOM) != 0)
                    {
                        x = x0 + (x1 - x0) * (BOUND_BOTTOM - y0) / (y1 - y0);
                        y = BOUND_BOTTOM;
                    }

                    if (outCode0 != 0)
                    {
                        x0 = x; y0 = y;
                        outCode0 = OutCode(x0, y0, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM);
                    }
                    else
                    {
                        x1 = x; y1 = y;
                        outCode1 = OutCode(x1, y1, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM);
                    }
                }
            }
            if (accept)
                g.DrawLine(PenInPolygon, x0, y0, x1, y1);
        }

        private void CohenSutherland(PointF Start, PointF End, int BOUND_LEFT, int BOUND_RIGHT, int BOUND_TOP, int BOUND_BOTTOM, List<PointF> list)
        {
            float x0 = Start.X, y0 = Start.Y, x1 = End.X, y1 = End.Y;
            byte outCode0 = OutCode(x0, y0, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM),
                 outCode1 = OutCode(x1, y1, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM),
                 outCode;
            bool accept = false;
            float x = 0, y = 0;

            while (true)
            {
                if ((outCode0 | outCode1) == 0)
                {
                    accept = true;
                    break;
                }
                else if ((outCode0 & outCode1) != 0)
                {
                    break;
                }
                else
                {
                    outCode = (outCode0 != 0) ? outCode0 : outCode1;

                    if ((outCode & LEFT) != 0)
                    {
                        x = BOUND_LEFT;
                        y = y0 + (y1 - y0) * (BOUND_LEFT - x0) / (x1 - x0);
                    }
                    else if ((outCode & RIGHT) != 0)
                    {
                        x = BOUND_RIGHT;
                        y = y0 + (y1 - y0) * (BOUND_RIGHT - x0) / (x1 - x0);
                    }
                    else if ((outCode & TOP) != 0)
                    {
                        x = x0 + (x1 - x0) * (BOUND_TOP - y0) / (y1 - y0);
                        y = BOUND_TOP;
                    }
                    else if ((outCode & BOTTOM) != 0)
                    {
                        x = x0 + (x1 - x0) * (BOUND_BOTTOM - y0) / (y1 - y0);
                        y = BOUND_BOTTOM;
                    }

                    if (outCode0 != 0)
                    {
                        x0 = x; y0 = y;
                        outCode0 = OutCode(x0, y0, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM);
                    }
                    else
                    {
                        x1 = x; y1 = y;
                        outCode1 = OutCode(x1, y1, BOUND_LEFT, BOUND_RIGHT, BOUND_TOP, BOUND_BOTTOM);
                    }
                }
            }
            PointF point1 = new PointF(x0, y0);
            PointF point2 = new PointF(x1, y1);
            if (!list.Contains(point1)) list.Add(point1);
            if (!list.Contains(point2)) list.Add(point2);

            if (accept)
                g.DrawLine(PenInPolygon, x0, y0, x1, y1);
        }

        private byte OutCode(float x, float y, int BOUND_LEFT, int BOUND_RIGHT, int BOUND_TOP, int BOUND_BOTTOM)
        {
            byte code = 0;

            if (x < BOUND_LEFT) code |= LEFT;
            else if (x > BOUND_RIGHT) code |= RIGHT;

            if (y < BOUND_TOP) code |= TOP;
            else if (y > BOUND_BOTTOM) code |= BOTTOM;

            return code;
        }
    }
}