using Core.LatencyChecker;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class ObservableLatency : INotifyPropertyChanged
    {
        private readonly Latency _latency;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableLatency()
        {
            _latency = new();
        }

        public ObservableCollection<float> LatencyList => new(_latency.LatencyList);

        public void Add(float ping)
        {
            _latency.Add(ping);
            OnPropertyChanged(nameof(Average));
            //OnPropertyChanged(nameof(Min));
            //OnPropertyChanged(nameof(Max));
            //OnPropertyChanged(nameof(LossRate));
            //OnPropertyChanged(nameof(StandardDeviation));
            //OnPropertyChanged(nameof(Score));
        }

        public void Clear()
        {
            _latency.Clear();
            OnPropertyChanged(nameof(Average));
            //OnPropertyChanged(nameof(Min));
            //OnPropertyChanged(nameof(Max));
            //OnPropertyChanged(nameof(LossRate));
            //OnPropertyChanged(nameof(StandardDeviation));
            //OnPropertyChanged(nameof(Score));
        }

        public float Average => _latency.LatencyList.Average();
        //public float Min => latencyService.GetMin();
        //public float Max => latencyService.GetMax();
        //public float LossRate => latencyService.GetLossRate();
        //public float StandardDeviation => latencyService.GetStandardDeviation();
        //public float Score => latencyService.GetScore();

        public Latency GetLatency() => _latency;
    }
}
