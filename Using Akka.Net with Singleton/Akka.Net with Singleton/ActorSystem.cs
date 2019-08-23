using System;

namespace AkkaDotNet.Singleton
{
    public interface ICreate<T>
    {
        T Create();
    }

    public class ActorSystemCreator : ICreate<Akka.Actor.ActorSystem>
    {
        public Akka.Actor.ActorSystem Create()
        {
            return Akka.Actor.ActorSystem.Create(Guid.NewGuid().ToString());
        }
    }

    public abstract class TSingle<T, Creator>
        where T : class
        where Creator : ICreate<T>, new()
    {
        public static T Instance { get { return SingleInstance.instance; } }

        class SingleInstance
        {
            static SingleInstance() { }
            internal static readonly T instance = new Creator().Create();
        }
    }

    public class ActorSystem : TSingle<Akka.Actor.ActorSystem, ActorSystemCreator>
    {
        ActorSystem() { }
    }
}
