using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Collections.Generic;
using System;
using Avalonia.Controls;
using Maurice.UI.Views;

namespace Maurice.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private string _selectedFileName;
        public string SelectedFileName
        {
            get => _selectedFileName;
            set => this.RaiseAndSetIfChanged(ref _selectedFileName, value);
        }

        public ReactiveCommand<Unit, Unit> SelectFileCommand { get; }

        public ReactiveCommand<Unit, Unit> OpenConfiguracionCommand { get; }

        public MainWindowViewModel()
        {
            SelectFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await SelectXmlFileAsync();
            });

            OpenConfiguracionCommand = ReactiveCommand.Create(OpenConfiguracion);

        }

        private async Task SelectXmlFileAsync()
        {
            var mainWindow = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (mainWindow == null || mainWindow.MainWindow == null)
            {
                return; // Handle the case where the main window is not available
            }

            var options = new FilePickerOpenOptions
            {
                Title = "Select an XML File",
                FileTypeFilter = new[]
            {
                new FilePickerFileType("XML Files") { Patterns = new[] { "*.xml" } }
            },
                AllowMultiple = false
            };

            try
            {
                IReadOnlyList<IStorageFile> files = null; // Initialize files to null

                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    files = await mainWindow.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                });

                if (files != null && files.Count > 0) // Check for null before accessing Count
                {
                    SelectedFileName = files[0].Name;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., show a message box
                Console.WriteLine($"Error selecting file: {ex.Message}");
            }
        }

        private void OpenConfiguracion()
        {
            var newWindow = new Configuracion();
            ShowWindow(newWindow);
        }

        private void ShowWindow(Window window)
        {
            var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            window.Show(lifetime?.MainWindow);
        }
    }
}
