using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Core.LatencyChecker
{
    public class UDPSessionThread
    {
        private readonly UDPSession _session;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Thread _thread;

        public UDPSessionThread(UDPSession session, Action<float> latencyAppend, Action notifyEvent)
        {
            _session = session;
            _cancellationTokenSource = new CancellationTokenSource();
            _thread = new Thread(() => Run(_cancellationTokenSource.Token, latencyAppend, notifyEvent))
            {
                IsBackground = true
            };
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        // 취소 토큰과 삽입할 데이터의 콜백 함수를 받아서 실행
        private void Run(CancellationToken token, Action<float> latencyAppend, Action notifyEvent)
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
                        // 패킷 손실이 lossCountDuringSession 조건을 만족하면 재연결
                        if (lossCountDuringSession >= 10)
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
                            byte[] receiveBytes = _session.ReceiveData(udpClient);
                            stopwatch.Stop();

                            // 보낸 패킷과 받은 패킷이 같은지 확인
                            if (Encoding.ASCII.GetString(_session.sendBytes) != Encoding.ASCII.GetString(receiveBytes))
                            {
                                // Wrong packet received
                                lossCountDuringSession++;
                                latencyAppend(-1);
                                notifyEvent();
                                continue;
                            }

                            // 핑이 1000ms 초과하면
                            if (stopwatch.ElapsedMilliseconds > 1000)
                            {
                                lossCountDuringSession++;
                                latencyAppend(-1);
                                notifyEvent();
                                continue;
                            }
                            // Add latency to the list
                            latencyAppend(stopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000);
                            notifyEvent();

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error during UDP session: {ex.Message}");
                            lossCountDuringSession++;
                        }

                        // Calculate delay
                        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        if (elapsedMilliseconds < 200)
                        {
                            Task.Delay(200 - (int)elapsedMilliseconds, token);
                        }
                    }
                }
            }
        }
    }
}
