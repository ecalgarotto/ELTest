using Swoop.EL.CodeTestApiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swoop.EL.CodeTestApiClient
{
    public interface ICodeTestApiClient
    {
        Task<List<Officer>> GetOfficers(string companyNumber, int? age);
    }
}