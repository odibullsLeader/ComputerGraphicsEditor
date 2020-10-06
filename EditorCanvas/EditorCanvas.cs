using System;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.IO;

namespace EditorCanvasLib
{
    public class EditorCanvas
    {
        public List<Primitive> DrawnPrimitives { get; }
        public Canvas CanvasElement { get; private set; }
        public System.Windows.Controls.Image ImageElement { get; private set; }
        private System.Windows.Size CanvasSize { get => CanvasElement.RenderSize; }
        private int Width => (int)(CanvasSize.Width * PresentationSource.FromVisual(CanvasElement).CompositionTarget.TransformToDevice.M22);
        private int Height => (int)(CanvasSize.Height * PresentationSource.FromVisual(CanvasElement).CompositionTarget.TransformToDevice.M11);
        private Bitmap basicBitmap;

        private BitmapImage BM()
        {
            MemoryStream ms = new MemoryStream();
            basicBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        public EditorCanvas(Canvas canvas)
        {
            CanvasElement = canvas;
            CanvasElement.SizeChanged += (sender, eventArgs) => { };
            CanvasElement.Children.Add(ImageElement = new System.Windows.Controls.Image());
            basicBitmap = new Bitmap(Width, Height);
            DrawnPrimitives = new List<Primitive>();
        }

        public void AddPrimitive(Primitive primitive)
        {
            System.Drawing.Pen pen =
                new System.Drawing.Pen(System.Drawing.Color.FromArgb(
                                primitive.Color.A,
                                primitive.Color.R,
                                primitive.Color.G,
                                primitive.Color.B));
            switch (primitive.Type)
            {
                case TypesOfPrimitive.Segment:
                    {
                        Action<System.Drawing.Pen, System.Drawing.Point, System.Drawing.Point> draw =
                              primitive.Algorithm switch
                              {
                                  SegmentAlgorithms.Standard => DrawStandardSegment,
                                  SegmentAlgorithms.Bresenham => DrawMySegment,
                                  _ => throw new Exception()
                              };
                        draw(
                            pen,
                            new System.Drawing.Point(
                                (primitive as Segment).StartPoint.Item1,
                                (primitive as Segment).StartPoint.Item2),
                            new System.Drawing.Point(
                                (primitive as Segment).TerminalPoint.Item1,
                                (primitive as Segment).TerminalPoint.Item2));
                    }
                    break;
                case TypesOfPrimitive.Circle:
                    {
                        Action<System.Drawing.Pen, System.Drawing.Point, int> draw =
                              primitive.Algorithm switch
                              {
                                  CircleAlgorithms.Standard => DrawStandardCircle,
                                  CircleAlgorithms.Bresenham => DrawMyCircle,
                                  _ => throw new Exception()
                              };
                        draw(
                            pen,
                            new System.Drawing.Point(
                                (primitive as Circle).Center.Item1,
                                (primitive as Circle).Center.Item2),
                                (primitive as Circle).Radius);
                    }
                    break;
                case TypesOfPrimitive.Ellipse:
                    {
                        Action<System.Drawing.Pen, (int, int), (int, int)> draw =
                              primitive.Algorithm switch
                              {
                                  EllipseAlgorithms.Standard => DrawStandardEllipse,
                                  EllipseAlgorithms.Bresenham => DrawMyEllipse,
                                  _ => throw new Exception()
                              };
                        draw(
                            pen,
                            (
                                ((primitive as Ellipse).TopLeftPoint.Item1 + (primitive as Ellipse).BottomRightPoint.Item1) / 2,
                                ((primitive as Ellipse).TopLeftPoint.Item2 + (primitive as Ellipse).BottomRightPoint.Item2) / 2
                            ),
                            (
                                ((primitive as Ellipse).BottomRightPoint.Item1 - (primitive as Ellipse).TopLeftPoint.Item1) / 2,
                                ((primitive as Ellipse).BottomRightPoint.Item2 - (primitive as Ellipse).TopLeftPoint.Item2) / 2)
                            );
                    }
                    break;
            }
            ImageElement.Source = BM();
            DrawnPrimitives.Add(primitive);
        }
        public void DrawStandardSegment(System.Drawing.Pen Pen, System.Drawing.Point firstPoint, System.Drawing.Point secondPoint)
        {
            using Graphics g = Graphics.FromImage(basicBitmap);
            g.DrawLine(Pen, firstPoint, secondPoint);
        }
        public void DrawStandardCircle(System.Drawing.Pen Pen, System.Drawing.Point center, int radius)
        {
            using Graphics g = Graphics.FromImage(basicBitmap);
            g.DrawEllipse(Pen, new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius));
        }
        public void DrawStandardEllipse(System.Drawing.Pen Pen, (int, int) center, (int, int) radiusesXY)
        {
            (int a, int b) = radiusesXY;
            using Graphics g = Graphics.FromImage(basicBitmap);
            g.DrawEllipse(
                Pen,
                new Rectangle(
                    center.Item1 - a,
                    center.Item2 - b,
                    2 * a, 2 * b
                    )
                );
        }
        private void SetPixelIfItIsInBounds(int x, int y, System.Drawing.Pen Pen)
        {
            if ((0 <= x && x < Width && 0 <= y && y < Height))
                basicBitmap.SetPixel(x, y, Pen.Color);
        }

