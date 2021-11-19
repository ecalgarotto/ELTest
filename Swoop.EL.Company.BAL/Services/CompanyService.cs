using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL.Extensions;
using System.Linq;

namespace Swoop.EL.Company.BAL.Services
{
    public class CompanyService : ICompanyService
    {
        ICompanyRepository companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<List<DTO.Company>> GetCompaniesByName(string companyName)
        {
            var companies = await companyRepository.GetCompaniesByName(companyName);

            if (companies == null || companies.Count == 0)
                return null;

            return companies.Select(c => c.Parse()).ToList();
        }

        public async Task<DTO.Company> GetCompanyByNumber(string companyNumber)
        {
            var company = await companyRepository.GetCompanyByNumber(companyNumber);

            return company.Parse();
        }

        public async Task<DTO.Company> SearchCompany(string companyName, string officerName)
        {
            var company = await companyRepository.SearchCompany(companyName, officerName);

            return company.Parse();
        }

        
    }
}
