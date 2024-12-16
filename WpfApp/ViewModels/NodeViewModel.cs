using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using Core.Node;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    [INotifyPropertyChanged]
    public partial class NodeViewModel
    {
        //private readonly ObservableNode _node;
        [ObservableProperty]
        private Node _node;

        //private ObservableLatency _latency;
        //[ObservableProperty]
        //private Latency _latency;

        private UDPSessionThread _udpSessionThread;
        private CancellationTokenSource _cancellationTokenSource;

        public NodeViewModel(ObservableNode node)
        {
            Node = node;
            //_latency = new();

            var session = new UDPSession(Node.IPAddress, Node.Port);
            _udpSessionThread = new UDPSessionThread(session);
            StartSessionAsync();
        }

        public string Address
        {
            get => Node.IPAddress.ToString();
        }

        public float Latency
        {
            get
            {
                if (Node.Latency.LatencyList.Count == 0)
                {
                    return 0;
                }
                return Node.Latency.LatencyList.Average();
            }
        }

        public async void StartSessionAsync()
        {
            _udpSessionThread.Start();
            _cancellationTokenSource = new CancellationTokenSource();
            await foreach (var ping in _udpSessionThread.Run(_cancellationTokenSource.Token))
            {
                Node.Latency.Add(ping);
                OnPropertyChanged(nameof(Latency));
            }
        }

        public void StopSession()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
