using Core.Region;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public class MainWindowViewModel
    {
        private ObservableCollection<RegionViewModel> regions;

        public MainWindowViewModel()
        {
            regions = [];

            var Korea = new Region("KR");
            Korea.Nodes.Add(new(IPAddress.Parse("43.155.193.230"), 20000));
            RegionViewModel KoreaVM = new RegionViewModel(Korea);
            regions.Add(KoreaVM);

            var Japan = new Region("JP");
            Japan.Nodes.Add(new(IPAddress.Parse("43.163.252.167"), 20000));
            RegionViewModel JapanVM = new RegionViewModel(Japan);
            regions.Add(JapanVM);

            var Singapore = new Region("SG");
            Singapore.Nodes.Add(new(IPAddress.Parse("43.134.150.4"), 20000));
            RegionViewModel SingaporeVM = new RegionViewModel(Singapore);
            regions.Add(SingaporeVM);
        }
    }
}
