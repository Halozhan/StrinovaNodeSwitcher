using Core.LatencyChecker;

namespace TestCore
{
    public class LatencyServiceTest
    {
        [Fact]
        public void GetAverageTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);

            Assert.Equal(150, latencyService.GetAverage());
        }

        [Fact]
        public void GetMinTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);

            Assert.Equal(100, latencyService.GetMin());
        }

        [Fact]
        public void GetMaxTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);

            Assert.Equal(200, latencyService.GetMax());
        }

        [Fact]
        public void GetLossRateTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(-1);

            Assert.Equal(0.5, latencyService.GetLossRate());
        }

        [Fact]
        public void GetStandardDeviationTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);

            Assert.Equal(50, latencyService.GetStandardDeviation());
        }

        [Fact]
        public void GetScoreTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);

            var average = (100 + 200) / 2;
            var standardDeviation = 50;
            var lossRate = 0;
            var score = (average + standardDeviation) * Math.Pow(50, 0.01 * lossRate);

            Assert.Equal(score, latencyService.GetScore());
        }
    }
}
