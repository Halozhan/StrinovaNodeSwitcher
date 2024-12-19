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
            latencyService.Update();

            Assert.Equal(150, latencyService.Average);
        }

        [Fact]
        public void GetMinTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);
            latencyService.Update();

            Assert.Equal(100, latencyService.Min);
        }

        [Fact]
        public void GetMaxTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);
            latencyService.Update();

            Assert.Equal(200, latencyService.Max);
        }

        [Fact]
        public void GetLossRateTestWithLoss()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(-1);
            latencyService.Update();

            Assert.Equal(0.5, latencyService.LossRate);
        }

        [Fact]
        public void GetLossRateTestNoLoss()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);
            latencyService.Update();

            Assert.Equal(0, latencyService.LossRate);
        }

        [Fact]
        public void GetStandardDeviationTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);
            latencyService.Update();

            Assert.Equal(50, latencyService.StandardDeviation);
        }

        [Fact]
        public void GetScoreTest()
        {
            Latency latency = new();
            LatencyService latencyService = new(latency);
            latency.Add(100);
            latency.Add(200);
            latencyService.Update();

            var average = (100 + 200) / 2;
            var standardDeviation = 50;
            var lossRate = 0;
            var score = (average + standardDeviation) * Math.Pow(50, 0.01 * lossRate);

            Assert.Equal(score, latencyService.Score);
        }
    }
}
