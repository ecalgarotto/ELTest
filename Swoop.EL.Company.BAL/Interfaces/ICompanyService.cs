using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.BAL.Interfaces
{
    public interface ICompanyService
    {
        Task<DTO.Company> GetCompanyByNumber(string companyNumber);

        Task<List<DTO.Company>> GetCompaniesByName(string companyName);

        Task<DTO.Company> SearchCompany(string companyName, string officerName);
    }
}
