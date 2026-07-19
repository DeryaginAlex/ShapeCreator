using ShapeCreator.Models;
using ShapeCreator.Services.Interface;
using System.Text;

namespace ShapeCreator.Services;

public class ValidateService(ILoggerService loggerService) : IValidateService
{
    private readonly ILoggerService loggerService = loggerService;

    public (bool, string) IsValid(List<Shape> shapes)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var shape in shapes)
        {
            if (string.IsNullOrEmpty(shape.name))
            {
                stringBuilder.Append($"У Фигуру \"{shape.id}\" не заданно имя{Environment.NewLine}");
            }

            if (shape.coordinateStart.X < 0)
            {
                stringBuilder.Append($"Фигура \"{shape.name}\" имеет отрицательную стартовую координату \"X\"{Environment.NewLine}");
            }

            if (shape.coordinateStart.Y < 0)
            {
                stringBuilder.Append($"Фигура \"{shape.name}\" имеет отрицательную стартовую координату \"Y\"{Environment.NewLine}");
            }

            if (shape.coordinateFinish.X < 0)
            {
                stringBuilder.Append($"Фигура \"{shape.name}\" имеет отрицательную финальную координату \"X\" {Environment.NewLine}");
            }

            if (shape.coordinateFinish.Y < 0)
            {
                stringBuilder.Append($"Фигура \"{shape.name}\" имеет отрицательную финальную координату \"Y\" {Environment.NewLine}");
            }
        }

        var duplicate = shapes
            .Select(x => x.name)
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicate.Count == 1)
        {
            stringBuilder.Append($"Найденны дубликат имени: \"{duplicate.Single()}\"{Environment.NewLine}");
        }

        if (duplicate.Count > 1)
        {
            var duplicates = string.Join(", ", duplicate);
            stringBuilder.Append($"Найденны дубликатs именён: \"{duplicates}\"{Environment.NewLine}");
        }

        var message = stringBuilder.ToString();

        var isValid = string.IsNullOrEmpty(message);

        return (isValid, message);
    }
}
