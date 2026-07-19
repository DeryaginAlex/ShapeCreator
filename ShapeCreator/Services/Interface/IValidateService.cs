using ShapeCreator.Models;

namespace ShapeCreator.Services.Interface;

public interface IValidateService
{
    /// <summary>
    /// Валидируем модель
    /// </summary>
    /// <param name="shapes"></param>
    /// <returns>
    /// <c>isValid</c> - Валидна ли модель?
    /// <c>errorMessage</c> - сообщение об ошибке
    /// </returns>
    (bool isValid, string errorMessage) IsValid(List<Shape> shapes);
}