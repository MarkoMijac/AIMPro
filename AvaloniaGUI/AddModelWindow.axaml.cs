using AIMCore;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaGUI;

public partial class AddModelWindow : Window
{
    public IModel AIModel { get; private set; }

    public AddModelWindow()
    {
        InitializeComponent();
    }
}