using Core.LatencyChecker;

namespace TestCore
{
    public class UDPSessionTest
    {
        [Fact]
        public async void SessionTest()
        {
            // Strinova South Korea Edge-One Global Accelerator IP
            var address = System.Net.IPAddress.Parse("43.155.193.230");
            var port = 20000;

            List<float> latency = [];

            UDPSession thread = new(address, port, latency.Add, () => { });

            // Start the thread
            thread.Start();

            // Collect data for 100 pings
            while (latency.Count < 100)
            {
                await Task.Delay(20);
            }

            // Stop the thread
            thread.Stop();


            Assert.True(latency.Average() > 0);
            Assert.True(latency.Average() < 1000);
        }
    }
}
