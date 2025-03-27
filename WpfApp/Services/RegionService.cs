using System.Collections.ObjectModel;
using WpfApp.ViewModels;

namespace WpfApp.Services
{
    public class RegionService : IRegionService
    {
        private readonly ObservableCollection<RegionViewModel> _regions;
        private RegionFactory _regionFactory;

        public RegionService(ObservableCollection<RegionViewModel> regions)
        {
            _regions = regions;
            _regionFactory = new(_regions);

            Task.Run(() => LoadRegionsAsync());
        }

        public async Task LoadRegionsAsync()
        {
            var tasks = new[]
            {
                _regionFactory.LoadNodesAsync("Seoul, Korea (the Republic of)", "kr"),
                _regionFactory.LoadNodesAsync("Tokyo, Japan", "jp"),
                _regionFactory.LoadNodesAsync("Hong Kong, Hong Kong", "hk"),
                _regionFactory.LoadNodesAsync("Singapore, Singapore", "sg"),
                _regionFactory.LoadNodesAsync("Frankfurt am Main, Germany", "de"),
                _regionFactory.LoadNodesAsync("Queretaro, Mexico", "mex"),
                _regionFactory.LoadNodesAsync("Chicago, United States", "chi"),
                _regionFactory.LoadNodesAsync("California, United States", "usw"),
                _regionFactory.LoadNodesAsync("Virginia, United States","use"),
                _regionFactory.LoadNodesAsync("Miami, United States", "mia"),
                _regionFactory.LoadNodesAsync("Sao Paulo, Brazil", "sao"),
            };

            await Task.WhenAll(tasks);
        }
    }
}
