using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVT
{
    class Edge
    {
        public Edge(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public Edge(PointF point1, PointF point2)
        {
            X1 = point1.X;
            Y1 = point1.Y;
            X2 = point2.X;
            Y2 = point2.Y;
        }

        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public PointF SP
        {
            get { return new PointF(X1, Y1); }
            set { X1 = value.X; Y1 = value.Y; }
        }
        public PointF EP
        {
            get { return new PointF(X2, Y2); }
            set { X2 = value.X; Y2 = value.Y; }
        }
    }
}
