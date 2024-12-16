using Core.Region;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public class RegionViewModel : ObservableObject
    {
        public RegionViewModel(Region region)
        {
            _region = region;
        }

        private Region _region;
        public Region Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }
    }
}
