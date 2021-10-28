using Ex_Paint.DrawingShapes.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes {
    public abstract class BaseOperation : IGraphical {

        protected Point startPoint, finishPoint;
        protected Image workingImage; 

        public abstract Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush);

        public abstract Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush);

        public abstract Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush);

        public abstract Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush);

        protected Rectangle GetRectangle(Point startPoint, Point finishPoint) {
            CoordNormalization(ref startPoint, ref finishPoint);
            Size size = GetSize(startPoint, finishPoint);
            return new Rectangle(startPoint, size);
        }

        protected Size GetSize(Point startPoint, Point finishPoint) {
            int hight = finishPoint.Y - startPoint.Y;
            int width = finishPoint.X - startPoint.X;
            return new Size(width, hight);
        }

        private void CoordNormalization(ref Point startPoint, ref Point finishPoint) {
            int x1, y1, x2, y2;
            x1 = Math.Min(startPoint.X, finishPoint.X);
            y1 = Math.Min(startPoint.Y, finishPoint.Y);
           
            x2 = Math.Max(startPoint.X, finishPoint.X);
            y2 = Math.Max(startPoint.Y, finishPoint.Y);

            startPoint.X = x1;
            startPoint.Y = y1;
            finishPoint.X = x2;
            finishPoint.Y = y2;
        }
    }
}
