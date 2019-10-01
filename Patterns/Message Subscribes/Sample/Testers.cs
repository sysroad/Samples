using System;
using Google.Protobuf;

namespace Sample
{
    static class Global
    {
        internal static int testerId = 0;
    }

    class Tester : Lib.SubscriberBase
    {
        public override int ID { get; } = Global.testerId++;

        public Tester()
        {
            Lib.MessageManager.Instance.RegisterID(ID, this);

            Subscribe(Msg.TestMessage.Descriptor.FullName, RecvTestMsg);

            if (ID % 2 == 0) // id 가 짝수면 하나 더 등록
            {
                Subscribe(Msg.TestMessage2.Descriptor.FullName, RecvTestMsg2);
            }
        }

        void RecvTestMsg(IMessage message)
        {
            var msg = (message as Msg.TestMessage);
            Console.WriteLine($"recv TestMessage : tester id ({ID}) : msg - {msg.Content}");
        }

        void RecvTestMsg2(IMessage message)
        {
            var msg = (message as Msg.TestMessage2);
            Console.WriteLine($"recv TestMessage2 : tester id ({ID}) : msg - {msg.Content}");
        }

        public override void Dispatcher(IMessage message)
        {
            var id = message.Descriptor.FullName;
            if (handlers.ContainsKey(id))
            {
                handlers[id].Invoke(message);
            }
        }
    }
}
