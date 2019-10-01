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
        }

        void RecvTestMsg(IMessage message)
        {
            var msg = (message as Msg.TestMessage);
            Console.WriteLine($"recv msg : tester id ({ID}) : msg - {msg.Content}");
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
