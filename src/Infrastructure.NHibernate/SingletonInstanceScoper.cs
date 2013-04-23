using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.NHibernate
{
    public class SingletonInstanceScoper<TValue>
    {
        private static readonly IDictionary _dictionary = new Dictionary<string, TValue>();

        public IDictionary GetDictionary()
        {
            return _dictionary;
        }

        public TValue GetScopedInstance(string key, Func<TValue> builder)
        {
            if (!GetDictionary().Contains(key))
            {
                BuildInstance(key, builder);
            }
            return (TValue) GetDictionary()[key];
        }

        public void ClearInstance(string key)
        {
            lock (GetDictionary().SyncRoot)
            {
                if (GetDictionary().Contains(key))
                {
                    RemoveInstance(key);
                }
            }
        }

        private void BuildInstance(string key, Func<TValue> builder)
        {
            lock (GetDictionary().SyncRoot)
            {
                if (!GetDictionary().Contains(key))
                {
                    GetDictionary().Add(key, builder.Invoke());
                }
            }
        }

        private void RemoveInstance(string key)
        {
            GetDictionary().Remove(key);
        }
    }
}
