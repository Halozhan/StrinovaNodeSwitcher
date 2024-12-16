using CommunityToolkit.Mvvm.ComponentModel;
using Core.LatencyChecker;
using System.ComponentModel;

namespace WpfApp.Models
{
    [INotifyPropertyChanged]
    public partial class ObservableLatency
    {
        [ObservableProperty]
        private Latency _latency;

        [ObservableProperty]
        private LatencyService latencyService;

        public ObservableLatency()
        {
            Latency = new();
            LatencyService = new(Latency);
        }


        public void Add(float ping)
        {
            Latency.Add(ping);
            OnPropertyChanged(nameof(Average));
            //OnPropertyChanged(nameof(Min));
            //OnPropertyChanged(nameof(Max));
            //OnPropertyChanged(nameof(LossRate));
            //OnPropertyChanged(nameof(StandardDeviation));
            //OnPropertyChanged(nameof(Score));
        }

        public void Clear()
        {
            Latency.Clear();
            OnPropertyChanged(nameof(Average));
            //OnPropertyChanged(nameof(Min));
            //OnPropertyChanged(nameof(Max));
            //OnPropertyChanged(nameof(LossRate));
            //OnPropertyChanged(nameof(StandardDeviation));
            //OnPropertyChanged(nameof(Score));
        }

        public float Average
        {
            get
            {
                if (Latency.LatencyList.Count == 0)
                {
                    return 0;
                }
                return LatencyService.GetAverage();
            }
        }
        //public float Min => latencyService.GetMin();
        //public float Max => latencyService.GetMax();
        //public float LossRate => latencyService.GetLossRate();
        //public float StandardDeviation => latencyService.GetStandardDeviation();
        //public float Score => latencyService.GetScore();
    }
}
