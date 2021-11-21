using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swoop.EL.Company.Common;
using Swoop.EL.Company.DAL.DTO;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ICustomAppSettings customAppSettings;
        private readonly IOfficerRepository officerRepository;
        private readonly ILogger<CompanyRepository> logger;

        public CompanyRepository(IHttpClientFactory httpClientFactory, ICustomAppSettings customAppSettings, IOfficerRepository officerRepository, ILogger<CompanyRepository> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.customAppSettings = customAppSettings;
            this.officerRepository = officerRepository;
            this.logger = logger;
        }

        public async Task<List<DTO.Company>> GetCompaniesByName(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
                throw new ArgumentException("CompanyName is mandatory.");

            using var client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

            var result = await client.GetAsync($"{customAppSettings.ApiURL}/search/companies?q={companyName}&items_per_page={customAppSettings.NumberOfRecords}");

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError("Request to Companies House API failed. Method: CompanyRepository.GetCompaniesByName Details: {details}", result.ReasonPhrase);
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var companiesSearch = JsonConvert.DeserializeObject<SearchCompanyResult>(content).Items;

            List<DTO.Company> companies = new List<DTO.Company>();
            foreach (var item in companiesSearch)
            {
                companies.Add(
                    new DTO.Company()
                    {
                        Company_name = item.Title,
                        Company_number = item.Company_number,
                        Date_of_creation = item.Date_of_creation,
                        Registered_office_address = item.Address
                    }
                );
            }

            return companies;
        }

        public async Task<DTO.Company> GetCompanyByNumber(string companyNumber)
        {
            if (string.IsNullOrEmpty(companyNumber))
                throw new ArgumentException("CompanyNumber is mandatory.");

            using var client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

            var result = await client.GetAsync($"{customAppSettings.ApiURL}/company/{companyNumber}");

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError("Request to Companies House API failed. Method: CompanyRepository.GetCompanyByNumber Details: {details}", result.ReasonPhrase);
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var company = JsonConvert.DeserializeObject<DTO.Company>(content);

            return company;
        }

        public async Task<DTO.Company> SearchCompany(string companyName, string officerName)
        {
            if (string.IsNullOrEmpty(companyName))
                throw new ArgumentException("CompanyName is mandatory.");

            if (string.IsNullOrEmpty(companyName))
                throw new ArgumentException("OfficerName is mandatory.");


            var companies = await this.GetCompaniesByName(companyName);

            if (companies.Count > 0)
            {
                foreach (var company in companies)
                {
                    var officers = await officerRepository.SearchOfficers(company.Company_number);

                    if (officers != null && officers.Count > 0)
                        if (officers.Any(o => o.Name.Contains(officerName)))
                            return company;
                }
            }
            else
            {
                logger.LogError("No Company found with current search criteria. Method: CompanyRepository.SearchCompany. Parameters: company name: {companyName}, officer name: {officerName}", companyName, officerName);
            }
            return null;
        }
    }
}
