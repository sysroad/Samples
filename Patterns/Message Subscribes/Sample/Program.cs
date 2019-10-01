using System;
using System.Linq;
using System.Collections.Generic;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Tester> testers = new List<Tester>();

            for (int i = 0; i <= 10; ++i)
            {
                testers.Add(new Tester());
            }

            Lib.MessageManager.Instance.Start();

            var sender = testers.First();

            Console.WriteLine($"send TestMessage : tester id ({sender.ID})");
            sender.Send(new Msg.TestMessage { Content = "Hello World" });
            Console.WriteLine($"send TestMessage2 : tester id ({sender.ID})");
            sender.Send(new Msg.TestMessage2 { Content = "Hello World2" });

            Console.WriteLine($"send TestMessage : anonymous sender");
            Lib.MessageManager.Instance.Send(new Msg.TestMessage { Content = "Who am I?" });

            Lib.MessageManager.Instance.Stop();
        }
    }
}
