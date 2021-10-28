using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.DrawingShapes.Interfaces {
    public interface IGraphical {
        Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush);
        Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush);
        Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush);
        Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush); 
    }
}
