using Core.LatencyChecker;
using System.Net;

namespace Core.Node
{
    public class Node
    {
        public virtual IPAddress? IPAddress { get; set; }
        public virtual int Port { get; set; }
        public virtual Latency Latency { get; set; }

        public Node(IPAddress address, int port)
        {
            IPAddress = address;
            Port = port;
            Latency = new Latency();
        }
    }
}
