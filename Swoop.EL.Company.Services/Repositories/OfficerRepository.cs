using Newtonsoft.Json;
using Swoop.EL.Company.Common;
using Swoop.EL.Company.DAL.DTO;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Repositories
{
    public class OfficerRepository : IOfficerRepository
    {
        IHttpClientFactory httpClientFactory;
        ICustomAppSettings customAppSettings;

        public OfficerRepository(IHttpClientFactory httpClientFactory, ICustomAppSettings customAppSettings)
        {
            this.httpClientFactory = httpClientFactory;
            this.customAppSettings = customAppSettings;
        }

        public async Task<List<Officer>> SearchOfficers(string companyNumber, int numberOfItems, bool? status = null, int? age = null)
        {
            using var client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

            var result = await client.GetAsync($"{customAppSettings.ApiURL}/company/{companyNumber}/officers&items_per_page={customAppSettings.NumberOfRecords}");

            if (!result.IsSuccessStatusCode)
            {
                //TODO: log error
                throw new Exception("Error retrieving Officers from a Company (OfficersRepository.SearchOfficers)");
            }

            string content = await result.Content.ReadAsStringAsync();
            var companiesSearch = JsonConvert.DeserializeObject<SearchOfficerResult>(content).items;

            List<DTO.Officer> officers = new List<DTO.Officer>();

            return officers;
        }
    }
}
