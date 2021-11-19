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
        IHttpClientFactory httpClientFactory;
        ICustomAppSettings customAppSettings;
        IOfficerRepository officerRepository;

        public CompanyRepository(IHttpClientFactory httpClientFactory, ICustomAppSettings customAppSettings, IOfficerRepository officerRepository)
        {
            this.httpClientFactory = httpClientFactory;
            this.customAppSettings = customAppSettings;
            this.officerRepository = officerRepository;
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
                //TODO: log error
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var companiesSearch = JsonConvert.DeserializeObject<SearchCompanyResult>(content).items;

            List<DTO.Company> companies = new List<DTO.Company>();
            //TODO: add AutoMapper or similar
            foreach (var item in companiesSearch)
            {
                companies.Add(new DTO.Company()
                {
                    company_name = item.title,
                    company_number = item.company_number,
                    date_of_creation = item.date_of_creation,
                    registered_office_address = item.address
                });
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
                //TODO: log error
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
                    var officers = await officerRepository.SearchOfficers(company.company_number);

                    if (officers != null && officers.Count > 0)
                        if (officers.Any(o => o.name.Contains(officerName)))
                            return company;
                }
            }
            //else
            //TODO: no company found with the search criteria

            //TODO: log that no company was found with this search criteria
            return null;
        }
    }
}
