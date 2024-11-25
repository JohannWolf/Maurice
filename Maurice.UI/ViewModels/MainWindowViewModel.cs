using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using Maurice.Core.Services;
using System.Collections.ObjectModel;
using Maurice.Core.Models;
using Maurice.UI.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace Maurice.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly FileService _fileService;
        private string _selectedFileName;
        private ObservableCollection<XmlEntry> _xmlData;
        public ReactiveCommand<Unit, Unit> OpenConfiguracionCommand  { get; }


        public string SelectedFileName
        {
            get => _selectedFileName;
            set => this.RaiseAndSetIfChanged(ref _selectedFileName, value);
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
            XmlData = new ObservableCollection<XmlEntry>();
            SelectFileCommand = ReactiveCommand.CreateFromTask(SelectXmlFileAsync);
            OpenConfiguracionCommand = ReactiveCommand.Create(OpenConfiguracion);

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

                var data = _fileService.ParseXml(files[0].Path.LocalPath);

                XmlData.Clear();
                foreach (var kvp in data)
                {
                    XmlData.Add(new XmlEntry { Key = kvp.Key, Value = kvp.Value });
                }
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
