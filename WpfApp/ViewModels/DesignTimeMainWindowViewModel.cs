using DnsClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

namespace WpfApp.ViewModels
{
    public class DesignTimeMainWindowViewModel : IMainWindowViewModel
    {
        private readonly ObservableCollection<RegionViewModel> _regions;
        public IEnumerable<RegionViewModel> Regions => _regions;

        public DesignTimeMainWindowViewModel()
        {
            _regions = [];
            Task.Run(() => LoadRegionsAsync());
        }

        public async Task LoadRegionsAsync()
        {
            var tasks = new[]
            {
                //LoadNodesAsync("Seoul, Korea (the Republic of)", "kr"),
                //LoadNodesAsync("Tokyo, Japan", "jp"),
                LoadNodesAsync("Hong Kong, Hong Kong", "hk"),
                LoadNodesAsync("Singapore, Singapore", "sg"),
                //LoadNodesAsync("Frankfurt am Main, Germany", "de"),
                //LoadNodesAsync("Queretaro, Mexico", "mex"),
                //LoadNodesAsync("Chicago, United States", "chi"),
                //LoadNodesAsync("California, United States", "usw"),
                //LoadNodesAsync("Virginia, United States","use"),
                //LoadNodesAsync("Miami, United States", "mia"),
                //LoadNodesAsync("Sao Paulo, Brazil", "sao"),
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadNodesAsync(string regionName, string regionCode)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 53);
            var client = new LookupClient(endpoint);

            RegionViewModel regionViewModel = new(regionName);

            // UI 스레드에서 Regions 컬렉션을 업데이트
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                _regions.Add(regionViewModel);
            });

            // EdgeOne Accelerator
            var edgeOneResult = await client.QueryAsync($"klbq-prod-ds-{regionCode}1-eo.strinova.com", QueryType.A);
            var edgeOneResponse = edgeOneResult.Answers.ARecords();
            foreach (var record in edgeOneResponse)
            {
                var ip = record.Address;
                var port = 20000;
                Debug.WriteLine($"{regionName}.{regionCode}:EdgeOne Accelerator: {ip}");
                regionViewModel.AddEdgeOne(new(ip, port));
            }

            // Servers
            for (int i = 1; i <= 150; i++)
            {
                var serverResult = await client.QueryAsync($"klbq-prod-ds-{regionCode}{i}-server.strinova.com", QueryType.A);
                var serverResponse = serverResult.Answers.ARecords();
                if (serverResponse.Count() == 0)
                {
                    break;
                }
                foreach (var record in serverResponse)
                {
                    var ip = record.Address;
                    var port = 20000;
                    Debug.WriteLine($"{regionName}.{regionCode}:{i}번째 Server: {ip}");
                    regionViewModel.AddServer(new(ip, port));
                }
            }
        }
    }
}
