using System;
using System.Linq;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;
using AIMSmartScale.Parsers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaGUI;

public partial class AddInstrumentWindow : Window
{
    private ScaleMeasurementParserFactory _parserFactory;
    private DefaultCommunicationFactory _communicationFactory;

    public ISensor Sensor { get; private set; }

    public AddInstrumentWindow()
    {
        _parserFactory = new ScaleMeasurementParserFactory();
        _communicationFactory = new DefaultCommunicationFactory();
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        FillCommunicationTypes();
        FillParsers();
    }

    private void FillParsers()
    {
        _parserFactory.GetParsers().ForEach(x => cmbParser.Items.Add(x));
    }

    private void FillCommunicationTypes()
    {
        _communicationFactory.GetCommunicationStrategies().ForEach(x => cmbCommunicationType.Items.Add(x));
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ValidateInput();
            SaveInstrument();
            Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ValidateInput()
    {
        if (string.IsNullOrEmpty(txtInstrumentName.Text))
        {
            throw new ArgumentException("Instrument name cannot be empty.");
        }
        if (string.IsNullOrEmpty(txtRequestCommand.Text))
        {
            throw new ArgumentException("Request command cannot be empty.");
        }
        if (cmbCommunicationType.SelectedIndex == -1)
        {
            throw new ArgumentException("Communication type must be selected.");
        }
        if (cmbParser.SelectedIndex == -1)
        {
            throw new ArgumentException("Parser must be selected.");
        }
        if (string.IsNullOrEmpty(txtPort.Text))
        {
            throw new ArgumentException("Port cannot be empty.");
        }
        if (string.IsNullOrEmpty(txtBaudRate.Text))
        {
            throw new ArgumentException("Baud rate cannot be empty.");
        }
    }

    private void SaveInstrument()
    {
        string name = txtInstrumentName.Text;
        string request = txtRequestCommand.Text;
        string comType = cmbCommunicationType.SelectedItem.ToString();

        var communicationType = cmbCommunicationType.SelectedItem as ICommunicationStrategy;
        var parser = cmbParser.SelectedItem as IMeasurementParser;
        string port = txtPort.Text;
        int baudRate = int.Parse(txtBaudRate.Text);

        Sensor = new DefaultSensorBuilder()
            .SetName(name)
            .SetRequestCommand(request)
            .SetCommunicationStrategy(communicationType)
            .SetParser(parser)
            .Build();
    }
}