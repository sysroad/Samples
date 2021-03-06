﻿using System;

namespace Lib
{
    public abstract class TSingle<T> where T : class
    {
        public static T Instance { get { return SingleInstance.instance; } }
        class SingleInstance
        {
            static SingleInstance() { }
            internal static readonly T instance = (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
