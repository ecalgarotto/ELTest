using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Swoop.EL.Company.DAL.DTO.Company> GetCompanyByNumber(string companyNumber);

        Task<List<Swoop.EL.Company.DAL.DTO.Company>> GetCompaniesByName(string companyName);

        Task<Swoop.EL.Company.DAL.DTO.Company> SearchCompany(string companyName, string officerName);
    }
}
