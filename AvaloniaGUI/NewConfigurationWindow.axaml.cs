using System.Collections.Generic;
using System.Linq;
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Sensors;
using AIMPersistence;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace AvaloniaGUI;

public partial class NewConfigurationWindow : Window
{
    private ISensor _baseInstrument;
    private IAIModel _aiModel;
    private List<ISensor> _sensors = new List<ISensor>();

    private Configuration _configuration = new Configuration();

    public NewConfigurationWindow()
    {
        InitializeComponent();
    }

    private void BtnAddInstrument_Click(object sender, RoutedEventArgs e)
    {
        var addInstrumentWindow = new AddInstrumentWindow();
        var dialogTask = addInstrumentWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _baseInstrument = addInstrumentWindow.Sensor;
                DisplayInstrumentInfo(_baseInstrument);
            });
        });
    }

    private void DisplayInstrumentInfo(ISensor instrument)
    {
        if(instrument == null)
        {
            txtInstrumentName.Text = string.Empty;
            btnRemoveInstrument.IsEnabled = false;
        }
        else
        {
            txtInstrumentName.Text = instrument.Name;
            btnRemoveInstrument.IsEnabled = true;
        }
    }

    private void BtnRemoveInstrument_Click(object sender, RoutedEventArgs e)
    {
        _baseInstrument = null;
        DisplayInstrumentInfo(_baseInstrument);
    }

    private void BtnAddModel_Click(object sender, RoutedEventArgs e)
    {
        var addModelWindow = new AddModelWindow();
        var dialogTask = addModelWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _aiModel = addModelWindow.AIModel;
                DisplayModelInfo(_aiModel);
            });
        });
    }

    private void DisplayModelInfo(IAIModel aiModel)
    {
        if(aiModel == null)
        {
            txtModelName.Text = string.Empty;
            btnRemoveModel.IsEnabled = false;
        }
        else
        {
            txtModelName.Text = aiModel.Name;
            btnRemoveModel.IsEnabled = true;
        }
    }

    private void BtnRemoveModel_Click(object sender, RoutedEventArgs e)
    {
        _aiModel = null;
        DisplayModelInfo(_aiModel);
    }

    private void BtnAddSensor_Click(object sender, RoutedEventArgs e)
    {
        var addSensorWindow = new AddInstrumentWindow();
        var dialogTask = addSensorWindow.ShowDialog(this);
        dialogTask.ContinueWith(task =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var sensor = addSensorWindow.Sensor;
                if(sensor != null)
                {
                    _sensors.Add(sensor);
                    RefreshSensors(_sensors);
                }
            });
        });
    }

    private void RefreshSensors(List<ISensor> sensors)
    {

        SensorsDataGrid.ItemsSource = sensors.ToList(); // Set the new items
    }

    private void BtnRemoveSensor_Click(object sender, RoutedEventArgs e)
    {
        ISensor selectedSensor = GetSelectedSensor();
        if(selectedSensor != null)
        {
            _sensors.Remove(selectedSensor);
            RefreshSensors(_sensors);
        }
    }

    private ISensor GetSelectedSensor()
    {
        return SensorsDataGrid.SelectedItem as ISensor;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .SetName(txtConfigurationName.Text)
            .SetInstrument(_baseInstrument)
            .SetAIModel(_aiModel)
            .SetSensors(_sensors)
            .Build();

        var saveFileOptions = new FilePickerSaveOptions
        {
            DefaultExtension = "json",
            SuggestedFileName = $"{configuration.Name}.json",
            FileTypeChoices = new List<FilePickerFileType>
            {
                new FilePickerFileType("JSON Files") { Patterns = new[] { "*.json" } }
            }
        };

        var result = await StorageProvider.SaveFilePickerAsync(saveFileOptions);
        if (result != null)
        {
            var filePath = result.Path.LocalPath;
            ConfigurationSerializer.SaveToFile(configuration, filePath);
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}