using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using Core.Node;
using System.ComponentModel;

namespace WpfApp.ViewModels
{
    public partial class NodeViewModel : ObservableObject
    {
        [ObservableProperty]
        private Node _node;

        public string IPAddress => Node.IPAddress.ToString();
        public int Port => Node.Port;

        public LatencyViewModel Latency { get; set; }

        [ObservableProperty]
        private LatencyService _latencyService;

        private readonly UDPSession _udpSession;

        public NodeViewModel(Node node)
        {
            Node = node;
            Latency latency = new();
            Latency = new LatencyViewModel(latency);
            LatencyService = new LatencyService(latency);

            _udpSession = new UDPSession(Node.IPAddress, Node.Port, Latency.Add, Latency_CollectionChanged);
            StartSession();
        }

        private void Latency_CollectionChanged()
        {
            OnPropertyChanged(nameof(Average));
            OnPropertyChanged(nameof(Min));
            OnPropertyChanged(nameof(Max));
            OnPropertyChanged(nameof(LossRate));
            OnPropertyChanged(nameof(StandardDeviation));
            OnPropertyChanged(nameof(Score));
        }

        public void StartSession()
        {
            _udpSession.Start();
        }

        public void StopSession()
        {
            _udpSession.Stop();
        }

        public string Address => Node.IPAddress.ToString();

        public float Average => LatencyService.GetAverage();
        public float Min => LatencyService.GetMin();
        public float Max => LatencyService.GetMax();
        public float LossRate => LatencyService.GetLossRate();
        public float StandardDeviation => LatencyService.GetStandardDeviation();
        public float Score => LatencyService.GetScore();
    }
}
