using Avalonia.Controls;
using Maurice.UI.ViewModels;

namespace Maurice.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
