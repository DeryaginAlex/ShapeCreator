using ShapeCreator.Models;

namespace ShapeCreator.Services.Interface;

public interface IFileService
{
    /// <summary>
    /// Сохраняем модель в json файл
    /// </summary>
    /// <param name="root">Модель которую хотим сохранить</param>
    /// <returns>
    /// <see cref="bool"/> - модель успешно сохранена?
    /// <see cref="string"/> - сообщение об ошибке
    /// </returns>
    public (bool isValid, string message) SaveToFile(Root root);

    /// <summary>
    /// Получаем модель из файла
    /// </summary>
    /// <param name="path">путь к файлу</param>
    /// <returns>
    /// <see cref="bool"/> - модель получена?
    /// <see cref="string"/> - сообщение об ошибке
    /// <see cref="string"/> - модель
    /// </returns>
    public (bool, string, Root) GetRootFromFile(string path);
}