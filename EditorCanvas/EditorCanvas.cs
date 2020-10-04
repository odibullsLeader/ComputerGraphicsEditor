using System;
using System.Windows.Controls;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace EditorCanvasLib
{
    public class EditorCanvas
    {
        public List<Primitive> DrawnPrimitives { get; }
        private int actualScale = 1;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Scale
        {
            get { return actualScale; }
            set
            {
                if (actualScale == value) return;
                actualScale = value;
                RefreshScaledBitmap();
            }
        }
        private readonly Bitmap basicBitmap, scaledBitmap;
        public EditorCanvas(System.Windows.Controls.Canvas canvas)
        {
            Width = (int)canvas.ActualHeight;
            Height = (int) canvas.ActualHeight;
            basicBitmap = new Bitmap(Width, Height);
            scaledBitmap = new Bitmap(Width, Height);
            DrawnPrimitives = new List<Primitive>();
        }
        public void AddPrimitive(Primitive primitive) =>
            DrawnPrimitives.Add(primitive);
        public void DrawStandardSegment(Pen pen, Point firstPoint, Point secondPoint)
        {
            using (Graphics g = Graphics.FromImage(basicBitmap))
                g.DrawLine(pen, firstPoint, secondPoint);
            RefreshScaledBitmap();
        }
        public void DrawStandardCircle(Pen pen, Point center, int radius)
        {
            using (Graphics g = Graphics.FromImage(basicBitmap))
                g.DrawEllipse(pen, new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius));
            RefreshScaledBitmap();
        }
        public void DrawStandardEllipse(Pen pen, Point topLeftCorner, Point bottomRightCorner)
        {
            using (Graphics g = Graphics.FromImage(basicBitmap))
                g.DrawEllipse(pen, new Rectangle(topLeftCorner.X, topLeftCorner.Y, bottomRightCorner.X, bottomRightCorner.Y));
            RefreshScaledBitmap();
        }

        private void SetPixelIfItIsInBounds(int x, int y, Pen pen)
        {
            if ((0 <= x && x < Width && 0 <= y && y < Height))
                basicBitmap.SetPixel(x, y, pen.Color);
        }

        public void DrawMySegment(Pen pen, Point firstPoint, Point secondPoint)
        {
            int x = firstPoint.X, y = firstPoint.Y;
            int dx = Math.Abs(secondPoint.X - firstPoint.X);
            int s1 = Math.Sign(secondPoint.X - firstPoint.X);
            int dy = Math.Abs(secondPoint.Y - firstPoint.Y);
            int s2 = Math.Sign(secondPoint.Y - firstPoint.Y);
            int ch, e = 2 * dx - dy;
            if (dy > dx)
            {
                int t = dx;
                dx = dy;
                dy = t;
                ch = 1;
            }
            else ch = 0;
            for (int i = 0; i <= dx; ++i, e += 2 * dy)
            {
                SetPixelIfItIsInBounds(x, y, pen);
                for (; e >= 0; e -= 2 * dx)
                    if (ch == 1)
                        x += s1;
                    else
                        y += s2;
                if (ch == 1)
                    y += s2;
                else
                    x += s1;
            }
            RefreshScaledBitmap();
        }
        public void DrawMyCircle(Pen pen, Point center, int radius)
        {
            int x = 0,
                y = radius,
                d = 2 * (1 - radius);
            SetPixelIfItIsInBounds(x + center.X, y + center.Y, pen);
            SetPixelIfItIsInBounds(-x + center.X, y + center.Y, pen);
            SetPixelIfItIsInBounds(x + center.X, -y + center.Y, pen);
            SetPixelIfItIsInBounds(-x + center.X, -y + center.Y, pen);
            while (y > 0)
            {
                if (d < 0 && 2 * (d + y) - 1 <= 0)
                    d += 2 * ((++x) + 1);
                else if (d > 0 && 2 * (d - x) - 1 > 0)
                    d += 1 - 2 * (--y);
                else
                    d += 2 * ((++x) - (--y) + 1);
                SetPixelIfItIsInBounds(x + center.X, y + center.Y, pen);
                SetPixelIfItIsInBounds(-x + center.X, y + center.Y, pen);
                SetPixelIfItIsInBounds(x + center.X, -y + center.Y, pen);
                SetPixelIfItIsInBounds(-x + center.X, -y + center.Y, pen);
            }
            RefreshScaledBitmap();
        }
        //public void DrawMyEllipse(Pen pen, Point topLeftCorner, Point bottomRightCorner)
        //{

        //}

        private void RefreshScaledBitmap()
        {
            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
                for (int x = 0; x < Width / actualScale; ++x)
                    for (int y = 0; y < Height / actualScale; ++y)
                        g.FillRectangle(new SolidBrush(basicBitmap.GetPixel(x, y)), x * actualScale, y * actualScale, actualScale, actualScale);
            }
        }
        public void SaveImage(string fileName)
        {
            scaledBitmap.Save(fileName);
        }
    }
}
