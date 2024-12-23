using System.Collections.ObjectModel;
using WpfApp.ViewModels;

namespace WpfApp.Services
{
    public class DesignTimeRegionService : IRegionService
    {
        private readonly ObservableCollection<RegionViewModel> _regions;
        private RegionFactory _regionFactory;

        public DesignTimeRegionService(ObservableCollection<RegionViewModel> regions)
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
                _regionFactory.LoadNodesAsync("Hong Kong, Hong Kong", "hk"),
            };

            await Task.WhenAll(tasks);
        }
    }
}
