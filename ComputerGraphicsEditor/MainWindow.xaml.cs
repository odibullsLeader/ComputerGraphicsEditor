using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EditorCanvasLib;

namespace ComputerGraphicsEditor
{
    public enum Types { Segment, Circle, Ellips }
    public enum SegmentAlgorithms { Standard, Bresenham }
    public interface IPrimitive
    {
        public Types Type { get; }
        public Enum Algorithm { get; }
        public Color Color { get; }
        public string Description { get; }

    }

    public class Segment : IPrimitive
    {

        public Types Type => Types.Segment;

        public Enum Algorithm { get; }

        public Color Color { get; }
        public string Description => $"({start.Item1}, {start.Item2}) - ({terminal.Item1}, {terminal.Item2})";
        
        private (int, int) start, terminal;
        public Segment((int, int) startPoint, (int, int) terminalPoint, Color color, SegmentAlgorithms algorithm)
        {
            Color = color;
            Algorithm = algorithm;
            start = startPoint;
            terminal = terminalPoint;
        }

    }

    public partial class MainWindow : Window
    {
        private readonly EditorCanvas canvas;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawSegmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxSegmentX1.Text == string.Empty ||
                TextBoxSegmentY1.Text == string.Empty ||
                TextBoxSegmentX2.Text == string.Empty ||
                TextBoxSegmentY2.Text == string.Empty)
            {
                MessageBox.Show("Остались незаполненные поля!");
                return;
            }
                int
                x1 = int.Parse(TextBoxSegmentX1.Text),
                y1 = int.Parse(TextBoxSegmentY1.Text),
                x2 = int.Parse(TextBoxSegmentX2.Text),
                y2 = int.Parse(TextBoxSegmentY2.Text);

        }

        private string textBeforeInput;
        private void TextBox_ValidateNumber(object sender, TextChangedEventArgs e)
        {
            int index = (sender as TextBox).CaretIndex;
            if (Regex.IsMatch((sender as TextBox).Text, @"^(-|\+)?\d*$"))
                return;
            (sender as TextBox).Text = textBeforeInput;
            (sender as TextBox).CaretIndex = index - 1;
        }

        private void TextBox_PreviewTextInput(object sender, EventArgs e)
        {
            textBeforeInput = (sender as TextBox).Text;
        }
    }
}
