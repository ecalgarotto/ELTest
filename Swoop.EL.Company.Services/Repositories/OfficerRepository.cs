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
    public class OfficerRepository : IOfficerRepository
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ICustomAppSettings customAppSettings;
        private readonly ILogger<OfficerRepository> logger;

        public OfficerRepository(IHttpClientFactory httpClientFactory, ICustomAppSettings customAppSettings, ILogger<OfficerRepository> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.customAppSettings = customAppSettings;
            this.logger = logger;
        }

        public async Task<List<Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null)
        {
            if (string.IsNullOrEmpty(companyNumber))
                throw new ArgumentException("CompanyNumber is mandatory.");

            using var client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(customAppSettings.ApiKey))}");

            var result = await client.GetAsync($"{customAppSettings.ApiURL}/company/{companyNumber}/officers?items_per_page={customAppSettings.NumberOfRecords}");

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError("Request to Companies House API failed. Method: OfficerRepository.SearchOfficers Details: {details}", result.ReasonPhrase);
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var officersSearch = JsonConvert.DeserializeObject<SearchOfficerResult>(content).Items;

            //there's no filter in the API itself, so that's why we filter in code
            if (status.HasValue)
            {
                //TODO: don't know how to filter only active ones, didn't find it in the API docs
                officersSearch = officersSearch.Where(c => c.Name != null).ToArray();
            }

            if (age.HasValue)
                officersSearch = officersSearch.Where(c => c.Age == age).ToArray();

            return officersSearch.ToList();
        }
    }
}
