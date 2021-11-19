using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL;
using Swoop.EL.Company.BAL.Interfaces;

namespace Swoop.EL.Company.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService companyService;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService)
        {
            _logger = logger;
            this.companyService = companyService;
        }

        [HttpGet("GetCompanyByNumber")]
        public Task<BAL.DTO.Company> GetCompanyByNumber(string companyNumber)
        {
            return companyService.GetCompanyByNumber(companyNumber);
        }

        [HttpGet("GetCompaniesByName")]
        public Task<List<BAL.DTO.Company>> GetCompaniesByName(string companyName)
        {
            return companyService.GetCompaniesByName(companyName);
        }

        [HttpGet("SearchCompany")]
        public Task<BAL.DTO.Company> SearchCompany(string companyName, string officerName)
        {
            return companyService.SearchCompany(companyName, officerName);
        }
    }
}
