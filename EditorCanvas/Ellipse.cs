using System;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum EllipseAlgorithms { Standard, Bresenham }
    public class Ellipse : Primitive
    {
        protected override TypesOfPrimitive Type => TypesOfPrimitive.Ellipse;
        public override string Description => $"O({Center.Item1}, {Center.Item2}), a = {RadiusX}, b = {RadiusY}";
        public override string AlgorithmDescription => Algorithm switch
        {
            EllipseAlgorithms.Bresenham => "Брезенхем",
            EllipseAlgorithms.Standard => "Стандартный",
            _ => "Ошибка"
        };
        public (int, int) Center { get; }
        public int RadiusX { get; }
        public int RadiusY { get; }
        public Ellipse((int, int) center, int radiusX, int radiusY, Color color, EllipseAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }
    }
}
