using CommunityToolkit.Mvvm.ComponentModel;
using Core.Node;
using System.Collections.ObjectModel;

namespace WpfApp.ViewModels
{
    public partial class RegionViewModel : ObservableObject
    {
        public string Name { get; set; }
        public ObservableCollection<NodeViewModel> Servers { get; set; }
        public ObservableCollection<NodeViewModel> EdgeOne { get; set; }

        public int ServerCount => Servers.Count;
        public NodeViewModel WorstServer => Servers.MaxBy(node => node.Latency.Average);
        public int EdgeOneCount => EdgeOne.Count;
        public NodeViewModel BestEdgeOne => EdgeOne.MinBy(node => node.Latency.Average);

        public RegionViewModel(string name)
        {
            Name = name;
            Servers = [];
            EdgeOne = [];
        }

        public void AddServer(Node node)
        {
            // Invoke the UI thread to add the node
            App.Current.Dispatcher.Invoke(() =>
            {
                Servers.Add(new NodeViewModel(node, ServerCount + 1));
            });
        }

        public void AddEdgeOne(Node node)
        {
            // Invoke the UI thread to add the node
            App.Current.Dispatcher.Invoke(() =>
            {
                EdgeOne.Add(new NodeViewModel(node, EdgeOneCount + 1));
            });
        }
    }
}
