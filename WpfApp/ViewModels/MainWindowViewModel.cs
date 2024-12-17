using CommunityToolkit.Mvvm.ComponentModel;
using Core.Region;
using System.Collections.ObjectModel;
using System.Net;

namespace WpfApp.ViewModels
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        [ObservableProperty]
        private ObservableCollection<RegionViewModel> _regions;

        public MainWindowViewModel()
        {
            Regions = [];

            Region KoreaRegion = new("KR");
            KoreaRegion.Nodes.Add(new(IPAddress.Parse("43.155.193.230"), 20000));
            RegionViewModel KoreaVM = new(KoreaRegion);
            Regions.Add(KoreaVM);

            Region JapanRegion = new("JP");
            JapanRegion.Nodes.Add(new(IPAddress.Parse("43.163.252.167"), 20000));
            RegionViewModel JapanVM = new(JapanRegion);
            Regions.Add(JapanVM);

            Region SingaporeRegion = new("SG");
            SingaporeRegion.Nodes.Add(new(IPAddress.Parse("43.134.150.4"), 20000));
            RegionViewModel SingaporeVM = new(SingaporeRegion);
            Regions.Add(SingaporeVM);

            Region HongKongRegion = new("HK");
            HongKongRegion.Nodes.Add(new(IPAddress.Parse("43.132.138.189"), 20000));
            HongKongRegion.Nodes.Add(new(IPAddress.Parse("43.175.252.41"), 20000));
            HongKongRegion.Nodes.Add(new(IPAddress.Parse("43.175.253.41"), 20000));
            RegionViewModel HongKongVM = new(HongKongRegion);
            Regions.Add(HongKongVM);

            Region Sangpaolo = new("BR");
            Sangpaolo.Nodes.Add(new(IPAddress.Parse("43.175.253.234"), 20000));
            RegionViewModel SangpaoloVM = new(Sangpaolo);
            Regions.Add(SangpaoloVM);
        }
    }
}
