using Core.LatencyChecker;

namespace TestCore
{
    public class UDPSessionThreadTest
    {
        [Fact]
        public async void ThreadTest()
        {
            // Strinova South Korea Edge-One Global Accelerator IP
            var address = System.Net.IPAddress.Parse("43.155.193.230");
            var port = 20000;

            List<float> latency = [];

            UDPSession session = new UDPSession(address, port);
            UDPSessionThread thread = new UDPSessionThread(session);

            // Start the thread
            thread.Start();

            // Collect data for 100 pings
            var token = new CancellationTokenSource();
            await foreach (var ping in thread.Run(token.Token))
            {
                latency.Add(ping);
                if (latency.Count >= 100)
                {
                    break;
                }
            }

            // Stop the thread
            thread.Stop();


            Assert.True(latency.Average() > 0);
            Assert.True(latency.Average() < 1000);
        }
    }
}
