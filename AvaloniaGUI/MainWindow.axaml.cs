using AIMPersistence;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnLoadConfiguration_Click(object sender, RoutedEventArgs e)
    {
        var confRepo = new ConfigurationRepository();
        var configuration = confRepo.GetDefaultConfiguration();
    }
}