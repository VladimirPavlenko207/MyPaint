using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.HelperClasses {
    public class DraggedFragment {
        public Rectangle SourceRect;//прямоугольник фрагмента в исходном изображении
        public Point Location;//положение сдвинутого фрагмента

        //прямоугольник сдвинутого фрагмента
        public Rectangle Rect {
            get { return new Rectangle(Location, SourceRect.Size); }
        }

        Color backColor;

        public DraggedFragment(Color color) {
            backColor = color;
        }
       
        //фиксация изменений в исх изображении
        public void Fix(Image image) {

            using( var clone = (Image)image.Clone())
            using (var gr = Graphics.FromImage(image)) {

                ToDrag(gr, clone);
            }
        }

        public void ToDrag(Graphics g, Image image) {
            //если есть сдвигаемый фрагмент
                //рисуем вырезанное белое место
                g.SetClip(SourceRect);
                g.Clear(backColor);

                //рисуем сдвинутый фрагмент
                g.SetClip(Rect);
                g.DrawImage( image, Location.X - SourceRect.X, Location.Y - SourceRect.Y);
            
        }
    }
}
