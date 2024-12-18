using CommunityToolkit.Mvvm.ComponentModel;
using Core.Region;
using DnsClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

namespace WpfApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
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
                LoadNodesAsync("Seoul, Korea (the Republic of)", "kr"),
                LoadNodesAsync("Tokyo, Japan", "jp"),
                LoadNodesAsync("Hong Kong, Hong Kong", "hk"),
                LoadNodesAsync("Singapore, Singapore", "sg"),
                LoadNodesAsync("Frankfurt am Main, Germany", "de"),
                LoadNodesAsync("Queretaro, Mexico", "mex"),
                LoadNodesAsync("Chicago, United States", "chi"),
                LoadNodesAsync("California, United States", "usw"),
                LoadNodesAsync("Virginia, United States","use"),
                LoadNodesAsync("Miami, United States", "mia"),
                LoadNodesAsync("Sao Paulo, Brazil", "sao"),
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadNodesAsync(string regionName, string regionCode)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 53);
            var client = new LookupClient(endpoint);

            // Region을 정의해주고 RegionViewModel에 추가
            Region servers = new($"{regionName} Server");
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
                    servers.Add(new(ip, port));
                    //App.Current.Dispatcher.Invoke(() =>
                    //{
                    //    servers.Add(new(ip, port));
                    //});
                }
            }


            Region edgeOne = new($"{regionName} EdgeOne(Accelerator)");
            var edgeOneResult = await client.QueryAsync($"klbq-prod-ds-{regionCode}1-eo.strinova.com", QueryType.A);
            var edgeOneResponse = edgeOneResult.Answers.ARecords();
            foreach (var record in edgeOneResponse)
            {
                var ip = record.Address;
                var port = 20000;
                Debug.WriteLine($"{regionName}.{regionCode}:EdgeOne Accelerator: {ip}");
                edgeOne.Add(new(ip, port));
                //App.Current.Dispatcher.Invoke(() =>
                //{
                //    edgeOne.Add(new(ip, port));
                //});
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
