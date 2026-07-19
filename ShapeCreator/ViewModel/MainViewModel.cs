using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ShapeCreator.Models;
using ShapeCreator.Services.Interface;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace ShapeCreator.ViewModel;

public partial class MainViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly IConfigService configService;
    private readonly ILoggerService loggerService;
    private readonly IUiService uiService;
    private readonly IValidateService validateService;
    private readonly IFileService fileService;

    public ObservableCollection<UIElement> CanvasElements { get; } = [];

    public ObservableCollection<Shape> Shapes { get; set; }

    public Shape SelectedShape { get; set; }

    public Array ShapeTypes => Enum.GetValues(typeof(ShapeType));

    public MainViewModel(
        IConfigService configService,
        ILoggerService loggerService,
        IUiService uiService,
        IValidateService validateService,
        IFileService fileService)
    {
        this.configService = configService;
        this.loggerService = loggerService;
        this.uiService = uiService;
        this.validateService = validateService;
        this.fileService = fileService;


        configService.LoadTestProject();
        var shapes = configService.GetTestProject().Shapes;

        ReLoadeShapes(shapes);
        ReDrawCanvas();
        Subscriptions();
    }

    public void Subscriptions()
    {
        Shapes.CollectionChanged += Shapes_CollectionChanged;
        foreach (var shape in Shapes)
        {
            shape.PropertyChanged += PropertyChanged;
            shape.CoordinateStart.PropertyChanged += PropertyChanged;
            shape.CoordinateFinish.PropertyChanged += PropertyChanged;
        }
    }

    private void ReDrawCanvas()
    {
        CanvasElements.Clear();

        var elements = uiService.GetUiElements(Shapes.ToList(), 785, 300);

        foreach (var element in elements)
        {
            CanvasElements.Add(element);
        }
    }

    public void ReLoadeShapes(List<Shape> shapes)
    {
        if (shapes == null || !shapes.Any())
        {
            Shapes.Clear();
            return;
        }

        if (Shapes == null)
        {
            Shapes = new ObservableCollection<Shape> { };
        }

        Shapes.Clear();

        foreach (var shape in shapes)
        {
            Shapes.Add(shape);
        }
    }

    private void PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ReDrawCanvas();
    }

    private void Shapes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (Shape shape in e.NewItems!)
            {
                shape.id = Guid.NewGuid().ToString().Replace("-", string.Empty);
                shape.PropertyChanged += PropertyChanged;
                shape.CoordinateStart.PropertyChanged += PropertyChanged;
                shape.CoordinateFinish.PropertyChanged += PropertyChanged;
            }
        }
        ReDrawCanvas();
    }

    [RelayCommand]
    private void CreateNewProject()
    {
        string emptyProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "EmptyProject.json");

        (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(emptyProjectPath);
        if (isValid)
        {
            ReLoadeShapes(root.Shapes);
            ReDrawCanvas();
            Subscriptions();
        }
        else
        {
            uiService.ShowMessage("Ошибка при сохранении", errorMessage);
        }
    }

    [RelayCommand]
    private void OpenProject()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "Выберите файл",
            Filter = "JSON files (*.json)|*.json",
            InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projects")
        };

        bool? result = openFileDialog.ShowDialog();
        if (result != true)
        {
            return;
        }

        string filePath = openFileDialog.FileName;

        (bool isValid, string errorMessage, Root root) = fileService.GetRootFromFile(filePath);
        if (isValid)
        {
            ReLoadeShapes(root.Shapes);
            ReDrawCanvas();
            Subscriptions();
        }
        else
        {
            uiService.ShowMessage("Ошибка при открытии файла", errorMessage);
        }
    }

    [RelayCommand]
    private void SaveProject()
    {
        (bool IsValid, string validMessage) = validateService.IsValid(Shapes.ToList());

        if (!IsValid)
        {
            uiService.ShowMessage("Ошибка валидации", validMessage);
            return;
        }

        Root root = new Root() { Shapes = Shapes.ToList() };
        (bool isValid, string saveMessage) = fileService.SaveToFile(root);
        if (IsValid)
        {
            uiService.ShowMessage("Сохранение", "Созранение выполненно успешно");
            return;
        }
    }
}