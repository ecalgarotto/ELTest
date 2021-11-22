using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RedisCacheProvider
{
    public class RedisConfigurationManager
    {
        #region Constants

        public RedisConfigurationManager(IConfiguration configuration)
        {
            Config = new RedisConfigurationSection()
            {
                Host = configuration["RedisConfiguration:host"],
                Port = int.Parse(configuration["RedisConfiguration:port"]),
                Password = configuration["RedisConfiguration:password"],
                DatabaseID = long.Parse(configuration["RedisConfiguration:databaseId"])
            };
        }

        public RedisConfigurationSection Config { get; }

        #endregion
    }
}
