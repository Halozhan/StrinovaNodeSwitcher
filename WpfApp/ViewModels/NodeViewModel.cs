using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using Core.Node;
using System.ComponentModel;

namespace WpfApp.ViewModels
{
    [INotifyPropertyChanged]
    public partial class NodeViewModel
    {
        [ObservableProperty]
        private Node _node;

        [ObservableProperty]
        private LatencyService _latencyService;

        private UDPSessionThread _udpSessionThread;
        private CancellationTokenSource _cancellationTokenSource;

        public NodeViewModel(Node node)
        {
            Node = node;
            LatencyService = new LatencyService(Node.Latency);

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
            get => LatencyService.GetAverage();
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
