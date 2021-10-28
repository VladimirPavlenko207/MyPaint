using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.HelperClasses {
    public class Poligon {
        public List<Point> points = new List<Point>();
        public Point LastPoint {
            get {return points[points.Count - 1]; }
            set { points[points.Count - 1] = value; }
        }
        public bool isClosed = false;

        Rectangle startArea;  // область начальной точки
       
        public Poligon(Point p1, Point p2, Pen pen) {
            points.Add(p1);
            points.Add(p2);
            startArea = GetStartArea(p1, (int)pen.Width);
        }

        private Rectangle GetStartArea(Point p1, int width) {
            Point p = new Point(p1.X, p1.Y);
            p.Offset(-width, -width);
            return new Rectangle(p, new Size(2 * width, 2 * width));
        }

        public void AddPoint(Point p) {
            if (startArea.Contains(p)) {
                isClosed = true;
                points.Add(points[0]);
                return;
            }
            points.Add(p);
        }
    }
}
