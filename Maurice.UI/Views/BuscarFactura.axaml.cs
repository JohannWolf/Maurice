using Avalonia.Controls;
using Maurice.UI.ViewModels;

namespace Maurice.UI.Views
{
    public partial class BuscarFactura : Window
    {
        public BuscarFactura()
        {
            InitializeComponent();
            DataContext = new BuscarFacturaViewModel();
        }
    }
}