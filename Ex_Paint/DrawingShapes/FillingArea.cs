using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes {
    public class FillingArea : BaseOperation {

        Stack<Point> stackForFilling;

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
           
            if (e.Button == MouseButtons.Left) {
                if (stackForFilling != null) return image;
                 ToFill(image, e, pen.Color);
            }
            else if (e.Button == MouseButtons.Right)  {
                if (stackForFilling != null) return image;
                Color c = (brush as SolidBrush).Color;
                ToFill(image, e, c);
            }
            return image;
        }

        private void ToFill(Image image, MouseEventArgs e, Color newColor) {
            Color color = ((Bitmap)image).GetPixel(e.X, e.Y);
            stackForFilling = new Stack<Point>();
            stackForFilling.Push(e.Location); 
            while (stackForFilling.Count > 0) {
                Point p = stackForFilling.Pop();
                ((Bitmap)image).SetPixel(p.X, p.Y, newColor);
                int x = p.X;
                int y = p.Y;
                p = new Point(x + 1, y);
                if (IsCoordCorrect(p, color, image)) { stackForFilling.Push(p); }
                p = new Point(x - 1, y);
                if (IsCoordCorrect(p, color, image)) { stackForFilling.Push(p); }
                p = new Point(x, y + 1);
                if (IsCoordCorrect(p, color, image)) { stackForFilling.Push(p); }
                p = new Point(x, y - 1);
                if (IsCoordCorrect(p, color, image)) { stackForFilling.Push(p); }
            }
            stackForFilling = null;
        }

        private bool IsCoordCorrect(Point p, Color color, Image image) {
            if (p.X < 0 || p.X >= image.Width) return false;
            if (p.Y < 0 || p.Y >= image.Height) return false;
            return ((Bitmap)image).GetPixel(p.X, p.Y) == color;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }
    }
}
