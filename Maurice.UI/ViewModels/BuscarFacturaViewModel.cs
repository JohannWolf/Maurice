using Maurice.Core.Models;
using ReactiveUI;
using System.Collections.Generic;

namespace Maurice.UI.ViewModels
{
    public class BuscarFacturaViewModel : ReactiveObject
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private List<SearchOptionsDBO> _searchOptions;
        public List<SearchOptionsDBO> SearchOptions
        {
            get => _searchOptions;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchOptions, value);
            }
        }

        public BuscarFacturaViewModel()
        {
            SearchOptions = GetSearchOptions();
        }

        private List<SearchOptionsDBO> GetSearchOptions()
        {
            return new List<SearchOptionsDBO>
            {
                new SearchOptionsDBO { Id = 1, OptionName = "RFC Emisor" },
                new SearchOptionsDBO { Id = 2, OptionName = "Fecha de Emision" },
                new SearchOptionsDBO { Id = 3, OptionName = "Nombre de Archivo" },
                new SearchOptionsDBO { Id = 4, OptionName = "Folio" },
                new SearchOptionsDBO { Id = 5, OptionName = "Nombre de Emisor" },
                new SearchOptionsDBO { Id = 6, OptionName = "UUID" },
            };
        }
    }
}
