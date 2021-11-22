using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swoop.EL.CodeTestApiClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Swoop.EL.CodeTestApiClient
{
    public class CodeTestApiClient : ICodeTestApiClient
    {
        private readonly CodeTestApiClientSettings settings;
        private readonly ILogger<CodeTestApiClient> logger;

        public CodeTestApiClient(CodeTestApiClientSettings settings)
        {
            this.settings = settings;
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
            });
            logger = loggerFactory.CreateLogger<CodeTestApiClient>();
        }

        public async Task<List<Officer>> GetOfficers(string companyNumber, int? age)
        {
            
            using var client = HttpClientFactory.Create();

            var result = await client.GetAsync($"{settings.Endpoint}?companyNumber={companyNumber}&age={age}");

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError("Request to Companies House API failed. Method: OfficerRepository.SearchOfficers Details: {details}", result.ReasonPhrase);
                return null;
            }

            string content = await result.Content.ReadAsStringAsync();
            var officers = JsonConvert.DeserializeObject<List<Officer>>(content);

            return officers;
        }
    }
}
