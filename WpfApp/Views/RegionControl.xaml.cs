using Core.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace WpfApp.Views
{
    public partial class RegionControl : UserControl
    {
        public RegionControl(Region region)
        {
            InitializeComponent();
            var vm = new RegionViewModel(region);
            DataContext = vm;
        }
    }
}
