using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Core.LatencyChecker
{
    public class UDPSessionThread
    {
        private readonly UDPSession _session;
        public List<float> latency;
        private Thread thread;
        private CancellationTokenSource _cancellationTokenSource;

        public UDPSessionThread(UDPSession session)
        {
            _session = session;
            latency = [];
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            thread = new Thread(() => Run(_cancellationTokenSource.Token))
            {
                IsBackground = true
            };
            thread.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            thread?.Join();
        }

        private void Run(CancellationToken token)
        {
            Stopwatch stopwatch = new();

            while (!token.IsCancellationRequested)
            {
                using (UdpClient udpClient = new())
                {
                    int lossCountDuringSession = 0;
                    udpClient.Connect(_session.IPAddress, _session.Port);
                    for (int i = 0; (i < 50) && !token.IsCancellationRequested; i++)
                    {
                        // 패킷 손실이 3회 이상이면 재연결
                        if (lossCountDuringSession >= 3)
                        {
                            break;
                        }

                        // Send data
                        _session.SendData(udpClient);
                        stopwatch.Reset();
                        stopwatch.Start();

                        // Receive data
                        byte[] receiveBytes = _session.ReceiveData(udpClient);
                        stopwatch.Stop();


                        // 보낸 패킷과 받은 패킷이 같은지 확인
                        if (Encoding.ASCII.GetString(_session.sendBytes) != Encoding.ASCII.GetString(receiveBytes))
                        {
                            // Wrong packet received
                            latency.Add(-1);
                            lossCountDuringSession++;
                            continue;
                        }

                        // 핑이 1000ms 초과하면
                        if (stopwatch.ElapsedMilliseconds > 1000)
                        {
                            latency.Add(-1);
                            lossCountDuringSession++;
                            continue;
                        }

                        // Add latency to the list
                        latency.Add(stopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000);
                    }
                }
            }
        }
    }
}
