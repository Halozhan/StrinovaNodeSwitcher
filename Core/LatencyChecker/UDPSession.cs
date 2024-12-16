using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core.LatencyChecker
{
    public class UDPSession
    {
        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }

        private IPEndPoint remoteEndPoint;
        public byte[] sendBytes;

        public UDPSession(IPAddress iPAddress, int port)
        {
            IPAddress = iPAddress;
            Port = port;

            // 목적지
            remoteEndPoint = new IPEndPoint(IPAddress, Port);

            // 보낼 패킷
            sendBytes = Encoding.ASCII.GetBytes("a");

        }

        public void SendData(UdpClient udpClient)
        {
            udpClient.Send(sendBytes, sendBytes.Length);
        }

        public byte[] ReceiveData(UdpClient udpClient)
        {
            return udpClient.Receive(ref remoteEndPoint);
        }
    }
}
