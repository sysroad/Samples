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

            for (int i = 0; i < 4; ++i)
            {
                testers.Add(new Tester());
            }

            Lib.MessageManager.Instance.Start();

            var sender = testers.First();

            Console.WriteLine($"send msg : tester id ({sender.ID})");
            sender.Send(new Msg.TestMessage { Content = "Hello World" });

            Lib.MessageManager.Instance.Stop();
        }
    }
}
