using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using Swoop.EL.Company.Common.Cache;
using System;

namespace RedisCacheProvider
{
    public class RedisCacheProvider : ICacheProvider
    {
        readonly RedisEndpoint endPoint;

        public bool Enabled { get; }

        public int ExpiresInHours { get; }
        public RedisCacheProvider(IConfiguration configuration)
        {
            var config = new RedisConfigurationManager(configuration);
            endPoint = new RedisEndpoint(config.Config.Host, config.Config.Port, config.Config.Password, config.Config.DatabaseID);
            Enabled = true;
        }

        public void Set<T>(string key, T value)
        {
            this.Set(key, value, TimeSpan.Zero);
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            using (RedisClient client = new RedisClient(endPoint))
            {
                client.As<T>().SetValue(key, value, timeout);
            }
        }

        public T Get<T>(string key)
        {
            T result = default(T);

            using (RedisClient client = new RedisClient(endPoint))
            {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }

        public bool Remove(string key)
        {
            bool removed = false;

            using (RedisClient client = new RedisClient(endPoint))
            {
                removed = client.Remove(key);
            }

            return removed;
        }

        public bool IsInCache(string key)
        {
            bool isInCache = false;

            using (RedisClient client = new RedisClient(endPoint))
            {
                isInCache = client.ContainsKey(key);
            }

            return isInCache;
        }
    }
}
