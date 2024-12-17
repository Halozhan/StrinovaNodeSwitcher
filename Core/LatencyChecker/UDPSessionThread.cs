using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

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

        public async IAsyncEnumerable<float> Run([EnumeratorCancellation] CancellationToken token)
        {
            Stopwatch stopwatch = new();

            while (!token.IsCancellationRequested)
            {
                using (UdpClient udpClient = new())
                {
                    int lossCountDuringSession = 0;
                    udpClient.Client.ReceiveTimeout = 1000; // 1초 타임아웃 설정
                    udpClient.Connect(_session.IPAddress, _session.Port);
                    for (int i = 0; (i < 50) && !token.IsCancellationRequested; i++)
                    {
                        float result = -1;
                        // 패킷 손실이 3회 이상이면 재연결
                        if (lossCountDuringSession >= 3)
                        {
                            break;
                        }

                        try
                        {
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
                                result = -1;
                                lossCountDuringSession++;
                                continue;
                            }

                            // 핑이 1000ms 초과하면
                            else if (stopwatch.ElapsedMilliseconds > 1000)
                            {
                                result = -1;
                                lossCountDuringSession++;
                            }
                            // Add latency to the list
                            else
                            {
                                result = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000;
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error during UDP session: {ex.Message}");
                            lossCountDuringSession++;
                        }

                        yield return result;
                    }
                }
            }
        }
    }
}
