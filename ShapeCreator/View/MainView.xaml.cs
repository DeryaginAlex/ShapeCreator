using ShapeCreator.ViewModel;
using System.Windows;

namespace ShapeCreator;

public partial class MainView : Window
{
    private MainViewModel ViewModel { get; set; }
    private readonly IServiceProvider serviceProvider;

    public MainView(MainViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this.serviceProvider = serviceProvider;
        ViewModel = viewModel;
        DataContext = ViewModel;
    }
}