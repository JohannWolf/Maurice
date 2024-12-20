using Avalonia.Controls;
using Maurice.UI.ViewModels;

namespace Maurice.UI.Views
{
    public partial class Configuracion : Window
    {
        public Configuracion()
        {
            InitializeComponent();
            DataContext = new ConfiguracionViewModel();
        }
    }
}
