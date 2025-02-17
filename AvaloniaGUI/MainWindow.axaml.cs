using AIMCore;
using AIMPersistence;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaGUI;

public partial class MainWindow : Window
{
    public AIM AIM { get; set; }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        AIM = new AIM();
    }

    private void btnLoadConfiguration_Click(object sender, RoutedEventArgs e)
    {
        var confRepo = new ConfigurationRepository();
        var configuration = confRepo.GetDefaultConfiguration();
        AIM.LoadConfiguration(configuration);
        
    }

    private void btnStartSession_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void btnEndSession_Click(object sender, RoutedEventArgs e)
    {
        
    }
}