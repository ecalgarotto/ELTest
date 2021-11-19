using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.BAL.Interfaces
{
    public interface IOfficerService
    {
        Task<List<DTO.Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null);
    }
}
