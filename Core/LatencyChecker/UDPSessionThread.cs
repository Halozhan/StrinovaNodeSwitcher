using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Core.LatencyChecker
{
    public class UDPSessionThread
    {
        private readonly UDPSession _session;
        private CancellationTokenSource? _cancellationTokenSource;

        public UDPSessionThread(UDPSession session)
        {
            _session = session;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        public async IAsyncEnumerable<float> Run(CancellationToken token)
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
                        byte[] receiveBytes = await _session.ReceiveDataAsync(udpClient);
                        stopwatch.Stop();


                        // 보낸 패킷과 받은 패킷이 같은지 확인
                        if (Encoding.ASCII.GetString(_session.sendBytes) != Encoding.ASCII.GetString(receiveBytes))
                        {
                            // Wrong packet received
                            yield return -1;
                            lossCountDuringSession++;
                            continue;
                        }

                        // 핑이 1000ms 초과하면
                        if (stopwatch.ElapsedMilliseconds > 1000)
                        {
                            yield return -1;
                            lossCountDuringSession++;
                            continue;
                        }

                        // Add latency to the list
                        yield return stopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000;
                    }
                }
            }
        }
    }
}
