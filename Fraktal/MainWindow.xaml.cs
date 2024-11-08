using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fraktal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Draw_click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            string input = Value.Text;
            int lvl;
            if (!int.TryParse(input, out lvl) || lvl < 1 || lvl > 10)
            {
                MessageBox.Show("Введите нужный интервал");
            }
            else
            {
                //цвет
                string selectedColor = ((ComboBoxItem)ColorComBox.SelectedItem).Content.ToString();
                Color color = GetColor(selectedColor);
                

                switch (FracComBox.Text)
                {
                    case "Фрактальное дерево":
                        DrawFracTree(DrawCanvas, lvl, color, 300, 500, -90, 100); //сделано
                        break;
                    case "Множество Кантора":
                        DrawFracKantor(DrawCanvas, lvl, color, 100, 200, 500); //сделано
                        break;
                    case "Кривая Коха":
                        DrawFracKoha(DrawCanvas, lvl, color, 200, 200, 500, 200); //сделано
                        break;
                    case "Треугольник Серпинского":
                        DrawFracTreygolnik(DrawCanvas, lvl, color, 200, 50, 400); //сделано
                        break;
                    case "Ковер Серпинского":
                        DrawFracKover(DrawCanvas, lvl, color, 150, 50, 400); //сделано
                        break;
                    
                    
                }
            }
        }

        private Color GetColor(string colorName)
        {
            return colorName switch
            {
                "Желтый" => Colors.Yellow,
                "Оранжевый" => Colors.Orange,
                "Розовый" => Colors.Pink,
                "Красный" => Colors.Red,
                "Фиолетовый" => Colors.Purple,
                "Зеленый" => Colors.Green,
                "Голубой" => Colors.Blue,
            };
        }

        //rанвас, количество, цвет, начальные точки, угол, длина
        //фрактальное дерево
        private void DrawFracTree(Canvas canvas, int lvl, Color color, double x, double y, double angle, double length) 
        {
                double xEnd = x + (Math.Cos(angle * Math.PI / 180) * length);
                double yEnd = y + (Math.Sin(angle * Math.PI / 180) * length);
            if ( lvl == 0)
            {
                
                return;
            }
            Line line = new Line
            {
                Stroke = new SolidColorBrush(color),
                X1 = x,
                Y1 = y,
                X2 = xEnd,
                Y2 = yEnd,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);

            DrawFracTree(canvas, lvl - 1, color, xEnd, yEnd, angle - 20, length * 0.7);
            DrawFracTree(canvas, lvl - 1, color, xEnd, yEnd, angle + 20, length * 0.7);
        }
        //Кантор
        private void DrawFracKantor(Canvas canvas, int lvl, Color color, double x, double y, double length)
        {
            if (lvl == 0)
            {
                Rectangle rect = new Rectangle
                {
                    Width = length,
                    Fill = new SolidColorBrush(color),
                    Height = 5
                };
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                canvas.Children.Add(rect);
                return;
            }
            DrawFracKantor(canvas, lvl - 1, color, x, y, length / 3);
            DrawFracKantor(canvas, lvl - 1, color, x + 2 * length / 3, y, length / 3);
        }

        //Коха
        private void DrawFracKoha(Canvas canvas, int lvl, Color color, double x1, double y1, double x2, double y2)
        {
            if (lvl == 1 || lvl == 0)
            {
                Line line = new Line
                {
                    Stroke = new SolidColorBrush(color),
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
                return;
            }
            
            //вычисляем точки
            double x3 = x1 + (x2 - x1) / 3;
            double y3 = y1 + (y2 - y1) / 3;
            double x4 = x1 + 2 * (x2 - x1) / 3;
            double y4 = y1 + 2 * (y2 - y1) / 3;
            //вычисляем координаты вершины пика
            double x5 = x3 + (x4-x3) / 2 + (y4-y3) + Math.Sqrt(3) / 2;
            double y5 = y3 + (y4 - y3) / 2 - (x4 - x3) + Math.Sqrt(3) / 2;
            //рисуем 4 отрезка
            DrawFracKoha(canvas, lvl - 1, color, x1, y1, x3, y3);
            DrawFracKoha(canvas, lvl - 1, color, x3, y3, x5, y5);
            DrawFracKoha(canvas, lvl - 1, color, x5, y5, x4, y4);
            DrawFracKoha(canvas, lvl - 1, color, x4, y4, x2, y2);
        }

        //треугольник
        private void DrawFracTreygolnik(Canvas canvas, int lvl, Color color, double x, double y, double length)
        {
            if (lvl == 1 || lvl == 0)
            {
                Point[] points = new Point[3]
                {
                    new Point(x, y),
                    new Point(x + length / 2, y + Math.Sqrt(3) * length / 2),
                    new Point(x + length, y)
                };
                Polygon polygon = new Polygon
                {
                    Fill = new SolidColorBrush(color),
                    
                    StrokeThickness = 1,
                    Points = new PointCollection(points)
                };
                canvas.Children.Add(polygon);
                return;
            }
            DrawFracTreygolnik(canvas, lvl - 1, color, x, y, length / 2);
            DrawFracTreygolnik(canvas, lvl - 1, color, x + length / 2, y, length / 2);
            DrawFracTreygolnik(canvas, lvl - 1, color, x + length / 4, y + Math.Sqrt(3) * length / 4, length / 2);
        }

        //ковер
        private void DrawFracKover(Canvas canvas, int lvl, Color color, double x, double y, double size)
        {
            if (lvl < 3)
            {
                Rectangle rect = new Rectangle
                {
                    Fill = new SolidColorBrush(color),
                    Width = size,
                    Height = size,
                };
                Canvas.SetLeft(rect, x); 
                Canvas.SetTop(rect, y);
                canvas.Children.Add(rect);
                return;
            }
            double newSize = size / 3;
            //нарисовать 8 квадратов без центрального
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        continue; //пропуск центрального квадрата
                        
                    }
                    DrawFracKover(canvas, lvl - 1, color, x + i * newSize, y + j * newSize, newSize);
                }
                
            }
        }

        
    }
}