        public void DrawMySegment(System.Drawing.Pen Pen, System.Drawing.Point firstPoint, System.Drawing.Point secondPoint)
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
                SetPixelIfItIsInBounds(x, y, Pen);
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
        }
        public void DrawMyCircle(System.Drawing.Pen Pen, System.Drawing.Point center, int radius)
        {
            int x = 0,
                y = radius,
                d = 2 * (1 - radius);
            SetPixelIfItIsInBounds(x + center.X, y + center.Y, Pen);
            SetPixelIfItIsInBounds(-x + center.X, y + center.Y, Pen);
            SetPixelIfItIsInBounds(x + center.X, -y + center.Y, Pen);
            SetPixelIfItIsInBounds(-x + center.X, -y + center.Y, Pen);
            while (y > 0)
            {
                if (d < 0 && 2 * (d + y) - 1 <= 0)
                    d += 2 * ((++x) + 1);
                else if (d > 0 && 2 * (d - x) - 1 > 0)
                    d += 1 - 2 * (--y);
                else
                    d += 2 * ((++x) - (--y) + 1);
                SetPixelIfItIsInBounds(x + center.X, y + center.Y, Pen);
                SetPixelIfItIsInBounds(-x + center.X, y + center.Y, Pen);
                SetPixelIfItIsInBounds(x + center.X, -y + center.Y, Pen);
                SetPixelIfItIsInBounds(-x + center.X, -y + center.Y, Pen);
            }
        }
        public void DrawMyEllipse(System.Drawing.Pen Pen, (int, int) center, (int, int) radiusesXY)
        {
            (int a, int b) = radiusesXY;
            int x = 0,
                y = b,
                d = a * a + b * b - 2 * a * a * b;
            SetPixelIfItIsInBounds(x + center.Item1, y + center.Item2, Pen);
            SetPixelIfItIsInBounds(-x + center.Item1, y + center.Item2, Pen);
            SetPixelIfItIsInBounds(x + center.Item1, -y + center.Item2, Pen);
            SetPixelIfItIsInBounds(-x + center.Item1, -y + center.Item2, Pen);
            while (y > 0)
            {
                if (d > 0)
                {
                    if (d - b * b * (2 * x - 1) > 0)
                    {
                        d -= a * a * (2 * y - 3);
                        --y;
                    }
                    else
                    {
                        if (2 * d - b * b * (2 * x - 1) > 0)
                        {
                            d -= a * a * (2 * y - 3);
                            --y;
                        }
                        else
                        {
                            d += b * b * (2 * x + 3) - a * a * (2 * y - 3);
                            --y;
                            ++x;
                        }
                    }
                }
                else
                {
                    if (d + a * a * (2 * y - 1) > 0)
                    {
                        if (2 * d - a * a * (2 * y - 1) > 0)
                        {
                            d += b * b * (2 * x + 3) - a * a * (2 * y - 3);
                            --y;
                            ++x;
                        }
                        else
                        {
                            d += b * b * (2 * x + 3);
                            ++x;
                        }
                    }
                    else
                    {
                        d += b * b * (2 * x + 3);
                        ++x;
                    }
                }

                SetPixelIfItIsInBounds(x + center.Item1, y + center.Item2, Pen);
                SetPixelIfItIsInBounds(-x + center.Item1, y + center.Item2, Pen);
                SetPixelIfItIsInBounds(x + center.Item1, -y + center.Item2, Pen);
                SetPixelIfItIsInBounds(-x + center.Item1, -y + center.Item2, Pen);
            }
        }
    }
}
