using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swoop.EL.Company.Common
{
    public class CustomAppSettings: ICustomAppSettings
    {
        public int NumberOfRecords { get; set; }
        public string ApiURL { get; set; }
        public string ApiKey { get; set; }

    }
}
