using Core.LatencyChecker;
using System.Net;

namespace Core.Node
{
    public class Node
    {
        public IPAddress? IPAddress { get; set; }
        public int Port { get; set; }
        public Latency Latency { get; set; }

        public Node(IPAddress address, int port)
        {
            IPAddress = address;
            Port = port;
            Latency = new Latency();
        }
    }
}
