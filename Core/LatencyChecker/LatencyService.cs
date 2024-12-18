namespace Core.LatencyChecker
{
    public class LatencyService(Latency latency)
    {
        private readonly Latency _latency = latency;

        // 이상치 제거
        private List<float> RemoveOutlier()
        {
            if (_latency.GetLatencyList().IsEmpty) return [];
            // 0 미만이거나 1000 초과인 값 제거
            return _latency.GetLatencyList().Where(ping => ping >= 0 && ping <= 1000).ToList();
        }

        public float GetAverage()
        {
            var list = RemoveOutlier();
            if (list.Count == 0) return -1;
            return list.Average();
        }

        public float GetMin()
        {
            var list = RemoveOutlier();
            if (list.Count == 0) return -1;
            return list.Min();
        }

        public float GetMax()
        {
            var list = RemoveOutlier();
            if (list.Count == 0) return -1;
            return list.Max();
        }

        public float GetLossRate()
        {
            var list = _latency.GetLatencyList();
            if (list.Count == 0) return -1;
            return (float)list.Count(ping => ping == -1) / list.Count;
        }

        public float GetStandardDeviation()
        {
            var list = RemoveOutlier();
            if (list.Count == 0) return -1;
            return (float)Math.Sqrt(list.Average(ping => Math.Pow(ping - GetAverage(), 2)));
        }

        public float GetScore()
        {
            if (_latency.GetLatencyList().Count == 0) return -1;
            return (float)((GetAverage() + GetStandardDeviation()) * Math.Pow(50, 0.01 * GetLossRate()));
        }
    }
}
