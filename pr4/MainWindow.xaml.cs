using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace pr4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            FractalCanvas.Children.Clear(); // Очистка канваса

            string fractalType = ((ComboBoxItem)FractalComboBox.SelectedItem).Tag.ToString();
            int depth = (int)DepthSlider.Value;
            string colorName = ((ComboBoxItem)ColorComboBox.SelectedItem).Tag.ToString();
            SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFromString(colorName);

            switch (fractalType)
            {
                case "Tree":
                    DrawFractalTree(FractalCanvas, 300, 500, -90, 100, depth, brush);
                    break;
                case "Carpet":
                    DrawSierpinskiCarpet(FractalCanvas, 50, 50, 400, depth, brush);
                    break;
                case "Triangle":
                    DrawSierpinskiTriangle(FractalCanvas, 50, 500, 400, depth, brush);
                    break;
                case "Koch":
                    DrawKochCurve(FractalCanvas, 50, 200, 500, 200, depth, brush);
                    break;
                case "Cantor":
                    DrawCantorSet(FractalCanvas, 50, 400, 700, depth, brush);
                    break;
            }
        }

        private void DrawFractalTree(Canvas canvas, double x1, double y1, double angle, double length, int depth, SolidColorBrush brush)
        {
            if (depth == 0) return;

            double x2 = x1 + length * Math.Cos(angle * Math.PI / 180);
            double y2 = y1 + length * Math.Sin(angle * Math.PI / 180);
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = brush,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);

            // Добавляем случайное отклонение от ветра
            Random rand = new Random();
            double windAngle = rand.NextDouble() * 20 - 10;

            DrawFractalTree(canvas, x2, y2, angle - 20 + windAngle, length * 0.7, depth - 1, brush);
            DrawFractalTree(canvas, x2, y2, angle + 20 + windAngle, length * 0.7, depth - 1, brush);
        }

        private void DrawSierpinskiCarpet(Canvas canvas, double x, double y, double size, int depth, SolidColorBrush brush)
        {
            
            if (depth < 3)
            {
                Rectangle rectangle = new Rectangle
                {
                    Width = size,
                    Height = size,
                    Fill = brush
                };
                Canvas.SetLeft(rectangle, x);
                Canvas.SetTop(rectangle, y);
                canvas.Children.Add(rectangle);
                return;
            }

            double newSize = size / 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1) continue; 
                    DrawSierpinskiCarpet(canvas, x + i * newSize, y + j * newSize, newSize, depth - 1, brush);
                }
            }
        }

        private void DrawSierpinskiTriangle(Canvas canvas, double x, double y, double size, int depth, SolidColorBrush brush)
        {
            if (depth == 0)
            {
                Point p1 = new Point(x, y);
                Point p2 = new Point(x + size, y);
                Point p3 = new Point(x + size / 2, y - Math.Sqrt(3) * size / 2);
                Polygon triangle = new Polygon
                {
                    Points = new PointCollection { p1, p2, p3 },
                    Fill = brush
                };
                canvas.Children.Add(triangle);
                return;
            }

            DrawSierpinskiTriangle(canvas, x, y, size / 2, depth - 1, brush);
            DrawSierpinskiTriangle(canvas, x + size / 2, y, size / 2, depth - 1, brush);
            DrawSierpinskiTriangle(canvas, x + size / 4, y - Math.Sqrt(3) * size / 4, size / 2, depth - 1, brush);
        }

        private void DrawKochCurve(Canvas canvas, double x1, double y1, double x2, double y2, int depth, SolidColorBrush brush)
        {
            if (depth == 0)
            {
                Line line = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = brush,
                    StrokeThickness = 1
                };
                canvas.Children.Add(line);
                return;
            }

            
            double x3 = x1 + (x2 - x1) / 3;
            double y3 = y1 + (y2 - y1) / 3;
            double x4 = x1 + 2 * (x2 - x1) / 3;
            double y4 = y1 + 2 * (y2 - y1) / 3;

            // Вычисляем координаты точки на равностороннем треугольнике
            double x5 = x3 + (x4 - x3) / 2 + (y4 - y3) * Math.Sqrt(3) / 2;
            double y5 = y3 + (y4 - y3) / 2 - (x4 - x3) * Math.Sqrt(3) / 2;

            // Рекурсивно рисуем кривую Коха для каждой из частей
            DrawKochCurve(canvas, x1, y1, x3, y3, depth - 1, brush);
            DrawKochCurve(canvas, x3, y3, x5, y5, depth - 1, brush);
            DrawKochCurve(canvas, x5, y5, x4, y4, depth - 1, brush);
            DrawKochCurve(canvas, x4, y4, x2, y2, depth - 1, brush);
        }

        private void DrawCantorSet(Canvas canvas, double x1, double y1, double size, int depth, SolidColorBrush brush)
        {
            if (depth == 0)
            {
                Rectangle rectangle = new Rectangle
                {
                    Width = size,
                    Height = 5,
                    Fill = brush
                };
                Canvas.SetLeft(rectangle, x1);
                Canvas.SetTop(rectangle, y1);
                canvas.Children.Add(rectangle);
                return;
            }

            DrawCantorSet(canvas, x1, y1, size / 3, depth - 1, brush);
            DrawCantorSet(canvas, x1 + 2 * size / 3, y1, size / 3, depth - 1, brush);
        }
    }
}

    

        









