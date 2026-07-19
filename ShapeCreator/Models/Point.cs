using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace ShapeCreator.Models;

public partial class Coordinate : ObservableObject
{
    [ObservableProperty]
    private double x;

    [ObservableProperty]
    private double y;
}
