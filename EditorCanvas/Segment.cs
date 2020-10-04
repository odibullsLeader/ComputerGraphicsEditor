using System;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum SegmentAlgorithms { Standard, Bresenham }
    public class Segment : Primitive
    {
        override protected TypesOfPrimitive Type => TypesOfPrimitive.Segment;
        public override string AlgorithmDescription => Algorithm switch
        {
            SegmentAlgorithms.Bresenham => "Брезенхем",
            SegmentAlgorithms.Standard => "Стандартный",
            _ => "Ошибка"
        };
        override public string Description => $"({StartPoint.Item1}, {StartPoint.Item2}) - ({TerminalPoint.Item1}, {TerminalPoint.Item2})";
        protected (int, int) StartPoint { get; }
        protected (int, int) TerminalPoint { get; }
        public Segment((int, int) startPoint, (int, int) terminalPoint, Color color, SegmentAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            StartPoint = startPoint;
            TerminalPoint = terminalPoint;
        }
    }
}
