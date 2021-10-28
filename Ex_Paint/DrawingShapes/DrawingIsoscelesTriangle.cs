using Common.Drawing;
using Ex_Paint.DrawingShapes.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes {
    public class DrawingIsoscelesTriangle : BaseOperation, IFillable {
        public bool WithFill { get; set; } = false;

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                startPoint = e.Location;
                workingImage = image;
            }
            return image;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                finishPoint = e.Location;
                Rectangle rec = GetRectangle(startPoint, finishPoint);
                Image imageClone = (Image)workingImage.Clone();

                Graphics g = Graphics.FromImage(imageClone);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                DrawFigures.IsoscelesTriangle(rec, g, pen, brush, WithFill);
                g.Dispose();
                return imageClone;
            }
            return image;
        }
        

        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }
    }
}
