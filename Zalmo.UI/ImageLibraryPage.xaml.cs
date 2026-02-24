using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zalmo.Infrastructure;
using Zalmo.Core;

namespace Zalmo.UI;

public partial class ImageLibraryPage : ContentPage
{
    private string? selectedPath;
    private List<string> imageFiles = new();

    public ImageLibraryPage()
    {
        InitializeComponent();
        // Picker removed, nothing to initialize
    }

    private async void OnBrowseClicked(object? sender, EventArgs e)
    {
        #if WINDOWS
            var hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.PlatformView).WindowHandle;
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                selectedPath = folder.Path;
                SelectedPathLabel.Text = selectedPath;
            }
        #else
            await DisplayAlert("Browse", "Folder picker is only implemented for Windows.", "OK");
        #endif
    }

    private async void OnOrganizeClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(selectedPath))
        {
            await DisplayAlert("Error", "Please select a file or folder first.", "OK");
            return;
        }

        // Gather image files
        imageFiles.Clear();
        if (Directory.Exists(selectedPath))
        {
            imageFiles.AddRange(Directory.GetFiles(selectedPath)
                .Where(f => IsImageFile(f)));
        }
        else if (File.Exists(selectedPath) && IsImageFile(selectedPath))
        {
            imageFiles.Add(selectedPath);
        }
        else
        {
            await DisplayAlert("Error", "Selected file is not a supported image.", "OK");
            return;
        }

        if (!imageFiles.Any())
        {
            await DisplayAlert("Error", "No supported image files found.", "OK");
            return;
        }

        var config = ConfigPersistenceService.LoadConfig();
        // Set DUMP folder to project root (2 levels up from bin/Debug/...)
        var exeDir = AppContext.BaseDirectory;
        var projectRoot = Path.GetFullPath(Path.Combine(exeDir, "..", "..", "..", ".."));
        var dumpRoot = Path.Combine(projectRoot, "DUMP");
        if (!Directory.Exists(dumpRoot))
            Directory.CreateDirectory(dumpRoot);
        ImageBatchGrouper.GroupAndDumpImages(imageFiles, dumpRoot, config.FolderNamingPattern, config.PeriodDays);
        ResultLabel.Text = $"Organized {imageFiles.Count} images into DUMP.";
    }

    private bool IsImageFile(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext == ".jpg" || ext == ".jpeg" || ext == ".bmp";
    }
}