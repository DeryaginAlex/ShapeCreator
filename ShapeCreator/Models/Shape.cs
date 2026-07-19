using CommunityToolkit.Mvvm.ComponentModel;

namespace ShapeCreator.Models;

public partial class Shape : ObservableObject
{
    [ObservableProperty]
    public string id = string.Empty;

    [ObservableProperty]
    public string name = string.Empty;

    [ObservableProperty]
    public ShapeType shapeType;

    [ObservableProperty]
    public Coordinate coordinateStart = new();

    [ObservableProperty]
    public Coordinate coordinateFinish = new();

    [ObservableProperty]
    public bool isActive;
}