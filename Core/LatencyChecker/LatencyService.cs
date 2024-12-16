using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.LatencyChecker
{
    public class LatencyService
    {
        private readonly List<float> LatencyList;

        public LatencyService(Latency latency)
        {
            LatencyList = latency.LatencyList;
        }

        public float GetAverage()
        {
            if (LatencyList.Count == 0) return -1;
            return LatencyList.Average();
        }

        public float GetMin()
        {
            if (LatencyList.Count == 0) return -1;
            return LatencyList.Min();
        }

        public float GetMax()
        {
            if (LatencyList.Count == 0) return -1;
            return LatencyList.Max();
        }

        public float GetLossRate()
        {
            if (LatencyList.Count == 0) return -1;
            return LatencyList.Count(ping => ping == -1) / LatencyList.Count;
        }

        public float GetStandardDeviation()
        {
            if (LatencyList.Count == 0) return -1;
            return (float)Math.Sqrt(LatencyList.Average(ping => Math.Pow(ping - GetAverage(), 2)));
        }

        public float GetScore()
        {
            if (LatencyList.Count == 0) return -1;
            return (float)((GetAverage() + GetStandardDeviation()) * Math.Pow(50, 0.01 * GetLossRate()));
        }
    }
}
