using Core.LatencyChecker;

namespace TestCore
{
    public class UDPSessionTest
    {
        [Fact]
        public async void SessionTest()
        {
            // Strinova server
            var address = System.Net.IPAddress.Parse("43.155.193.230");
            var port = 20000;

            List<float> latency = [];

            async Task LatencyAppend(float value)
            {
                latency.Add(value);
                await Task.CompletedTask;
            }

            UDPSession thread = new(address, port, LatencyAppend);

            // Start the thread
            thread.Start();

            await Task.Delay(5000);

            // Stop the thread
            thread.Stop();

            Assert.True(latency.Average() > 0);
            Assert.True(latency.Average() < 1000);
        }
    }
}
