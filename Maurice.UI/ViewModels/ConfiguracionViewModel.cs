using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using Maurice.Data.Services;
using System;
using System.Threading.Tasks;
using Maurice.Core.Models;
using Avalonia.Controls;

namespace Maurice.UI.ViewModels
{
    public class ConfiguracionViewModel : ReactiveObject
    {
        private readonly DatabaseService _databaseService;

        private string _rfc;
        public string Rfc
        {
            get => _rfc;
            set => this.RaiseAndSetIfChanged(ref _rfc, value);
        }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set => this.RaiseAndSetIfChanged(ref _nombre, value);
        }

        private string _codigoPostal;
        public string CodigoPostal
        {
            get => _codigoPostal;
            set => this.RaiseAndSetIfChanged(ref _codigoPostal, value);
        }

        private RegimenFiscalOption _selectedRegimenFiscal;
        public RegimenFiscalOption SelectedRegimenFiscal
        {
            get => _selectedRegimenFiscal;
            set => this.RaiseAndSetIfChanged(ref _selectedRegimenFiscal, value);
        }

        private ObservableCollection<RegimenFiscalOption> _regimenFiscalOptions;
        public ObservableCollection<RegimenFiscalOption> RegimenFiscalOptions
        {
            get => _regimenFiscalOptions;
            set => this.RaiseAndSetIfChanged(ref _regimenFiscalOptions, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public ConfiguracionViewModel()
        {
            _databaseService = new DatabaseService();
            LoadRegimenFiscalOptionsAsync(); // Asynchronously load options
            SaveCommand = ReactiveCommand.Create(SaveUserData);
        }

        private async Task LoadRegimenFiscalOptionsAsync()
        {
            try
            {
                RegimenFiscalOptions = new ObservableCollection<RegimenFiscalOption>( _databaseService.GetRegimenFiscalOptions());
                // Optionally set the first item as selected:
                if (RegimenFiscalOptions.Count > 0)
                {
                    SelectedRegimenFiscal = RegimenFiscalOptions[0];
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error, show a message to the user)
                Console.Error.WriteLine($"Error loading regimen fiscal options: {ex}");
            }
        }

        private void SaveUserData()
        {
            string regimenFiscal = SelectedRegimenFiscal?.Code; // Assuming your model uses "Code"
            if (!_databaseService.SaveUserData(Rfc, Nombre, CodigoPostal, regimenFiscal, out string errorMessage))
            {
                // Handle the error (e.g., display a message box)
                Console.Error.WriteLine($"Error saving user data: {errorMessage}");
            }
        }
    }
}