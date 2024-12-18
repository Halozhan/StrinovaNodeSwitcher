using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using System.Collections.Concurrent;
using System.Dynamic;

namespace WpfApp.ViewModels
{
    public partial class LatencyViewModel : ObservableObject
    {
        private Latency _latency;

        public LatencyViewModel(Latency latency)
        {
            _latency = latency;
        }

        public void Add(float ping)
        {
            _latency.Add(ping);
        }

        public void Clear()
        {
            _latency.Clear();
        }

        public ConcurrentQueue<float> GetLatencyList()
        {
            return _latency.GetLatencyList();
        }
    }
}
