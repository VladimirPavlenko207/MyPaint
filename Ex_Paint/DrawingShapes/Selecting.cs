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
    public class Selecting : BaseOperation {

        DraggedFragment drFragment;
        Image subImage;

        public override Image WhenMouseDoubleClick(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            return image;
        }

        public override Image WhenMouseDown(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (e.Button == MouseButtons.Left) {
                startPoint = e.Location;
                if (drFragment != null) {
                    //юзер кликнул мышью мимо фрагмента?
                    if (!drFragment.Rect.Contains(e.Location)) {
                        //уничтожаем фрагмент
                        drFragment = null;
                        workingImage = subImage;
                        return subImage;
                    }
                }
                else {
                    workingImage = subImage = image;
                }
            }
            return image;
        }

        public override Image WhenMouseMove(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (image == null) return image;
            if (e.Button == MouseButtons.Left) {
                
                finishPoint = e.Location;
                Rectangle rec = GetRectangle(startPoint, finishPoint);
                Image imageClone = (Image)workingImage.Clone();

                Graphics g = Graphics.FromImage(imageClone);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // юзер тянет фрагмент ?
                if (drFragment != null)  {

                    //сдвигаем фрагмент
                    drFragment.Location.Offset(e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                    startPoint = e.Location;
                    drFragment.ToDrag(g, workingImage);
                }
                else {
                    //если выделена область
                    if (startPoint != finishPoint)
                        //рисуем рамку
                        ControlPaint.DrawFocusRectangle(g, GetRectangle(startPoint, finishPoint));
                }
                //сдвигаем выделенную область
                finishPoint = e.Location;
                return imageClone;
            }
            return image;
        }
       
        public override Image WhenMouseUp(Image image, MouseEventArgs e, Pen pen, Brush brush) {
            if (workingImage == null) return image;
                       
            subImage = new Bitmap(workingImage);
            Graphics g = Graphics.FromImage(subImage);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // пользователь выделил фрагмент и отпустил мышь?
            if (startPoint != finishPoint)  {
                //создаем DraggedFragment
                var rect = GetRectangle(startPoint, finishPoint);
                drFragment = new DraggedFragment(new Pen(brush).Color) { SourceRect = rect, Location = rect.Location };
            }
            else {
                //пользователь сдвинул фрагмент и отпуcтил мышь?
                if (drFragment != null) {
                    subImage = new Bitmap(workingImage);
                    //фиксируем изменения в исходном изображении
                    drFragment.Fix(subImage);
                }
                return subImage;
            }
            return image;
        }
    }
}
