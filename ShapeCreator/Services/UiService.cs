using ShapeCreator.Models;
using ShapeCreator.Services.Interface;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace ShapeCreator.Services;


public class UiService(ILoggerService loggerService) : IUiService
{
    private readonly ILoggerService loggerService = loggerService;

    /// <summary>
    /// Отступ от левого нижнего угла
    /// </summary>
    double Offset => 25;

    /// <summary>
    /// Отступ между делениями на осях
    /// </summary>
    double DivisionStep => 50;

    /// <summary>
    /// Цвет фигур
    /// </summary>
    SolidColorBrush ShapeColor => new SolidColorBrush(Colors.DarkBlue);

    /// <summary>
    /// Цвет осей и делений
    /// </summary>
    SolidColorBrush AxisColor => new SolidColorBrush(Colors.Black);

    /// <summary>
    /// Размер названия фигуры
    /// </summary>
    double FontSize => 8;

    public List<UIElement> GetUiElements(List<Models.Shape> shapes, double canvasWidth, double canvasHeight)
    {
        var result = GetAxis(canvasWidth, canvasHeight);

        foreach (var shape in shapes)
        {
            if (shape.IsActive == false)
            {
                continue;
            }

            double x1 = shape.CoordinateStart.X + Offset;
            double x2 = shape.CoordinateFinish.X + Offset;

            double y1 = canvasHeight - shape.CoordinateStart.Y - Offset;
            double y2 = canvasHeight - shape.CoordinateFinish.Y - Offset;

            double width = Math.Abs(x2 - x1);
            double height = Math.Abs(y2 - y1);
            double left = Math.Min(x1, x2);
            double top = Math.Min(y1, y2);
            double centerX = (x1 + x2) / 2;
            double centerY = (y1 + y2) / 2;

            var container = new Grid
            {
                Width = width,
                Height = height,
                Margin = new Thickness(left, top, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            switch (shape.ShapeType)
            {
                case ShapeType.Circle:
                    container.Children.Add(new Ellipse
                    {
                        Width = width,
                        Height = height,
                        Stroke = ShapeColor,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;

                case ShapeType.Square:
                    container.Children.Add(new Rectangle
                    {
                        Width = width,
                        Height = height,
                        Stroke = ShapeColor,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;

                case ShapeType.Rhombus:
                    var rhombus = new Polygon
                    {
                        Stroke = ShapeColor,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    double halfWidth = width / 2;
                    double halfHeight = height / 2;
                    rhombus.Points.Add(new Point(halfWidth, 0));
                    rhombus.Points.Add(new Point(width, halfHeight));
                    rhombus.Points.Add(new Point(halfWidth, height));
                    rhombus.Points.Add(new Point(0, halfHeight));

                    container.Children.Add(rhombus);

                    break;
            }

            var name = new TextBlock
            {
                Text = shape.Name,
                FontSize = FontSize,
                Foreground = ShapeColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
            };
            container.Children.Add(name);

            result.Add(container);
        }

        return result;
    }

    /// <summary>
    /// Ось абсцисс и ось ординат
    /// </summary>
    public List<UIElement> GetAxis(double canvasWidth, double canvasHeight)
    {
        var result = new List<UIElement>();

        // x-axis
        result.Add(new Line
        {
            X1 = Offset,
            Y1 = canvasHeight - Offset,
            X2 = canvasWidth - Offset,
            Y2 = canvasHeight - Offset,
            Stroke = AxisColor,
            StrokeThickness = 1
        });

        // y-axis
        result.Add(new Line
        {
            X1 = Offset,
            Y1 = canvasHeight - Offset,
            X2 = Offset,
            Y2 = Offset,
            Stroke = AxisColor,
            StrokeThickness = 1
        });

        // x-division
        int stepX = (int)((canvasWidth - (2 * Offset)) / DivisionStep);
        for (int i = 1; i <= stepX; i++)
        {
            double x = DivisionStep * i + Offset;
            result.Add(new Line
            {
                X1 = x,
                Y1 = canvasHeight - Offset - 5,
                X2 = x,
                Y2 = canvasHeight - Offset + 5,
                Stroke = AxisColor,
                StrokeThickness = 1
            });
        }

        // y-division
        int stepY = (int)((canvasHeight - (2 * Offset)) / DivisionStep);
        for (int i = 1; i <= stepY; i++)
        {
            double y = canvasHeight - i * DivisionStep - Offset;
            result.Add(new Line
            {
                X1 = Offset - 5,
                Y1 = y,
                X2 = Offset + 5,
                Y2 = y,
                Stroke = AxisColor,
                StrokeThickness = 1
            });
        }

        return result;
    }

    public void ShowMessage(string title, string message)
    {
        MessageBox.Show(message, title);
    }
}
