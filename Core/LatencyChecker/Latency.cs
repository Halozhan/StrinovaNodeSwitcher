namespace Core.LatencyChecker
{
    public class Latency
    {
        public Latency()
        {
            LatencyList = new(1000);
        }

        public readonly List<float> LatencyList;

        public void Add(float ping)
        {
            if (LatencyList.Count >= 1000)
            {
                LatencyList.RemoveAt(0);
            }
            LatencyList.Add(ping);
        }

        public void Clear()
        {
            LatencyList.Clear();
        }
    }
}
