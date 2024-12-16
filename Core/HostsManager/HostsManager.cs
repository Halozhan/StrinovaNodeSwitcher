using System.Diagnostics;

namespace Core.HostsManager
{
    public sealed class HostsManager
    {
        private static readonly Lazy<HostsManager> instance = new Lazy<HostsManager>(() => new HostsManager());
        private static readonly object fileLock = new object();
        private string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";

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

        public void SetHostsPath(string path)
        {
            lock (fileLock)
            {
                hostsPath = path;
            }
        }

        private string[] ReadHosts()
        {
            string[] lines;

            lock (fileLock)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(hostsPath))
                    {
                        lines = reader.ReadToEnd().Split('\n');
                    }
                    return lines;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            throw new Exception("Error reading hosts file");
        }

        public string? GetIPByDomain(string domain)
        {
            string[] lines = ReadHosts();
            foreach (var line in lines)
            {
                if (line.Contains(domain))
                {
                    return line.Split('\t')[0];
                }
            }
            return null;
        }

        private void AddHost(Host host)
        {
            lock (fileLock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(hostsPath, true))
                    {
                        writer.WriteLine(host.IP + "\t" + host.Hostname);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void RemoveHostByDomain(string domain)
        {
            lock (fileLock)
            {
                string[] lines = ReadHosts();
                try
                {
                    using (StreamWriter writer = new StreamWriter(hostsPath))
                    {
                        foreach (var line in lines)
                        {
                            if (!line.Contains(domain))
                            {
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public void ChangeHost(Host host)
        {
            if (GetIPByDomain(host.Hostname) != null)
            {
                RemoveHostByDomain(host.Hostname);
            }
            AddHost(host);
        }
    }
}
