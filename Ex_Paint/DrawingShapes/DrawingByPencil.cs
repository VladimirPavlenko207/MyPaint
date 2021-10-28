using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes {
    public class DrawingByPencil : BaseOperation {

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                startPoint = e.Location;
            }
            return image;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                finishPoint = e.Location;
                Graphics g = Graphics.FromImage(image);
                g.DrawLine(pen, startPoint, e.Location);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                startPoint = e.Location;
                g.Dispose();
            }
            return image;
        }

        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }
    }
}
