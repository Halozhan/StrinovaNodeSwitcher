namespace Core.LatencyChecker
{
    public class LatencyService
    {
        private readonly List<float> LatencyList;

        public LatencyService(Latency latency)
        {
            LatencyList = latency.LatencyList;
        }

        // 이상치 제거
        private List<float> RemoveOutlier()
        {
            if (LatencyList.Count == 0) return [];
            // 0 미만이거나 1000 초과인 값 제거
            return LatencyList.Where(ping => ping >= 0 && ping <= 1000).ToList();
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
            if (LatencyList.Count == 0) return -1;
            return (float)LatencyList.Count(ping => ping == -1) / LatencyList.Count;
        }

        public float GetStandardDeviation()
        {
            var list = RemoveOutlier();
            if (list.Count == 0) return -1;
            return (float)Math.Sqrt(LatencyList.Average(ping => Math.Pow(ping - GetAverage(), 2)));
        }

        public float GetScore()
        {
            if (LatencyList.Count == 0) return -1;
            return (float)((GetAverage() + GetStandardDeviation()) * Math.Pow(50, 0.01 * GetLossRate()));
        }
    }
}
