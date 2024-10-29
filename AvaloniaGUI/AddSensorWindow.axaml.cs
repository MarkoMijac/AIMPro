using AIMCore.Sensors;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaGUI;

public partial class AddSensorWindow : Window
{
    public ISensor Sensor { get; private set; }

    public AddSensorWindow()
    {
        InitializeComponent();
    }
}