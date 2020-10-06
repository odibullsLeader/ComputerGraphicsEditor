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
    public partial class MainWindow : Window
    {
        private EditorCanvas canvas;
        public MainWindow()
        {
            InitializeComponent();
        }
        private static readonly Regex EmptyNumberRegex = new Regex(@"^(\+|-)?$");
        private bool HasEmptyNumbers(params string[] numbers)
        {
            foreach (string number in numbers)
                if (EmptyNumberRegex.IsMatch(number))
                    return true;
            return false;
        }
        private void DrawSegmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyNumbers(
                TextBoxSegmentX1.Text,
                TextBoxSegmentY1.Text,
                TextBoxSegmentX2.Text,
                TextBoxSegmentY2.Text))
            {
                MessageBox.Show("Остались незаполненные поля!");
                return;
            }
            Segment segment = new Segment(
                (int.Parse(TextBoxSegmentX1.Text), int.Parse(TextBoxSegmentY1.Text)),
                (int.Parse(TextBoxSegmentX2.Text), int.Parse(TextBoxSegmentY2.Text)),
                ColorPickerSegmentColor.SelectedColor.Value,
                (SegmentAlgorithms)Enum.Parse(typeof(SegmentAlgorithms), (ComboBoxSegmentAlgotithm.SelectedItem as ComboBoxItem).Tag.ToString()));

            canvas.AddPrimitive(segment);
            ListViewDisplayedElements.Items.Refresh();
        }
        private void DrawCircleButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyNumbers(
                TextBoxCircleX.Text,
                TextBoxCircleY.Text,
                TextBoxCircleR.Text))
            {
                MessageBox.Show("Остались незаполненные поля!");
                return;
            }
            Circle circle = new Circle(
                (int.Parse(TextBoxCircleX.Text), int.Parse(TextBoxCircleY.Text)),
                int.Parse(TextBoxCircleR.Text),
                ColorPickerCircleColor.SelectedColor.Value,
                (CircleAlgorithms)Enum.Parse(typeof(CircleAlgorithms), (ComboBoxCircleAlgotithm.SelectedItem as ComboBoxItem).Tag.ToString()));

            canvas.AddPrimitive(circle);
            ListViewDisplayedElements.Items.Refresh();
        }
        private void DrawEllipseButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasEmptyNumbers(
               TextBoxEllipseX.Text,
               TextBoxEllipseY.Text,
               TextBoxEllipseRx.Text,
               TextBoxEllipseRy.Text))
            {
                MessageBox.Show("Остались незаполненные поля!");
                return;
            }
            EditorCanvasLib.Ellipse ellipse = new EditorCanvasLib.Ellipse(
                (int.Parse(TextBoxEllipseX.Text), int.Parse(TextBoxEllipseY.Text)),
                int.Parse(TextBoxEllipseRx.Text), int.Parse(TextBoxEllipseRy.Text),
                ColorPickerEllipseColor.SelectedColor.Value,
                (EllipseAlgorithms)Enum.Parse(typeof(EllipseAlgorithms), (ComboBoxEllipseAlgotithm.SelectedItem as ComboBoxItem).Tag.ToString()));

            canvas.AddPrimitive(ellipse);
            ListViewDisplayedElements.Items.Refresh();
        }
        private string textBeforeInput;
        private void TextBox_Validate(TextBox textBox, Regex pattern)
        {
            int index = textBox.CaretIndex;
            if (pattern.IsMatch(textBox.Text))
                return;
            textBox.Text = textBeforeInput;
            textBox.CaretIndex = Math.Max(0, index - 1);
        }

        private static readonly Regex NumberRegex = new Regex(@"^(-|\+)?\d*$");
        private static readonly Regex PositiveNumberRegex = new Regex(@"^(\+?[1-9]\d*)?$");
        private void TextBox_ValidateNumber(object sender, TextChangedEventArgs e) =>
            TextBox_Validate(sender as TextBox, NumberRegex);
        private void TextBox_ValidatePositiveNumber(object sender, TextChangedEventArgs e) =>
            TextBox_Validate(sender as TextBox, PositiveNumberRegex);
        private void TextBox_PreviewTextInput(object sender, EventArgs e) =>
            textBeforeInput = (sender as TextBox).Text;

        private void CanvasResultingImage_Loaded(object sender, RoutedEventArgs e)
        {
            canvas = new EditorCanvas(CanvasResultingImage);
            ListViewDisplayedElements.ItemsSource = canvas.DrawnPrimitives;
        }
    }
}
