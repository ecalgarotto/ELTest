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
    public class OfficerRepository : IOfficerRepository
    {
        IHttpClientFactory httpClientFactory;
        ICustomAppSettings customAppSettings;

        public OfficerRepository(IHttpClientFactory httpClientFactory, ICustomAppSettings customAppSettings)
        {
            this.httpClientFactory = httpClientFactory;
            this.customAppSettings = customAppSettings;
        }

        public async Task<List<Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null)
        {
            if (string.IsNullOrEmpty(companyNumber))
                throw new ArgumentException("CompanyNumber is mandatory to retrieve Officers");

            using var client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

            var result = await client.GetAsync($"{customAppSettings.ApiURL}/company/{companyNumber}/officers?items_per_page={customAppSettings.NumberOfRecords}");

            if (!result.IsSuccessStatusCode)
            {
                //TODO: log error
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var officersSearch = JsonConvert.DeserializeObject<SearchOfficerResult>(content).items;

            //there's no filter in the API itself, so that's why we filter in code
            if (status.HasValue)
            {
                //TODO: don't know how to filter only active ones, didn't find it in the API docs
                officersSearch = officersSearch.Where(c => c.name != null).ToArray();
            }

            if (age.HasValue)
                officersSearch = officersSearch.Where(c => c.age == age).ToArray();

            return officersSearch.ToList();
        }
    }
}
