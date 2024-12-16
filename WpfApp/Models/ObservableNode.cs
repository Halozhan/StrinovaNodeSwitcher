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
    public class ObservableNode : Node, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Address
        {
            get => base.IPAddress.ToString();
            set
            {
                base.IPAddress = IPAddress.Parse(value);
                OnPropertyChanged();
            }
        }

        public new int Port
        {
            get => base.Port;
            set
            {
                base.Port = value;
                OnPropertyChanged();
            }
        }

        private ObservableLatency _latency;
        public new ObservableLatency Latency
        {
            get => _latency;
            set
            {
                _latency = value;
                OnPropertyChanged();
            }
        }

        public ObservableNode(IPAddress address, int port) : base(address, port)
        {
            _latency = new ObservableLatency();
        }
    }
}
