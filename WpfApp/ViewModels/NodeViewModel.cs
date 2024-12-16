using Core.LatencyChecker;
using System.Collections.ObjectModel;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class NodeViewModel
    {
        private readonly ObservableNode _node;
        private UDPSessionThread _udpSessionThread;

        public NodeViewModel(ObservableNode node)
        {
            _node = node;
            var session = new UDPSession(node.IPAddress, node.Port);
            _udpSessionThread = new UDPSessionThread(session);
            StartSession();
        }

        public string Address
        {
            get => _node.Address;
            set => _node.Address = value;
        }

        public float Latency
        {
            get => _node.Latency.Average;
        }

        public async void StartSession()
        {
            _udpSessionThread.Start();
        }

        public void StopSession()
        {
            _udpSessionThread.Stop();
        }
    }
}
