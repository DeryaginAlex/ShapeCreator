using ShapeCreator.Models;
using System.Windows;

namespace ShapeCreator.Services.Interface;

public interface IUiService
{
    /// <summary>
    /// По модели Shape рисуем ui элементы для canvas
    /// </summary>
    /// <param name="shapes">Модель по которой будем рисовать UI</param>
    /// <param name="canvasWidth">Ширина canvas в котором будем рисовать UI элементы</param>
    /// <param name="canvasHeight">Высота canvas в котором будем рисовать UI элементы</param>
    /// <returns></returns>
    public List<UIElement> GetUiElements(List<Shape> shapes, double canvasWidth, double canvasHeight);

    /// <summary>
    /// Отображение сообщения для пользователя
    /// </summary>
    /// <param name="title">Заголовок</param>
    /// <param name="message">Сообщение</param>
    public void ShowMessage(string title, string message);
}