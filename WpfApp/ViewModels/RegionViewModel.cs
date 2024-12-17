using CommunityToolkit.Mvvm.ComponentModel;
using Core.Region;

namespace WpfApp.ViewModels
{
    [INotifyPropertyChanged]
    public partial class RegionViewModel
    {
        [ObservableProperty]
        private Region _region;

        public RegionViewModel(Region region)
        {
            Region = region;
        }

        public string Name
        {
            get => Region.Name;
            set => SetProperty(Region.Name, value, Region, (region, name) => region.Name = name);
        }

        public List<NodeViewModel> Nodes
        {
            get => Region.Nodes.Select(node => new NodeViewModel(node)).ToList();
        }
    }
}
