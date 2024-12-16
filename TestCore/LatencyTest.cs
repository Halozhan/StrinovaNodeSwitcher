using Core.LatencyChecker;

namespace TestCore
{
    public class LatencyTest
    {
        [Fact]
        public void AddTest()
        {
            Latency latency = new();
            latency.Add(100);
            latency.Add(200);
            Assert.Equal(2, latency.LatencyList.Count);
        }

        [Fact]
        public void AddManyTest()
        {
            Latency latency = new();
            for (int i = 0; i < 2000; i++)
            {
                latency.Add(100);
            }

            Assert.Equal(1000, latency.LatencyList.Count);
        }

        [Fact]
        public void ClearTest()
        {
            Latency latency = new();
            latency.Add(100);
            latency.Add(200);
            latency.Clear();
            Assert.Empty(latency.LatencyList);
        }
    }
}
