using Swoop.EL.Company.DAL.DTO;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swoop.EL.Company.DAL.Repositories
{
    public class OfficerRepository : IOfficerRepository
    {
        public async Task<List<Officer>> SearchOfficers(string companyNumber, int numberOfItems, bool? status = null, int? age = null)
        {
            throw new NotImplementedException();
        }
    }
}
