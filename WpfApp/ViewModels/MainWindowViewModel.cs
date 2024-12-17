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
                LoadRegionsAsync("kr", false),
                LoadRegionsAsync("kr", true),
                LoadRegionsAsync("jp", false),
                LoadRegionsAsync("jp", true),
                LoadRegionsAsync("hk", false),
                LoadRegionsAsync("hk", true),
                LoadRegionsAsync("sg", false),
                LoadRegionsAsync("sg", true),
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadRegionsAsync(string regionCode, bool isEdgeOne)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 53);
            var client = new LookupClient(endpoint);
            string edge = isEdgeOne ? "eo" : "server";
            int edgeOneIndex = 1;
            int serverIndex = 30;

            Region region = new($"{regionCode}-{(isEdgeOne ? "eo" : "server")}");

            for (int i = 1; i <= (isEdgeOne ? edgeOneIndex : serverIndex); i++)
            {
                var result = client.Query($"klbq-prod-ds-{regionCode}{i}-{edge}.strinova.com", QueryType.A);
                var response = result.Answers.ARecords();
                foreach (var record in response)
                {
                    var ip = record.Address;
                    var port = 20000;
                    Debug.WriteLine($"{regionCode}:{i}번째 서버: {ip}");
                    region.Nodes.Add(new(ip, port));
                    //App.Current.Dispatcher.Invoke(() => region.Nodes.Add(new(ip, port)));
                }
            }
            //Regions.Add(new RegionViewModel(region));
            // IDK why should I use App.Current.Dispatcher.Invoke() here
            App.Current.Dispatcher.Invoke(() => Regions.Add(new RegionViewModel(region)));
        }
    }
}
