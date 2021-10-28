using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.HelperClasses {
    public static class Grid {
        internal static Bitmap CreateGridImage(int width, int height, float cell) {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.LightBlue);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            pen.DashOffset = 20;
            for (int y = 0; y < height / cell; y++)  {
                g.DrawLine(pen, 0, y * cell, width, y * cell);
            }
            for (int x = 0; x < width / cell; x++) {
                g.DrawLine(pen, x * cell, 0, x * cell, height);
            }
            g.Dispose();
            return bitmap;
        }
    }
}
