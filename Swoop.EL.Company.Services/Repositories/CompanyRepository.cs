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
            using (var client = httpClientFactory.CreateClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

                var result = await client.GetAsync($"{customAppSettings.ApiURL}/search/companies?q={companyName}&items_per_page={customAppSettings.NumberOfRecords}");

                if (!result.IsSuccessStatusCode)
                {
                    //TODO: log error
                    throw new Exception("Error retrieving Companies by Name (CompanyRepository.GetCompanyByName)");
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
        }

        public async Task<DTO.Company> GetCompanyByNumber(string companyNumber)
        {
            using (var client = httpClientFactory.CreateClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

                var result = await client.GetAsync($"{customAppSettings.ApiURL}/company/{companyNumber}");

                if (!result.IsSuccessStatusCode)
                {
                    //TODO: log error
                    throw new Exception("Error retrieving Company by Number (CompanyRepository.GetCompanyByNumber)");
                }

                string content = await result.Content.ReadAsStringAsync();
                var company = JsonConvert.DeserializeObject<DTO.Company>(content);

                return company;
            }
        }

        public async Task<DTO.Company> SearchCompany(string companyName, string officerName)
        {
            var companies = await this.GetCompaniesByName(companyName);

            if (companies.Count > 0)
            {
                foreach (var company in companies)
                {
                    var officers = await officerRepository.SearchOfficers(company.company_number, customAppSettings.NumberOfRecords);
                    if (officers != null && officers.Count > 0)
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
