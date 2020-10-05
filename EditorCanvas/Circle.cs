using System;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum CircleAlgorithms { Standard, Bresenham }
    public class Circle : Primitive
    {
        override public TypesOfPrimitive Type => TypesOfPrimitive.Circle;
        override public string Description => $"({Center.Item1}, {Center.Item2}), R={Radius}";
        public override string AlgorithmDescription => Algorithm switch
        {
            CircleAlgorithms.Bresenham => "Брезенхем",
            CircleAlgorithms.Standard => "Стандартный",
            _ => "Ошибка"
        };
        public (int, int) Center { get; }
        public int Radius { get; }
        public Circle((int, int) center, int radius, Color color, CircleAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            Center = center;
            Radius = radius;
        }
    }
}
