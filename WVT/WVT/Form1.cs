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
        Pen PenBoundary, PenPolygon, PenInPolygon;
        Brush BrushFillPolygon;
        Rectangle Window, ViewPort;
        bool DrawPolygonMode, DrawWindow, MousePressed, ValidPoint;
        List<PointF> Points, Transformed;
        List<Edge> WindowClipPolygon;
        List<Edge> ViewClipPolygon;

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
            if (!DrawWindow) g.DrawRectangle(PenBoundary, ViewPort);

            if (DrawPolygonMode)
            {
                if (Points.Count == 1)
                {
                    g.DrawRectangle(PenPolygon, Points[0].X, Points[0].Y, 1, 1);
                    if (Transformed[0].X > A.X)
                        g.DrawRectangle(PenPolygon, Transformed[0].X, Transformed[0].Y, 1, 1);
                }
                else if (Points.Count > 0)
                {
                    g.DrawPolygon(PenPolygon, Points.ToArray());

                    PointF[] DrawnTransPoly = SutherlandHodgman(Transformed, new List<Edge>() { new Edge(A, B) }).ToArray();
                    if (DrawnTransPoly.Length > 1) g.DrawPolygon(PenPolygon, DrawnTransPoly);


                    PointF[] InnerPolygon = SutherlandHodgman(Points, WindowClipPolygon).ToArray();
                    PointF[] InnerTransPolygon = SutherlandHodgman(Transformed, ViewClipPolygon).ToArray();
                    if (InnerPolygon.Length > 1)
                    {
                        g.DrawPolygon(PenInPolygon, InnerPolygon);
                        g.FillPolygon(BrushFillPolygon, InnerPolygon);
                        g.DrawPolygon(PenInPolygon, InnerTransPolygon);
                        g.FillPolygon(BrushFillPolygon, InnerTransPolygon);
                    }
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            MousePressed = true;
            ValidPoint = false;
            if (!DrawPolygonMode)
            {
                if (DrawWindow)
                {
                    if (e.X < A.X)
                    {
                        Window = new Rectangle(e.Location, new Size(1, 1));
                        ValidPoint = true;
                    }
                }
                else
                {
                    if (e.X > A.X)
                    {
                        ViewPort = new Rectangle(e.Location, new Size(1, 1));
                        ValidPoint = true;
                    }
                }
            }
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (MousePressed)
            {
                if (!DrawPolygonMode && ValidPoint)
                {
                    if (DrawWindow)
                    {
                        if (e.X > 0 && e.X < A.X && e.Y > 0 && e.Y < Canvas.Height)
                        {
                            Window.Size = new Size(e.X - Window.X, e.Y - Window.Y);
                            Canvas.Invalidate();
                        }
                    }
                    else
                    {
                        if (e.X > A.X && e.X < Canvas.Width && e.Y > 0 && e.Y < Canvas.Height)
                        {
                            ViewPort.Size = new Size(e.X - ViewPort.X, e.Y - ViewPort.Y);
                            Canvas.Invalidate();
                        }
                    }
                }
            }
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (DrawPolygonMode)
            {
                if (e.X > 0 && e.X < A.X && e.Y > 0 && e.Y < Canvas.Width)
                {
                    Points.Add(new PointF(e.X, e.Y));
                    Transformed.Add(WindowtoViewport((int)e.X, (int)e.Y, Window.X, Window.Y, Window.Width, Window.Height,
                                                                         ViewPort.X, ViewPort.Y, ViewPort.Width, ViewPort.Height));
                    Canvas.Invalidate();
                }
            }
            else if (ValidPoint)
            {
                if (DrawWindow)
                {
                    if (Window.Width > 9 && Window.Height > 9)
                    {
                        DrawWindow = false;
                        WindowClipPolygon = new List<Edge>
                        {
                            new Edge(Window.X, Window.Y, Window.X, Window.Y + Window.Height),
                            new Edge(Window.X, Window.Y + Window.Height, Window.X + Window.Width, Window.Y + Window.Height),
                            new Edge(Window.X + Window.Width, Window.Y + Window.Height, Window.X + Window.Width, Window.Y),
                            new Edge(Window.X + Window.Width, Window.Y, Window.X, Window.Y)
                        };
                    }
                }
                else
                {
                    if (ViewPort.Width > 9 && ViewPort.Height > 9)
                    {
                        DrawPolygonMode = true;
                        ViewClipPolygon = new List<Edge>()
                        {
                            new Edge(ViewPort.X, ViewPort.Y, ViewPort.X, ViewPort.Y + ViewPort.Height),
                            new Edge(ViewPort.X, ViewPort.Y + ViewPort.Height, ViewPort.X + ViewPort.Width, ViewPort.Y + ViewPort.Height),
                            new Edge(ViewPort.X + ViewPort.Width, ViewPort.Y + ViewPort.Height, ViewPort.X + ViewPort.Width, ViewPort.Y),
                            new Edge(ViewPort.X + ViewPort.Width, ViewPort.Y, ViewPort.X, ViewPort.Y)
                        };
                    }
                }
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

        private List<PointF> SutherlandHodgman(List<PointF> SubjectPolygon, List<Edge> ClipPolygon)
        {
            List<PointF> OutputList = SubjectPolygon.ConvertAll(point => new PointF(point.X, point.Y));

            foreach (Edge ClipEdge in ClipPolygon)
            {
                List<PointF> InputList = OutputList.ConvertAll(point => new PointF(point.X, point.Y));
                OutputList.Clear();

                for (int i = 0; i < InputList.Count; i++)
                {
                    PointF CurrentPoint = InputList[i];
                    PointF PrevPoint = InputList[(i + InputList.Count - 1) % InputList.Count];
                    PointF? IntersectiongPoint = ComputeIntersection(new Edge(PrevPoint, CurrentPoint), ClipEdge);

                    if (IsLeft(CurrentPoint, ClipEdge))
                    {
                        if (!IsLeft(PrevPoint, ClipEdge))
                        {
                            OutputList.Add((PointF)IntersectiongPoint);
                        }
                        OutputList.Add(CurrentPoint);
                    }
                    else if (IsLeft(PrevPoint, ClipEdge))
                    {
                        OutputList.Add((PointF)IntersectiongPoint);
                    }
                }
            }
            return OutputList;
        }
        private PointF? ComputeIntersection(Edge edge1, Edge edge2)
        {
            float A1 = edge1.Y2 - edge1.Y1, B1 = edge1.X1 - edge1.X2, C1 = A1 * edge1.X1 + B1 * edge1.Y1;
            float A2 = edge2.Y2 - edge2.Y1, B2 = edge2.X1 - edge2.X2, C2 = A2 * edge2.X1 + B2 * edge2.Y1;

            float delta = A1 * B2 - A2 * B1;

            if (delta == 0)
                return null;

            float x = (B2 * C1 - B1 * C2) / delta;
            float y = (A1 * C2 - A2 * C1) / delta;
            return new PointF(x, y);
        }
        private bool IsLeft(PointF point, Edge edge)
        {
            return ((edge.X2 - edge.X1) * (point.Y - edge.Y1) - (edge.Y2 - edge.Y1) * (point.X - edge.X1)) <= 0;
        }

        private void ResetWnV_Click(object sender, EventArgs e)
        {
            WindowClipPolygon = null;
            ViewClipPolygon = null;
            Points.Clear();
            Transformed.Clear();
            DrawPolygonMode = false;
            DrawWindow = true;
            MousePressed = false;
            Window.Width = 0;
            Window.Height = 0;
            Canvas.Refresh();
        }
        private void ResetPoly_Click(object sender, EventArgs e)
        {
            Points.Clear();
            Transformed.Clear();
            Canvas.Refresh();
        }
    }
}
