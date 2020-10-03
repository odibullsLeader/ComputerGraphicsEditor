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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EditorCanvas canvas = new EditorCanvas();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            long
                x1 = long.Parse(TextBoxX1.Text.Replace('.', ',')),
                y1 = long.Parse(TextBoxY1.Text.Replace('.', ',')),
                x2 = long.Parse(TextBoxX2.Text.Replace('.', ',')),
                y2 = long.Parse(TextBoxY2.Text.Replace('.', ','));
            DisplayedElements.ItemsSource = new List<ElementRecord>() { new ElementRecord(Types.Segment, new List<int>() { 1, 2, 3 }, true) };
        }
        private enum Types { Segment, Circle, Ellips }
        private readonly struct ElementRecord
        {
            public ElementRecord(Types elementType, List<int> parameters, bool isMyAlgorythm)
            {
                Type = elementType switch
                {
                    Types.Segment => "Отрезок",
                    Types.Circle => "Окружность",
                    Types.Ellips => "Эллипс",
                    _ => "ERROR"
                };
                IsMyAlgorythm = isMyAlgorythm ? "Алексеев В Д" : "Брезенхем Дж Э";
                Parameters = parameters[0] + parameters.Skip(1).Aggregate<int, string>("", (str, param) => str + ", " + param);
            }
            public string Type { get; }
            public string Parameters { get; }
            public string IsMyAlgorythm { get; }
        }

        private void TextBox_ValidateNumber(object sender, TextChangedEventArgs e)
        {
        }
    }
}
