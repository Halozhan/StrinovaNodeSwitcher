using CommunityToolkit.Mvvm.ComponentModel;
using Core.Region;
using DnsClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            Task.Run(() => LoadRegionsAsync());
        }

        private async Task LoadRegionsAsync()
        {
            var tasks = new[]
            {
                Task.Run(() => LoadNodesAsync("Seoul, Korea (the Republic of)", "kr")),
                Task.Run(() => LoadNodesAsync("Tokyo, Japan", "jp")),
                Task.Run(() => LoadNodesAsync("Hong Kong, Hong Kong", "hk")),
                Task.Run(() => LoadNodesAsync("Singapore, Singapore", "sg")),
                Task.Run(() => LoadNodesAsync("Frankfurt am Main, Germany", "de")),
                Task.Run(() => LoadNodesAsync("Queretaro, Mexico", "mex")),
                Task.Run(() => LoadNodesAsync("Chicago, United States", "chi")),
                Task.Run(() => LoadNodesAsync("California, United States", "usw")),
                Task.Run(() => LoadNodesAsync("Virginia, United States","use")),
                Task.Run(() => LoadNodesAsync("Miami, United States", "mia")),
                Task.Run(() => LoadNodesAsync("Sao Paulo, Brazil", "sao")),
            };

            await Task.WhenAll(tasks);
        }

        private void LoadNodesAsync(string regionName, string regionCode)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 53);
            var client = new LookupClient(endpoint);

            Region servers = new($"{regionName} Server");
            for (int i = 1; i <= 150; i++)
            {
                var serverResult = client.Query($"klbq-prod-ds-{regionCode}{i}-server.strinova.com", QueryType.A);
                var serverResponse = serverResult.Answers.ARecords();
                foreach (var record in serverResponse)
                {
                    var ip = record.Address;
                    var port = 20000;
                    Debug.WriteLine($"{regionName}.{regionCode}:{i}번째 Server: {ip}");
                    servers.Nodes.Add(new(ip, port));
                }
            }


            Region edgeOne = new($"{regionName} EdgeOne(Accelerator)");
            var edgeOneResult = client.Query($"klbq-prod-ds-{regionCode}1-eo.strinova.com", QueryType.A);
            var edgeOneResponse = edgeOneResult.Answers.ARecords();
            foreach (var record in edgeOneResponse)
            {
                var ip = record.Address;
                var port = 20000;
                Debug.WriteLine($"{regionName}.{regionCode}:EdgeOne Accelerator: {ip}");
                edgeOne.Nodes.Add(new(ip, port));
            }

            //Regions.Add(new RegionViewModel(region));
            // IDK why should I use App.Current.Dispatcher.Invoke() here
            App.Current.Dispatcher.Invoke(() =>
            {
                Regions.Add(new RegionViewModel(servers));
                Regions.Add(new RegionViewModel(edgeOne));
            });
        }
    }
}
