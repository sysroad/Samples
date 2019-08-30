using NUnit.Framework;
using Akka.Actor;
using System.Threading;

namespace AkkaDotNet.Singleton.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
           ActorSystem.Instance.ActorOf<TestActor>("test");
        }

        [Test]
        public void ActorMessageRecvTest()
        {
            var eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            ActorSystem.Instance.ActorSelection("/user/test").Tell(new TestMessage
            {
                WaitHandle = eventWait
            });
            Assert.IsTrue(eventWait.WaitOne(1000));
        }
    }

    public class TestMessage
    {
        public EventWaitHandle WaitHandle { get; set; }
    }

    public class TestActor : ReceiveActor
    {
        public TestActor()
        {
            Receive<TestMessage>(m =>
            {
                m.WaitHandle.Set();
            });
        }
    }
}