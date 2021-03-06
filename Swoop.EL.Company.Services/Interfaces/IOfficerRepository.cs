using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Interfaces
{
    public interface IOfficerRepository
    {
        Task<List<DTO.Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null);
    }
}
