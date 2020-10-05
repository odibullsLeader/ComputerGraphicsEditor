using System;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum SegmentAlgorithms { Standard, Bresenham }
    public class Segment : Primitive
    {
        override public TypesOfPrimitive Type => TypesOfPrimitive.Segment;
        public override string AlgorithmDescription => Algorithm switch
        {
            SegmentAlgorithms.Bresenham => "Брезенхем",
            SegmentAlgorithms.Standard => "Стандартный",
            _ => "Ошибка"
        };
        override public string Description => $"({StartPoint.Item1}, {StartPoint.Item2}) - ({TerminalPoint.Item1}, {TerminalPoint.Item2})";
        public (int, int) StartPoint { get; }
        public (int, int) TerminalPoint { get; }
        public Segment((int, int) startPoint, (int, int) terminalPoint, Color color, SegmentAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            StartPoint = startPoint;
            TerminalPoint = terminalPoint;
        }
    }
}
