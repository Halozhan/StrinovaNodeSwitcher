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

        private readonly UDPSessionThread _udpSessionThread;

        public NodeViewModel(Node node)
        {
            Node = node;
            LatencyService = new LatencyService(Node.Latency);

            UDPSession session = new(Node.IPAddress, Node.Port);
            _udpSessionThread = new UDPSessionThread(session, Node.Latency.Add, Latency_CollectionChanged);
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

        public string Address
        {
            get => Node.IPAddress.ToString();
        }

        public void StartSession()
        {
            _udpSessionThread.Start();
        }

        public void StopSession()
        {
            _udpSessionThread.Stop();
        }

        public float Average
        {
            get => LatencyService.GetAverage();
        }

        public float Min
        {
            get => LatencyService.GetMin();
        }

        public float Max
        {
            get => LatencyService.GetMax();
        }

        public float LossRate
        {
            get => LatencyService.GetLossRate();
        }

        public float StandardDeviation
        {
            get => LatencyService.GetStandardDeviation();
        }

        public float Score
        {
            get => LatencyService.GetScore();
        }
    }
}
