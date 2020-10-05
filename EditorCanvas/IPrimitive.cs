using System;
using System.ComponentModel;
using System.Windows.Media;

namespace EditorCanvasLib
{
    public enum TypesOfPrimitive 
    { 
        Segment, 
        Circle,
        Ellipse
    }
    public abstract class Primitive
    {
        public string TypeDescription => Type switch
        {
            TypesOfPrimitive.Segment => "Отрезок",
            TypesOfPrimitive.Circle => "Окружность",
            TypesOfPrimitive.Ellipse => "Эллипс",
            _ => "Ошибка"
        };
        abstract public TypesOfPrimitive Type { get; }
        abstract public string AlgorithmDescription { get; }
        public Enum Algorithm { get; set; }
        public Color Color { get; protected set; }
        abstract public string Description { get; }
    }
}
