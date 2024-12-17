using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using Core.Node;
using System.ComponentModel;
using System.Diagnostics;

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

            UDPSession session = new(Node.IPAddress, Node.Port);
            _udpSessionThread = new UDPSessionThread(session);
            StartSessionAsync();
        }

        public string Address
        {
            get => Node.IPAddress.ToString();
        }

        public async Task StartSessionAsync()
        {
            _udpSessionThread.Start();
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                // TODO: To be refactored
                await Task.Run(async () =>
                {
                    await foreach (var ping in _udpSessionThread.Run(_cancellationTokenSource.Token))
                    {
                        Node.Latency.Add(ping);
                        OnPropertyChanged(nameof(Average));
                        OnPropertyChanged(nameof(Min));
                        OnPropertyChanged(nameof(Max));
                        OnPropertyChanged(nameof(LossRate));
                        OnPropertyChanged(nameof(StandardDeviation));
                        OnPropertyChanged(nameof(Score));
                    }
                });
            }
            catch (TaskCanceledException)
            {
                // Task is cancelled
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _udpSessionThread.Stop();
                Debug.WriteLine("스레드가 종료됨");
            }
        }

        public void StopSession()
        {
            _cancellationTokenSource.Cancel();
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
