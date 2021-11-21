using Swoop.EL.Company.BAL.DTO;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL.Extensions;
using System.Linq;
using Swoop.EL.Company.Common.Cache;
using Microsoft.Extensions.Logging;

namespace Swoop.EL.Company.BAL.Services
{
    public class OfficerService : IOfficerService
    {
        private readonly IOfficerRepository officerRepository;
        private readonly ICacheProvider cacheProvider;
        private readonly ILogger<OfficerService> logger;

        public OfficerService(IOfficerRepository officerRepository, ICacheProvider cacheProvider, ILogger<OfficerService> logger)
        {
            this.officerRepository = officerRepository;
            this.cacheProvider = cacheProvider;
            this.logger = logger;
        }

        public async Task<List<Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null)
        {
            try
            {
                if (cacheProvider.Enabled && cacheProvider.IsInCache($"searchofficers{companyNumber}{status}{age}"))
                {
                    return cacheProvider.Get<List<Officer>>($"searchofficers{companyNumber}{status}{age}");
                }

                var officers = await officerRepository.SearchOfficers(companyNumber, status, age);

                if (officers == null || officers.Count == 0)
                    return null;

                var parsedOfficers = officers.Select(o => o.Parse()).ToList();

                if (cacheProvider.Enabled)
                {
                    cacheProvider.Set<List<Officer>>($"searchofficers{companyNumber}{status}{age}", parsedOfficers, TimeSpan.FromHours(1));
                }

                return parsedOfficers;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in OfficerService.SearchOfficers. Request: company number: {companyNumber}, status: {status}, age: {age}", companyNumber, status, age);
                throw ex;
            }
        }
    }
}
