using ShapeCreator.Models;

namespace ShapeCreator.Services.Interface;

public interface IConfigService
{
    /// <summary>
    /// Грузим конфиг из файла .json в кэш память
    /// </summary>
    bool LoadTestProject();

    Root GetTestProject();
}