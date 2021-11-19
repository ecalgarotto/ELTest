using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swoop.EL.Company.BAL.Interfaces;

namespace Swoop.EL.Company.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfficerController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IOfficerService officerService;

        public OfficerController(ILogger<CompanyController> logger, IOfficerService officerService)
        {
            _logger = logger;
            this.officerService = officerService;
        }

        [HttpGet("SearchOfficers")]
        public async Task<ActionResult<List<BAL.DTO.Officer>>> SearchOfficers(string companyNumber, bool? status = null, int? age = null)
        {
            var response = await officerService.SearchOfficers(companyNumber, status, age);

            if (response == null)
                return NotFound("Officers not found with this search criteria.");

            return Ok(response);
        }
    }
}
