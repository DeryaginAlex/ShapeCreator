using System.ComponentModel;
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
        Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.Closing();
        }
    }
}