using Common.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.HelperClasses {
    public static class ScaleSetter {
        static List<MyLine> ScaleH;
        static List<MyLine> ScaleV;

        internal static Bitmap GetScaleH(int width, int height, float indent,
            Color backColor, Color scaleColor) {
           
            InitScaleH(width, height, indent);
            Bitmap bitmap = new Bitmap(width, height);
            ToScaleBitmap(ScaleH, bitmap, backColor, scaleColor);
            PrintNumH(bitmap, indent, Brushes.Navy);
            return bitmap;
        }

        private static void PrintNumH(Bitmap bitmap, float indent, Brush brush) {
            Font font = new Font("", indent * 1.4f, FontStyle.Regular);
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < ScaleH.Count; ++i) {
                if (i % 10 == 0) {
                    string str = (i * 10).ToString();
                    g.DrawString(str, font, Brushes.Navy, ScaleH[i].end.X + indent, 0);
                }
            }
            g.Dispose();
        }

        internal static Bitmap GetScaleV(int width, int height, float indent,
            Color backColor, Color scaleColor) {
           
            InitScaleV(width, height, indent);
            Bitmap bitmap = new Bitmap(width, height);
            ToScaleBitmap(ScaleV, bitmap, backColor, scaleColor);
            PrintNumV(bitmap, indent, Brushes.Navy);
            return bitmap;
        }

        private static void PrintNumV(Bitmap bitmap, float indent, Brush navy) {
            Font font = new Font("", indent * 1.4f, FontStyle.Regular);
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < ScaleV.Count; ++i) {
                if (i % 10 == 0) {
                    string str = (i * 10).ToString();
                    SizeF strSize = g.MeasureString(str, font);
                    RectangleF r = new RectangleF(
                        ScaleV[i].end.X, ScaleV[i].end.Y + indent, strSize.Width, strSize.Height * 2);
                    using (StringFormat string_format = new StringFormat()) {
                        string_format.Alignment = StringAlignment.Far;
                        string_format.LineAlignment = StringAlignment.Near;

                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        DrawFigures.DrawSidewaysText(g, font, Brushes.Navy, r, string_format, str, -90);
                    }
                }
            }
            g.Dispose();
        }

        private static void InitScaleV(int width, int height, float indent) {
            ScaleV = new List<MyLine>();
            int cellSize = (int)(indent * 2);

            float x1 = width;
            float y1 = indent * 1.7f;
            float numOfCells = height;
            for (int y = 0; y <= numOfCells; ++y) {
                float k = 0.9f;
                if (y % 10 != 0) k /= 3;

                MyLine tmpLine = new MyLine(
                  new Point((int)x1, (int)(y * cellSize + y1)),
                  new Point((int)(x1 - x1 * k), (int)(y * cellSize + y1)));
                ScaleV.Add(tmpLine);
            }
        }

        private static void ToScaleBitmap(
            List<MyLine> scale, Bitmap bitmap, Color backColor, Color scaleColor) {

            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(backColor);
            Pen p = new Pen(scaleColor);
            for (int i = 0; i < scale.Count; ++i) {
                g.DrawLine(p, scale[i].begin, scale[i].end);
            }
            g.Dispose();
        }

        private static void InitScaleH(int width, int height, float indent) {
            ScaleH = new List<MyLine>();
            int cellSize = (int)(indent * 2);

            float x1 = indent * 1.7f;
            float y1 = height;
            float numOfCells = width / cellSize;
            for (int x = 0; x <= numOfCells; ++x) {
                float k = 0.9f;
                if (x % 10 != 0) k /= 3;

                MyLine tmpLine = new MyLine(
                   new Point((int)(x * cellSize + x1), (int)y1),
                   new Point((int)(x * cellSize + x1), (int)(y1 - y1 * k)));

                ScaleH.Add(tmpLine);
            }
        }

        internal static Image CursorLocationOnScaleH(Image image, int x) {
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Transparent);
            g.DrawLine(new Pen(Color.Red), x, 0, x, image.Height);
            g.Dispose();
            return image;
        }

        //internal static void CursorLocationOnScaleH(Image image, int x) {
        //    Graphics g = Graphics.FromImage(image);
        //    g.Clear(Color.Transparent);
        //    g.DrawLine(new Pen(Color.Red), x, 0, x, image.Height);
        //    g.Dispose();
        //}

        internal static Image CursorLocationOnScaleV(Image image, int y) {
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Transparent);
            g.DrawLine(new Pen(Color.Red), 0, y, image.Height, y);
            g.Dispose();
            return image;
        }
        
    }
}
