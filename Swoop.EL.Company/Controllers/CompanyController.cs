using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.Common.Cache;

namespace Swoop.EL.Company.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService companyService;
        private readonly ICacheProvider cacheProvider;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService, ICacheProvider cacheProvider)
        {
            _logger = logger;
            this.companyService = companyService;
            this.cacheProvider = cacheProvider;
        }

        [HttpGet("GetCompanyByNumber")]
        public async Task<ActionResult<BAL.DTO.Company>> GetCompanyByNumber(string companyNumber)
        {
            var response = await companyService.GetCompanyByNumber(companyNumber);

            if (response == null)
                return NotFound("Company not found with this search criteria");

            return Ok(response);
        }

        [HttpGet("GetCompaniesByName")]
        [Produces(typeof(List<BAL.DTO.Company>))]
        public async Task<IActionResult> GetCompaniesByName(string companyName)
        {
            var response = await companyService.GetCompaniesByName(companyName);
            if (response == null)
                return NotFound("Companies not found with this search criteria");

            return Ok(response);
        }

        [HttpGet("SearchCompany")]
        public async Task<ActionResult<BAL.DTO.Company>> SearchCompany(string companyName, string officerName)
        {
            var response = await companyService.SearchCompany(companyName, officerName);

            if (response == null)
                return NotFound("Company not found with this search criteria");

            return Ok(response);
        }
    }
}
