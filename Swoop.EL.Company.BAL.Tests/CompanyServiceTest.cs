using Microsoft.Extensions.Logging;
using Moq;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.BAL.Services;
using Swoop.EL.Company.Common.Cache;
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

            List<DAL.DTO.Company> companies = new List<DAL.DTO.Company>()
            {
                new DAL.DTO.Company(){ Company_name = "COMPANY 1", Company_number = "11111", Date_of_creation = new DateTime(2021, 01, 01)},
                new DAL.DTO.Company(){ Company_name = "COMPANY 2", Company_number = "22222", Date_of_creation = new DateTime(2021, 02, 02)},
                new DAL.DTO.Company(){ Company_name = "ANOTHER COMPANY", Company_number = "33333", Date_of_creation = new DateTime(2021, 03, 03)}
            };

            companyRepository.Setup(c => c.GetCompaniesByName(It.IsAny<string>()))
                .Returns(Task.FromResult(companies));
            companyRepository.Setup(c => c.GetCompaniesByName(null))
                .Throws<ArgumentException>();

            companyRepository.Setup(c => c.GetCompanyByNumber("11111"))
                .Returns(Task.FromResult(companies[0]));
            companyRepository.Setup(c => c.GetCompanyByNumber("22222"))
                .Returns(Task.FromResult(companies[1]));
            companyRepository.Setup(c => c.GetCompanyByNumber(null))
                .Throws<ArgumentException>();

            companyRepository.Setup(c => c.SearchCompany(It.IsAny<string>(), "OFFICER 1"))
                .Returns(Task.FromResult(companies[0]));
            companyRepository.Setup(c => c.SearchCompany(It.IsAny<string>(), "OFFICER 2"))
                .Returns(Task.FromResult(companies[1]));

            companyRepository.Setup(c => c.SearchCompany(It.IsAny<string>(), null))
                .Throws<ArgumentException>();
            companyRepository.Setup(c => c.SearchCompany(null, It.IsAny<string>()))
                .Throws<ArgumentException>();

            companyService = new CompanyService(companyRepository.Object, new EmptyCache(), new Mock<ILogger<CompanyService>>().Object);
        }

        [Fact]
        public async void GetCompaniesByName_OK()
        {
            var response = await companyService.GetCompaniesByName("COMPANY");

            Assert.NotNull(response);
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async void GetCompaniesByName_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => companyService.GetCompaniesByName(null));
        }


        [Theory]
        [InlineData("11111", "COMPANY 1")]
        [InlineData("22222", "COMPANY 2")]
        public async void GetCompanyByNumber_OK(string number, string expectedCompanyName)
        {
            var response = await companyService.GetCompanyByNumber(number);

            Assert.NotNull(response);
            Assert.Equal(expectedCompanyName, response.Name);
        }

        [Fact]
        public async void GetCompanyByNumber_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => companyService.GetCompanyByNumber(null));
        }

        [Theory]
        [InlineData("OFFICER 1", "COMPANY 1")]
        [InlineData("OFFICER 2", "COMPANY 2")]
        public async void SearchCompany_OK(string officerName, string expectedCompanyName)
        {
            var response = await companyService.SearchCompany("any", officerName);

            Assert.NotNull(response);
            Assert.Equal(expectedCompanyName, response.Name);
        }

        [Fact]
        public async void SearchCompany_COMPANY_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => companyService.SearchCompany(null, "321"));
        }

        [Fact]
        public async void SearchCompany_OFFICER_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => companyService.SearchCompany("company", null));
        }


    }
}
