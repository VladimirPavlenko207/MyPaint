using Ex_Paint.DrawingShapes.Interfaces;
using Ex_Paint.HelperClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes {
    public class DrawingPoligon : BaseOperation, IFillable {
        Poligon poligon;

        public bool WithFill { get; set; } = false;

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            Image imageClone = (Image)workingImage.Clone();
            Graphics g = Graphics.FromImage(imageClone);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            poligon.AddPoint(e.Location);
            poligon.AddPoint(poligon.points[0]);
            DrawMyPoligon(g, poligon, pen, brush);
            g.Dispose();
            return imageClone;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            startPoint = e.Location;
            if (poligon == null) {
                workingImage = (Image)image.Clone();
                poligon = new Poligon(startPoint, startPoint, pen);
            }
            else {
                poligon.AddPoint(e.Location);
            }
            return image;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (poligon == null) return image;
            if (e.Button == MouseButtons.Left) {
                poligon.LastPoint = e.Location;
                Image imageClone = (Image)workingImage.Clone();

                Graphics g = Graphics.FromImage(imageClone);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                DrawMyPoligon(g, poligon, pen, brush);
                g.Dispose();
                return imageClone;
            }
            return image;
        }

        private void DrawMyPoligon(Graphics g, Poligon poligon, Pen pen, Brush brush) {
            GraphicsPath path = new GraphicsPath();
            for (int i = 0; i < poligon.points.Count - 1; i++) {
                if (poligon.isClosed) {
                    poligon.LastPoint = poligon.points[0];
                }
                path.AddLine(poligon.points[i], poligon.points[i + 1]);
            }
            if (poligon.isClosed) {
                path.CloseFigure();
            }
            if (WithFill)
                g.FillPath(brush, path);

            g.DrawPath(pen, path);
        }

        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (poligon == null) return image;

            Image imageClone = (Image)workingImage.Clone();
            Graphics g = Graphics.FromImage(imageClone);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawMyPoligon(g, poligon, pen, brush);
            if (poligon.isClosed) poligon = null;
            g.Dispose();
            return imageClone;
        }
    }
}
