using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class SampleClass
    {
        public readonly DateTime time;

        SampleClass()
        {
            time = DateTime.Now;
        }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSingleInstance()
        {
            var t1 = DateTime.Now;

            var tk = Task.Factory.StartNew(() =>
            {
                SpinWait.SpinUntil(() => false, 1000);
            }).ContinueWith(t =>
            {
                var t2 = Utility.TSingle<SampleClass>.Instance.time;

                Assert.IsTrue((t2 - t1).TotalSeconds > 0.9);
            });

            tk.Wait();
        }
    }
}