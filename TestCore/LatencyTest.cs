using Core.LatencyChecker;
using Xunit.Sdk;

namespace TestCore
{
    public class LatencyTest
    {
        //[Fact]
        //public void AddTest()
        //{
        //    var latency = new Latency();
        //    latency.Add(100);
        //    latency.Add(200);
        //    Assert.Equal((100 + 200) / 2, latency.GetAverage());
        //}

        //[Fact]
        //public void AddTestLarge()
        //{
        //    var latency = new Latency();
        //    // 100으로 1000개 채우고
        //    for (int i = 0; i < 1001; i++)
        //    {
        //        latency.Add(100);
        //    }

        //    // 200으로 1000개 채우면
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        latency.Add(200);
        //    }

        //    // 리스트는 1000개만 저장하므로 1000개의 200만 남아있어야 함
        //    Assert.Equal(200, latency.GetAverage());
        //}

        //[Fact]
        //public void ClearTest()
        //{
        //    var latency = new Latency();
        //    latency.Add(100);
        //    latency.Add(200);
        //    latency.Clear();
        //    Assert.Equal(-1, latency.GetAverage());
        //}

        //[Fact]
        //public void GetMinTest()
        //{
        //    var latency = new Latency();
        //    latency.Add(100);
        //    latency.Add(200);
        //    Assert.Equal(100, latency.GetMin());
        //}

        //[Fact]
        //public void GetMaxTest()
        //{
        //    var latency = new Latency();
        //    latency.Add(100);
        //    latency.Add(200);
        //    Assert.Equal(200, latency.GetMax());
        //}
    }
}
