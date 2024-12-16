using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp.Views
{
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            //var vm = new MainWindowViewModel();
            var vm = new NodeViewModel(new(IPAddress.Parse("43.155.193.230"), 20000));
            DataContext = vm;
        }
    }
}
