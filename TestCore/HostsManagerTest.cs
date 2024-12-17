using Core.HostsManager;

namespace TestCore
{
    public class HostsManagerTest
    {
        private readonly string hostsPath = @"hosts";

        void BeforeTestStarting()
        {
            using (File.Create(hostsPath)) { }
            var hostsManager = HostsManager.GetInstance();
            hostsManager.SetHostsPath(hostsPath);
        }

        [Fact]
        public void GetIPByDomainTest()
        {
            BeforeTestStarting();
            var hostsManager = HostsManager.GetInstance();
            var host = new Host("127.0.0.1", "localhost");

            hostsManager.ChangeHost(host);
            var ip = hostsManager.GetIPByDomain("localhost");

            Assert.Equal(host.IP, ip);
            AfterTestFinished();
        }

        [Fact]
        public void ChangeHostTest()
        {
            BeforeTestStarting();
            var hostsManager = HostsManager.GetInstance();
            var host = new Host("192.168.0.1", "test.com");

            hostsManager.ChangeHost(host);

            var ip = hostsManager.GetIPByDomain("test.com");

            Assert.Equal(host.IP, ip);
            AfterTestFinished();
        }

        void AfterTestFinished()
        {
            File.Delete(hostsPath);
        }
    }
}