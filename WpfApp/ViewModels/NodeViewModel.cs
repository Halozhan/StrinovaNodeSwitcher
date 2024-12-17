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

        public float Latency
        {
            get => LatencyService.GetAverage();
        }

        public async void StartSessionAsync()
        {
            _udpSessionThread.Start();
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await foreach (var ping in _udpSessionThread.Run(_cancellationTokenSource.Token))
                {
                    Node.Latency.Add(ping);
                    OnPropertyChanged(nameof(Latency));
                    OnPropertyChanged(nameof(LossRate));
                }
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

        public float LossRate
        {
            get => LatencyService.GetLossRate();
        }
    }
}
