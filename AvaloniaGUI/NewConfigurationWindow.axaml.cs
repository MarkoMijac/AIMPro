using AIMCore;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace AvaloniaGUI;

public partial class NewConfigurationWindow : Window
{
    private Configuration _configuration = new Configuration();

    public NewConfigurationWindow()
    {
        InitializeComponent();
    }

    private void BtnAddInstrument_Click(object sender, RoutedEventArgs e)
    {
        var addInstrumentWindow = new AddInstrumentWindow(_configuration);
        var dialogTask = addInstrumentWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DisplayInstrumentInfo();
            });
        });
    }

    private void DisplayInstrumentInfo()
    {
        if(_configuration.BaseInstrument == null)
        {
            txtInstrumentName.Text = string.Empty;
            btnRemoveInstrument.IsEnabled = false;
        }
        else
        {
            txtInstrumentName.Text = _configuration.BaseInstrument.Name;
            btnRemoveInstrument.IsEnabled = true;
        }
    }

    private void BtnRemoveInstrument_Click(object sender, RoutedEventArgs e)
    {
        _configuration.BaseInstrument = null;
        DisplayInstrumentInfo();
    }

    private void BtnAddModel_Click(object sender, RoutedEventArgs e)
    {
        var addModelWindow = new AddModelWindow();
        var dialogTask = addModelWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DisplayModelInfo();
            });
        });
    }

    private void DisplayModelInfo()
    {
        if(_configuration.AIModel == null)
        {
            txtModelName.Text = string.Empty;
            btnRemoveModel.IsEnabled = false;
        }
        else
        {
            txtModelName.Text = _configuration.AIModel.Name;
            btnRemoveModel.IsEnabled = true;
        }
    }

    private void BtnRemoveModel_Click(object sender, RoutedEventArgs e)
    {
        _configuration.AIModel = null;
        DisplayModelInfo();
    }

    private void BtnAddSensor_Click(object sender, RoutedEventArgs e)
    {
        var addSensorWindow = new AddSensorWindow();
        var dialogTask = addSensorWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                RefreshSensors();
            });
        });
    }

    private void RefreshSensors()
    {
        SensorsDataGrid.ItemsSource = _configuration.Sensors;
        btnRemoveSensor.IsEnabled = _configuration.Sensors.Count > 0;
    }

    private void BtnRemoveSensor_Click(object sender, RoutedEventArgs e)
    {
        ISensor selectedSensor = GetSelectedSensor();
        if(selectedSensor != null)
        {
            _configuration.Sensors.Remove(selectedSensor);
            RefreshSensors();
        }
    }

    private void TextBox_TextChanged(object sender, RoutedEventArgs e)
    {
        _configuration.Name = txtConfigurationName.Text;
    }

    private ISensor GetSelectedSensor()
    {
        return SensorsDataGrid.SelectedItem as ISensor;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}