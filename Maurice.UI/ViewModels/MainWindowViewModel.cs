using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using Maurice.Core.Services;
using System.Collections.ObjectModel;
using Maurice.Core.Models;
using Maurice.UI.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Maurice.Data.Services;
using System.Collections.Generic;

namespace Maurice.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly FileService _fileService;
        private readonly DatabaseService _databaseService;
        private string _selectedFileName;
        private string _errorMessage;
        private string _successMessage;
        private ObservableCollection<XmlEntry> _xmlData;
        private IDictionary<string, string> _currentFacturaData;

        public ReactiveCommand<Unit, Unit> OpenConfiguracionCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenResumenMensualCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenResumenAnualCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveToDatabaseCommand { get; }



        public string SelectedFileName
        {
            get => _selectedFileName;
            set => this.RaiseAndSetIfChanged(ref _selectedFileName, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => this.RaiseAndSetIfChanged(ref _successMessage, value);
        }

        public ObservableCollection<XmlEntry> XmlData
        {
            get => _xmlData;
            set => this.RaiseAndSetIfChanged(ref _xmlData, value);
        }

        public ReactiveCommand<Unit, Unit> SelectFileCommand { get; }

        public MainWindowViewModel()
        {
            _fileService = new FileService();
            _databaseService = new DatabaseService();
            _currentFacturaData = new Dictionary<string, string>();
            XmlData = new ObservableCollection<XmlEntry>();
            SelectFileCommand = ReactiveCommand.CreateFromTask(SelectXmlFileAsync);
            SaveToDatabaseCommand = ReactiveCommand.Create(SaveToDatabase);
            OpenConfiguracionCommand = ReactiveCommand.Create(OpenConfiguracion);
            OpenResumenMensualCommand = ReactiveCommand.Create(OpeResumenMensual);
            OpenResumenAnualCommand = ReactiveCommand.Create(OpenResumenAnual);

            // Initialize SQLite database on startup
            _databaseService.InitializeDatabase();
        }

        private async Task SelectXmlFileAsync()
        {
            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime switch
            {
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
                _ => null
            };

            if (mainWindow == null) return;

            var dialog = new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                Title = "Select XML File",
                FileTypeFilter = new[]
                {
                    new Avalonia.Platform.Storage.FilePickerFileType("XML Files") { Patterns = new[] { "*.xml" } }
                },
                AllowMultiple = false
            };

            var files = await mainWindow.StorageProvider.OpenFilePickerAsync(dialog);
            if (files.Count > 0)
            {
                SelectedFileName = files[0].Name;

                _currentFacturaData = _fileService.ParseXml(files[0].Path.LocalPath);
                ErrorMessage = string.Empty; // Clear previous errors
                SuccessMessage = string.Empty; // Clear previous success messages

                XmlData.Clear();
                foreach (var kvp in _currentFacturaData)
                {
                    XmlData.Add(new XmlEntry { Key = kvp.Key, Value = kvp.Value });
                }
            }
        }

        private void SaveToDatabase()
        {
            if (_currentFacturaData != null && _currentFacturaData.Count > 0)
            {
                string fileName = SelectedFileName;
                if (!_databaseService.SaveFactura(_currentFacturaData, fileName, out string errorMessage))
                {
                    ErrorMessage = errorMessage; // Display error message in UI
                }
                else
                {
                    SuccessMessage = "Factura guardada exitosamente.";
                    XmlData.Clear();
                }
            }
            else
            {
                ErrorMessage = "Error, ningun dato guardado.";
            }
        }

        private void OpenConfiguracion()
        {
            // Ensure you have a reference to the Avalonia.Window class
            var configuracionWindow = new Configuracion
            {
                DataContext = new ConfiguracionViewModel() // Set the ViewModel as the DataContext
            };

            configuracionWindow.Show();
        }
        private void OpeResumenMensual()
        {
            // Ensure you have a reference to the Avalonia.Window class
            var resumenMensualWindow = new ResumenMensual
            {
                DataContext = new ResumenMensualViewModel() // Set the ViewModel as the DataContext
            };

            resumenMensualWindow.Show();
        }
        private void OpenResumenAnual()
        {
            // Ensure you have a reference to the Avalonia.Window class
            var resumenAnualWindow = new ResumenAnual
            {
                DataContext = new ResumenAnualViewModel() // Set the ViewModel as the DataContext
            };

            resumenAnualWindow.Show();
        }
        private void ShowWindow(Window window)
        {
            var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            window.Show(lifetime?.MainWindow);
        }
    }
}
