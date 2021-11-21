using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL.Extensions;
using System.Linq;
using Swoop.EL.Company.Common.Cache;
using Microsoft.Extensions.Logging;

namespace Swoop.EL.Company.BAL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly ICacheProvider cacheProvider;
        private readonly ILogger<CompanyService> logger;

        public CompanyService(ICompanyRepository companyRepository, ICacheProvider cacheProvider, ILogger<CompanyService> logger)
        {
            this.companyRepository = companyRepository;
            this.cacheProvider = cacheProvider;
            this.logger = logger;
        }

        public async Task<List<DTO.Company>> GetCompaniesByName(string companyName)
        {
            try
            {
                if (cacheProvider.Enabled && cacheProvider.IsInCache($"getcompaniesbyname{companyName}"))
                {
                    return cacheProvider.Get<List<DTO.Company>>($"getcompaniesbyname{companyName}");
                }


                var companies = await companyRepository.GetCompaniesByName(companyName);

                if (companies == null || companies.Count == 0)
                    return null;

                var parsedCompanies = companies.Select(c => c.Parse()).ToList();

                if (cacheProvider.Enabled)
                {
                    cacheProvider.Set<List<DTO.Company>>($"getcompaniesbyname{companyName}", parsedCompanies, TimeSpan.FromHours(1));
                }

                return parsedCompanies;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyService.GetCompaniesByName. Request: company name: {companyName}", companyName);
                throw;
            }
        }

        public async Task<DTO.Company> GetCompanyByNumber(string companyNumber)
        {
            try
            {
                if (cacheProvider.Enabled && cacheProvider.IsInCache($"getcompanybynumber{companyNumber}"))
                {
                    return cacheProvider.Get<DTO.Company>($"getcompanybynumber{companyNumber}");
                }

                var company = await companyRepository.GetCompanyByNumber(companyNumber);

                var parsedCompany = company.Parse();

                if (cacheProvider.Enabled)
                {
                    cacheProvider.Set<DTO.Company>($"getcompanybynumber{companyNumber}", parsedCompany, TimeSpan.FromHours(1));
                }

                return parsedCompany;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyService.GetCompanyByNumber. Request: company number: {companyNumber}", companyNumber);

                throw;
            }

        }

        public async Task<DTO.Company> SearchCompany(string companyName, string officerName)
        {
            try
            {
                if (cacheProvider.Enabled && cacheProvider.IsInCache($"searchcompany{companyName}{officerName}"))
                {
                    return cacheProvider.Get<DTO.Company>($"searchcompany{companyName}{officerName}");
                }


                var company = await companyRepository.SearchCompany(companyName, officerName);

                var parsedCompany = company.Parse();


                if (cacheProvider.Enabled)
                {
                    cacheProvider.Set<DTO.Company>($"searchcompany{companyName}{officerName}", parsedCompany, TimeSpan.FromHours(1));
                }

                return parsedCompany;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CompanyService.GetCompanyByNumber. Request: company name: {companyName}, officer name: {officerName}", companyName);
                throw;
            }
        }


    }
}
