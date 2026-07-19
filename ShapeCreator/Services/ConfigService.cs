using ShapeCreator.Models;
using ShapeCreator.Services.Interface;
using System.IO;

namespace ShapeCreator.Services;

public class ConfigService(
    ILoggerService loggerService,
    IFileService fileService,
    IUiService uiService
    ) : IConfigService
{
    private readonly ILoggerService loggerService = loggerService;
    private readonly IFileService fileService = fileService;
    private readonly IUiService uiService = uiService;

    private Root TestProject { get; set; }
    private string TestProjectPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TestProject.json");

    public bool LoadTestProject()
    {
        (bool isValid, string errorMessage, var root) = fileService.GetRootFromFile(TestProjectPath);

        if (isValid)
        {
            TestProject = root;
            return true;
        }
        else
        {
            uiService.ShowMessage("Ошибка при загрузке тестовой конфигурации", errorMessage);
            return false;
        }
    }

    public Root GetTestProject()
    {
        return TestProject;
    }
}
