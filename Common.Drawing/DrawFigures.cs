using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Drawing {
    public static class DrawFigures {

        public static void RoundedRectangle(Rectangle rec, Graphics g, Pen pen, Brush brush, bool withFill) {
            float a = rec.Width * 0.2f;
            float b = rec.Height * 0.2f;
            if (a < 0.001) a = 0.001f;
            if (b < 0.001) b = 0.001f;
            if (a > 20) a = 20;
            if (b > 20) b = 20;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new RectangleF(rec.Left, rec.Top, 2 * a, 2 * b), -90, -90);
            path.AddLine(rec.Left, rec.Top + b, rec.Left, rec.Bottom - b);
            path.AddArc(new RectangleF(rec.Left, rec.Bottom - 2 * b, 2 * a, 2 * b), 180, -90);
            path.AddLine(rec.Left + a, rec.Bottom, rec.Right - a, rec.Bottom);
            path.AddArc(new RectangleF(rec.Right - 2 * a, rec.Bottom - 2 * b, 2 * a, 2 * b), 90, -90);
            path.AddLine(rec.Right, rec.Bottom - b, rec.Right, rec.Top + b);
            path.AddArc(new RectangleF(rec.Right - 2 * a, rec.Top, 2 * a, 2 * b), 0, -90);
            path.AddLine(rec.Right - a, rec.Top, rec.Left + a, rec.Top);
            path.CloseFigure();
            try {
                if (withFill)
                    g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
            catch { }
        }

        public static void IsoscelesTriangle(Rectangle rec, Graphics g, Pen pen, Brush brush, bool withFill) {
            List<PointF> points = new List<PointF>();
            float x = rec.Right - rec.Width / 2;
            float y = rec.Top;
            points.Add(new PointF(x, y));

            x = rec.Left;
            y = rec.Bottom;
            points.Add(new PointF(x, y));
            x = rec.Right;
            y = rec.Bottom;
            points.Add(new PointF(x, y));
            GraphicsPath path = new GraphicsPath();

            for (int i = 0; i < points.Count; i++) {
                int j = i + 1;
                if (j == points.Count)
                    j = 0;
                path.AddLine(points[i], points[j]);
            }
            path.CloseFigure();
            if (withFill)
                g.FillPath(brush, path);
            g.DrawPath(pen, path);
        }

        public static void DrawSidewaysText(Graphics gr, Font font,
           Brush brush, RectangleF bounds, StringFormat string_format,
           string txt, int angle) {
            // Создаем поворот прямоугольника в начале координат.
            RectangleF rotated_bounds = new RectangleF(
                0, 0, bounds.Height, bounds.Width);
            // Поворот.
            gr.ResetTransform();
            gr.RotateTransform(angle);
            // Переместите, чтобы переместить прямоугольник в правильное положение.
            gr.TranslateTransform(bounds.Left, bounds.Bottom,
                MatrixOrder.Append);
            // Рисуем текст.
            gr.DrawString(txt, font, brush, rotated_bounds, string_format);
            gr.ResetTransform();
        }
    }
}
