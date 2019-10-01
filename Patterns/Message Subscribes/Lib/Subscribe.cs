#define Parallel_Dispatch

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Lib
{
    public interface ISubscriber
    {
        int ID { get; }
        void Dispatcher(IMessage message);
    }

    public abstract class SubscriberBase : ISubscriber
    {
        public virtual int ID { get { return -1; } }

#if Parallel_Dispatch
        protected readonly ConcurrentDictionary<string, Action<IMessage>> handlers = new ConcurrentDictionary<string, Action<IMessage>>();
#else
        protected readonly Dictionary<string, Action<IMessage>> handlers = new Dictionary<string, Action<IMessage>>();
#endif

        public virtual void Dispatcher(IMessage message)
        {
            var id = message.Descriptor.FullName;
            if (handlers.ContainsKey(id))
            {
                handlers[id].Invoke(message);
            }
        }

        public virtual void Subscribe(string msgId, Action<IMessage> handler)
        {
            MessageManager.Instance.Subscribe(ID, msgId);
            handlers.TryAdd(msgId, handler);
        }

        public virtual void Send(IMessage message)
        {
            MessageManager.Instance.Send(ID, message);
        }
    }

    public class MessageManager : TSingle<MessageManager>
    {
        class InternalMessage
        {
            internal int Sender { get; set; }
            internal IMessage Message { get; set; }
        }

        ConcurrentQueue<InternalMessage> MessageQ { get; } = new ConcurrentQueue<InternalMessage>();
#if Parallel_Dispatch
        ConcurrentDictionary<int, ISubscriber> Subscribers { get; } = new ConcurrentDictionary<int, ISubscriber>();
        ConcurrentDictionary<string, List<int>> MsgSubscriptionMap { get; } = new ConcurrentDictionary<string, List<int>>();
#else
        Dictionary<int, ISubscriber> Subscribers { get; } = new Dictionary<int, ISubscriber>();
        Dictionary<string, List<int>> MsgSubscriptionMap { get; } = new Dictionary<string, List<int>>();
#endif
        readonly EventWaitHandle sendEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
        CancellationTokenSource cts = null;
        Task msgProcTask = null;
        MessageManager() { }

        public void Start()
        {
            cts = new CancellationTokenSource();
            msgProcTask = Task.Factory.StartNew(MsgProc);
        }

        public void Stop()
        {
            cts.Cancel();
            sendEvent.Set();
            msgProcTask?.Wait(500);

            while (MessageQ.TryDequeue(out var _)) ;
        }

        public void RegisterID(int id, ISubscriber sub)
        {
            if (!Subscribers.ContainsKey(id))
            {
                Subscribers.TryAdd(id, sub);
            }
            else
            {
                throw new ArgumentException($"key duplicated - {id}");
            }
        }

        public void Subscribe(int sub, string messageId)
        {
            if (!MsgSubscriptionMap.ContainsKey(messageId))
            {
                MsgSubscriptionMap.TryAdd(messageId, new List<int>());
            }

            var sublist = MsgSubscriptionMap[messageId];

            if (!sublist.Contains(sub))
            {
                sublist.Add(sub);
            }
        }

        public void Send(int id, IMessage msg)
        {
            if (cts.IsCancellationRequested == false)
            {
                MessageQ.Enqueue(new InternalMessage
                {
                    Sender = id,
                    Message = msg
                });

                sendEvent.Set();
            }
        }

        void MsgProc()
        {
            while (cts.IsCancellationRequested == false)
            {
                sendEvent.WaitOne();

                while (MessageQ.TryDequeue(out var msg))
                {
                    var sender = msg.Sender;
                    var msgId = msg.Message.Descriptor.FullName;

                    if (!MsgSubscriptionMap.ContainsKey(msgId))
                    {
                        // log?
                        continue;
                    }

                    var subIds = MsgSubscriptionMap[msgId];
#if Parallel_Dispatch
                    Parallel.ForEach(subIds, id =>
                    {
                        if (id == sender)
                            return;
                        if (Subscribers.ContainsKey(id) == false)
                            return;

                        try
                        {
                            Subscribers[id].Dispatcher(msg.Message);
                        }
                        catch
                        {
                            // log?
                        }
                    });
#else
                    foreach (var id in subIds)
                    {
                        if (id != sender && Subscribers.ContainsKey(id))
                        {
                            try
                            {
                                Subscribers[id].Dispatcher(msg.Message);
                            }
                            catch
                            {
                                // log?
                            }
                        }
                    }
#endif
                }
            }
        }
    }
}
