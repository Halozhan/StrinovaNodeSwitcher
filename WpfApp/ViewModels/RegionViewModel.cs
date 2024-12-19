using CommunityToolkit.Mvvm.ComponentModel;
using Core.Node;
using System.Collections.ObjectModel;

namespace WpfApp.ViewModels
{
    public partial class RegionViewModel : ObservableObject
    {
        public string Name { get; set; }
        public ObservableCollection<NodeViewModel> Nodes { get; set; }

        public int NodeCount => Nodes.Count;

        public RegionViewModel(string name)
        {
            Name = name;
            Nodes = [];
        }

        public void Add(Node node)
        {
            // Invoke the UI thread to add the node
            App.Current.Dispatcher.Invoke(() =>
            {
                Nodes.Add(new NodeViewModel(node, NodeCount + 1));
            });
        }
    }
}
