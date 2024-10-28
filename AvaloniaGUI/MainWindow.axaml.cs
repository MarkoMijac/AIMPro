using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnNewConfiguration_Click(object sender, RoutedEventArgs e)
    {
        var newConfigurationWindow = new NewConfigurationWindow();
        newConfigurationWindow.Show();
    }

}