using Moq;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.BAL.Services;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Swoop.EL.Company.BAL.Tests
{
    public class CompanyServiceTest
    {
        ICompanyService companyService;

        public CompanyServiceTest()
        {
            Mock<ICompanyRepository> companyRepository = new Mock<ICompanyRepository>();

            companyRepository.Setup(c => c.GetCompaniesByName(It.IsAny<string>()))
                .Returns(Task.FromResult(default(List<DAL.DTO.Company>)));

            companyRepository.Setup(c => c.GetCompanyByNumber(It.IsAny<string>()))
                .Returns(Task.FromResult(default(DAL.DTO.Company)));

            companyRepository.Setup(c => c.SearchCompany(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(default(DAL.DTO.Company)));

            Mock<CompanyService> mockCompanyService = new Mock<CompanyService>(companyRepository.Object);



            companyService = mockCompanyService.Object;
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
