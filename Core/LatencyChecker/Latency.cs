namespace Core.LatencyChecker
{
    public class Latency
    {
        private const int MaxCapacity = 1000;
        private readonly List<float> _latencyList;
        private readonly ReaderWriterLockSlim _lock;

        public Latency()
        {
            _latencyList = new List<float>();
            _lock = new ReaderWriterLockSlim();
        }

        public void Add(float ping)
        {
            _lock.EnterWriteLock();
            try
            {
                _latencyList.Add(ping);
                while (_latencyList.Count > MaxCapacity)
                {
                    _latencyList.RemoveAt(0);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _latencyList.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public List<float> GetLatencyList()
        {
            _lock.EnterReadLock();
            try
            {
                return _latencyList;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _latencyList.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
    }
}
