using CommunityToolkit.Mvvm.ComponentModel;

namespace ShapeCreator.Models;

public partial class Coordinate : ObservableObject
{
    [ObservableProperty]
    private double x;

    [ObservableProperty]
    private double y;
}
