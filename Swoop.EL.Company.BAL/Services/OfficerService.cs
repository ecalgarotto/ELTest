using Swoop.EL.Company.BAL.DTO;
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
    public class OfficerService : IOfficerService
    {
        IOfficerRepository officerRepository;

        public OfficerService(IOfficerRepository officerRepository)
        {
            this.officerRepository = officerRepository;
        }

        public async Task<List<Officer>> SearchOfficers(string companyNumber, bool? status = null, int? age = null)
        {
            try
            {
                var officers = await officerRepository.SearchOfficers(companyNumber, status, age);

                if (officers == null || officers.Count == 0)
                    return null;

                return officers.Select(o => o.Parse()).ToList();
            }
            catch (Exception ex)
            {
                //log error
                throw ex;
            }
        }
    }
}
