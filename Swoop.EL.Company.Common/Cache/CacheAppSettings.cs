using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swoop.EL.Company.Common
{
    public class CacheAppSettings : ICacheAppSettings
    {
        public string CacheProvider { get; set; }
        public string Provider { get; set; }
    }
}
