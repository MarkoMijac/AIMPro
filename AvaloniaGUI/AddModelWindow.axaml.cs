using System;
using AIMCore;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaGUI;

public partial class AddModelWindow : Window
{
    public IModel AIModel { get; private set; }

    public AddModelWindow()
    {
        InitializeComponent();
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ValidateInput();
            SaveModel();
            Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ValidateInput()
    {
        if(string.IsNullOrEmpty(txtModelName.Text))
        {
            throw new ArgumentException("Model name cannot be empty.");
        }
        if(string.IsNullOrEmpty(txtBaseAdress.Text))
        {
            throw new ArgumentException("Base address cannot be empty.");
        }
        if(string.IsNullOrEmpty(txtPredictionRoute.Text))
        {
            throw new ArgumentException("Prediction route cannot be empty.");
        }
    }

    private void SaveModel()
    {
        string name = txtModelName.Text;
        string baseAddress = txtBaseAdress.Text;
        string predictionRoute = txtPredictionRoute.Text;

        var model = new AIModelService(name, baseAddress, predictionRoute);

        var aim = AIM.Instance;
        aim.AddAIModel(model);
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}