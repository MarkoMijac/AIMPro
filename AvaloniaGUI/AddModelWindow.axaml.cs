using System;
using System.Collections.Generic;
using AIMCore;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace AvaloniaGUI;

public partial class AddModelWindow : Window
{
    public IAIModel AIModel { get; private set; }

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
        if(string.IsNullOrEmpty(txtFilePath.Text))
        {
            throw new ArgumentException("File path cannot be empty.");
        }
    }

    private async void BtnBrowse_Click(object sender, RoutedEventArgs e)
    {
        var result = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Model File",
            FileTypeFilter = new List<FilePickerFileType>
            {
                new FilePickerFileType("Model Files") { Patterns = new[] { "*.onnx", "*.pb" } }
            },
            AllowMultiple = false
        });
    
        if (result != null && result.Count > 0)
        {
            txtFilePath.Text = result[0].Path.LocalPath;
        }
    }

    private void SaveModel()
    {
        string name = txtModelName.Text;
        string path = txtFilePath.Text;

        AIModel = new AIModel(name, path);        
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}