using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using Core.Node;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    [INotifyPropertyChanged]
    public partial class ObservableNode : Node
    {
        public override IPAddress? IPAddress
        {
            get => base.IPAddress;
            set
            {
                base.IPAddress = value;
                OnPropertyChanged();
            }
        }

        public override int Port
        {
            get => base.Port;
            set
            {
                base.Port = value;
                OnPropertyChanged();
            }
        }

        private ObservableLatency _observableLatency;
        public ObservableLatency ObservableLatency
        {
            get => _observableLatency;
            set
            {
                _observableLatency = value;
                Latency = _observableLatency.Latency;
                OnPropertyChanged();
            }
        }

        public ObservableNode(IPAddress address, int port) : base(address, port)
        {
            _observableLatency = new ObservableLatency();
        }
    }
}
