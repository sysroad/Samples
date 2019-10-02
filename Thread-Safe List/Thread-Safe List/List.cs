using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Facility.Thread
{
    public class Enumerator : IEnumerator, IDisposable
    {
        readonly IEnumerator iterator;
        readonly object lockObject;
        bool disposed = false;

        public Enumerator(IEnumerator itor, object lockObject)
        {
            iterator = itor;
            this.lockObject = lockObject;

            Monitor.Enter(lockObject);
        }

        ~Enumerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {
                    }
                }
                finally
                {
                    disposed = true;
                    Monitor.Exit(lockObject);
                }
            }
        }

        public object Current { get { return iterator.Current; } }

        public bool MoveNext()
        {
            return iterator.MoveNext();
        }

        public void Reset()
        {
            iterator.Reset();
        }
    }

    public class Enumerator<T> : IEnumerator<T>, IDisposable
    {
        readonly IEnumerator<T> iterator;
        readonly object lockObject;
        bool disposed = false;

        public Enumerator(IEnumerator<T> itor, object lockObject)
        {
            iterator = itor;
            this.lockObject = lockObject;

            Monitor.Enter(lockObject);
        }

        ~Enumerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {
                    }
                }
                finally
                {
                    disposed = true;
                    Monitor.Exit(lockObject);
                }
            }
        }

        public T Current { get { return iterator.Current; } }

        object IEnumerator.Current { get { return iterator.Current; } }

        public bool MoveNext()
        {
            return iterator.MoveNext();
        }

        public void Reset()
        {
            iterator.Reset();
        }
    }

    public class List<T> : IList<T>
    {
        readonly object accessLock = new object();
        readonly System.Collections.Generic.List<T> container = new System.Collections.Generic.List<T>();

        public T this[int index]
        {
            get
            {
                try
                {
                    Monitor.Enter(accessLock);
                    return container[index];
                }
                finally
                {
                    Monitor.Exit(accessLock);
                }
            }
            set
            {
                try
                {
                    Monitor.Enter(accessLock);
                    container[index] = value;
                }
                finally
                {
                    Monitor.Exit(accessLock);
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    Monitor.Enter(accessLock);
                    return container.Count;
                }
                finally
                {
                    Monitor.Exit(accessLock);
                }
            }
        }

        public bool IsReadOnly { get; }

        public void Add(T item)
        {
            try
            {
                Monitor.Enter(accessLock);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public void Clear()
        {
            try
            {
                Monitor.Enter(accessLock);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public bool Contains(T item)
        {
            try
            {
                Monitor.Enter(accessLock);
                return container.Contains(item);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                Monitor.Enter(accessLock);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(container.GetEnumerator(), accessLock);
        }

        public int IndexOf(T item)
        {
            try
            {
                Monitor.Enter(accessLock);
                return container.IndexOf(item);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public void Insert(int index, T item)
        {
            try
            {
                Monitor.Enter(accessLock);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public bool Remove(T item)
        {
            try
            {
                Monitor.Enter(accessLock);
                return container.Remove(item);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        public void RemoveAt(int index)
        {
            try
            {
                Monitor.Enter(accessLock);
            }
            finally
            {
                Monitor.Exit(accessLock);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(container.GetEnumerator(), accessLock);
        }
    }
}
