using System.Diagnostics;
using ShapeCreator.Models;
using ShapeCreator.Services.Interface;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace ShapeCreator.Services;

public class FileService(ILoggerService loggerService, IUiService uiService) : IFileService
{
    private readonly ILoggerService loggerService = loggerService;
    private readonly IUiService uiService = uiService;

    private JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public (bool, string, Root) GetRootFromFile(string path)
    {
        if (!File.Exists(path))
        {
            loggerService.Error($"TestProject file not found: {path}");
            return (false, "Не найден файл", new Root() { });
        }

        string json = File.ReadAllText(path);

        if (json == null)
        {
            loggerService.Error("json config is null");
            return (false, "Файл пуст", new Root() { });
        }

        Root result = new Root();
        try
        {
            result = JsonSerializer.Deserialize<Root>(json, JsonSerializerOptions) ?? new Root();
            return (true, string.Empty, result);
        }
        catch (Exception ex)
        {
            loggerService.Error("Deserialize TestProject Error", ex);
            loggerService.Error($"TestProject: {result}");
            return (false, "Ошибка при сериализации Json", new Root() { });
        }
    }

    public (bool? isValid, string errorMessage) SaveToFile(Root root)
    {
        try
        {
            (bool? isValid, string errorMessage, string path) = uiService.SaveFileDialog();

            if (isValid == false)
            {
                return (false, errorMessage);
            }

            if (isValid == true)
            {
                string json = JsonSerializer.Serialize(root, JsonSerializerOptions);
                File.Create(path).Close();

                File.WriteAllText(path, json);

                return (true, string.Empty);
            }

            return (null, string.Empty);

        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (bool isValid, string errorMessage) OpenLogFolder()
    {
        try
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            Process.Start("explorer.exe", logFolder);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            loggerService.Error("OpenTodayLogs Error", ex);
            return (false, ex.Message);
        }
    }

    public (bool isValid, string errorMessage) OpenTodayLogs()
    {
        try
        {
            string logFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "logs",
                $"{DateTime.Now:yyyy-MM-dd}.log");

            if (!File.Exists(logFile))
            {
                return (false, $"Не найден файл {logFile}");
            }

            Process.Start("explorer.exe", logFile);

            return (true, string.Empty);

        }
        catch (Exception ex)
        {
            loggerService.Error("OpenTodayLogs Error", ex);
            return (false, ex.Message);
        }
    }
}
