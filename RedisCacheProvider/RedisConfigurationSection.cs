using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RedisCacheProvider
{
    public class RedisConfigurationSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public long DatabaseID { get; set; }
    }
}
