using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Interfaces
{
    public interface IOfficerRepository
    {
        Task<List<Swoop.EL.Company.DAL.DTO.Officer>> SearchOfficers(string companyNumber, int numberOfItems, bool? status = null, int? age = null);
    }
}
