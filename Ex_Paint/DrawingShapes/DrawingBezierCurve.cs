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
    public class DrawingBezierCurve : BaseOperation {
        BezierCurve bezierCurve;

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            startPoint = e.Location;
           
            if (bezierCurve == null) {
                workingImage = image;
                bezierCurve = new BezierCurve(startPoint, startPoint);
            }
            else {
                bezierCurve.count++;
                if (bezierCurve.count < bezierCurve.MaxCount) {
                    bezierCurve.p2 = e.Location;
                }
                else {
                    bezierCurve.p3 = e.Location;
                }
            }
            return image;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                finishPoint = e.Location;
                Image imageClone = (Image)workingImage.Clone();

                Graphics g = Graphics.FromImage(imageClone);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (bezierCurve == null) return image;
                switch (bezierCurve.count) {
                    case 0:
                        bezierCurve.p3 = bezierCurve.p4 = e.Location;
                        break;
                    case 1:
                        bezierCurve.p2 = e.Location;
                        break;
                    case 2:
                        bezierCurve.p3 = e.Location;
                        break;
                }
                g.DrawBezier(pen, bezierCurve.p1, bezierCurve.p2, bezierCurve.p3, bezierCurve.p4);
                g.Dispose();
                return imageClone;
            }
            return image;
        }

        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            Image imageClone = (Image)workingImage.Clone();
            Graphics g = Graphics.FromImage(imageClone);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (bezierCurve != null) {
                g.DrawBezier(pen, bezierCurve.p1, bezierCurve.p2, bezierCurve.p3, bezierCurve.p4);
            }

            if (bezierCurve?.count == bezierCurve?.MaxCount)
                // удаляем объект bezierCurve
                bezierCurve = null;
            g.Dispose();
            return imageClone;
        }
    }
}
