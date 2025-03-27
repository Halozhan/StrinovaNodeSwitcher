using System.Diagnostics;

namespace Core.HostsManager
{
    public sealed class HostsManager
    {
        private static readonly Lazy<HostsManager> instance = new(() => new HostsManager());

        private List<(string, Host?)> hostsList = [];
        private static readonly object hostsListLock = new();

        private HostsManager() { }

        // Singleton pattern
        public static HostsManager Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public static HostsManager GetInstance()
        {
            return instance.Value;
        }

        public void LoadHosts()
        {
            lock (hostsListLock)
            {
                var fileHandler = HostsFileHandler.GetInstance();
                var readLine = fileHandler.ReadHosts();
                hostsList = HostsParser.ParseTextToHostsList(readLine);
            }
        }

        public string? GetIPByDomain(string domain)
        {
            lock (hostsListLock)
            {
                foreach (var (line, host) in hostsList)
                {
                    if (host?.Hostname == domain)
                    {
                        return host.IP;
                    }
                }
                return null;
            }
        }

        public void AddOrChangeHost(Host host)
        {
            lock (hostsListLock)
            {
                foreach (var (_, compareHost) in hostsList)
                {
                    if (compareHost?.Hostname == host.Hostname)
                    {
                        compareHost.IP = host.IP;
                        return;
                    }
                }
                hostsList.Add(("", host));
            }
        }

        public void RemoveHostByDomain(string domain)
        {
            lock (hostsListLock)
            {
                foreach (var (line, host) in hostsList)
                {
                    if (host?.Hostname == domain)
                    {
                        hostsList.Remove((line, host));
                    }
                }
            }
        }

        public void UpdateHostsFile()
        {
            lock (hostsListLock)
            {
                var fileHandler = HostsFileHandler.GetInstance();
                var serializedLines = HostsParser.SerializeHostsListToText(hostsList);
                fileHandler.WriteHosts(serializedLines);
            }
        }
    }
}
