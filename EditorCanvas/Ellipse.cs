using System;
using System.Drawing;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum EllipseAlgorithms { Standard, Bresenham }
    public class Ellipse : Primitive
    {
        public override TypesOfPrimitive Type => TypesOfPrimitive.Ellipse;
        public override string Description => 
            $"({TopLeftPoint.Item1}, {TopLeftPoint.Item2}), ({BottomRightPoint.Item1}, {BottomRightPoint.Item2})";
        public (int, int) TopLeftPoint => (Center.Item1 - RadiusX, Center.Item2 - RadiusY);
        public (int, int) BottomRightPoint => (Center.Item1 + RadiusX, Center.Item2 + RadiusY);
        public override string AlgorithmDescription => Algorithm switch
        {
            EllipseAlgorithms.Bresenham => "Брезенхем",
            EllipseAlgorithms.Standard => "Стандартный",
            _ => "Ошибка"
        };
        public (int, int) Center { get; }
        public int RadiusX { get; }
        public int RadiusY { get; }
        public Ellipse((int, int) center, int radiusX, int radiusY, System.Windows.Media.Color color, EllipseAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }
    }
}
