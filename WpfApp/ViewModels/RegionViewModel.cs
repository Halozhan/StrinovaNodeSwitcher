using CommunityToolkit.Mvvm.ComponentModel;
using Core.Node;
using Core.Region;
using System.Collections.ObjectModel;

namespace WpfApp.ViewModels
{
    public partial class RegionViewModel : ObservableObject
    {
        //private readonly Region _region;
        public string Name { get; set; }
        public ObservableCollection<NodeViewModel> Nodes { get; set; }

        public RegionViewModel(Region region)
        {
            Name = region.Name;
            Nodes = [];

            foreach (var node in region.Nodes)
            {
                Nodes.Add(new NodeViewModel(node));
            }
        }

        public void Add(Node node)
        {
            Nodes.Add(new NodeViewModel(node));
        }
    }
}
