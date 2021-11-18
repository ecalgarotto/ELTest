using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swoop.EL.Company.Common
{
    public interface ICustomAppSettings
    {
        int NumberOfRecords { get; }
        string ApiURL { get; }
        string ApiKey { get; }
    }
}
