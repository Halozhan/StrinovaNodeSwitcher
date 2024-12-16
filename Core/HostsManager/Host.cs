using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HostsManager
{
    public class Host(string ip, string hostname)
    {
        public string IP { get; set; } = ip;
        public string Hostname { get; set; } = hostname;
    }
}
