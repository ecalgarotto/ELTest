using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.Common.Cache
{
    public interface ICacheProvider
    {
        bool Enabled { get; }

        int ExpiresInHours { get; }

        void Set<T>(string key, T value);

        void Set<T>(string key, T value, TimeSpan timeout);

        T Get<T>(string key);

        bool Remove(string key);

        bool IsInCache(string key);
    }
}
