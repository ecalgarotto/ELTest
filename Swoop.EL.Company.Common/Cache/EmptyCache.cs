using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.Common.Cache
{
    public class EmptyCache : ICacheProvider
    {
        public EmptyCache()
        {
            Enabled = false;
        }

        public bool Enabled { get; }

        public int ExpiresInHours { get; }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsInCache(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
