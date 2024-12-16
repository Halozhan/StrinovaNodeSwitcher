using Core.LatencyChecker;
using Core.Node;
using System.Threading;
using System.Xml.Linq;

namespace TestCore
{
    public class UDPSessionThreadTest
    {
        [Fact]
        public void ThreadTest()
        {
            // Strinova South Korea Edge-One Global Accelerator IP
            var address = System.Net.IPAddress.Parse("43.155.193.230");
            var port = 20000;

            List<float> latency = [];

            UDPSession session = new UDPSession(address, port);
            UDPSessionThread thread = new UDPSessionThread(session);

            // Start the thread
            thread.Start();

            // Wait for 2 seconds
            Thread.Sleep(2000);

            // Stop the thread
            thread.Stop();
            thread.latency.ForEach(latency.Add);

            Assert.True(latency.Average() < 0);
            Assert.True(latency.Average() < 500);
        }
    }
}
