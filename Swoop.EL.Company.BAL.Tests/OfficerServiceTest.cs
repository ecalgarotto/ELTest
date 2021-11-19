using Moq;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.BAL.Services;
using Swoop.EL.Company.DAL.DTO;
using Swoop.EL.Company.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Swoop.EL.Company.BAL.Tests
{
    public class OfficerServiceTest
    {
        IOfficerService officerService;

        public OfficerServiceTest()
        {
            Mock<IOfficerRepository> mockOfficerRepository = new Mock<IOfficerRepository>();

            List<Officer> officers = new List<Officer>()
            {
                new Officer(){ name = "OFFICER 1", officer_role = "Director", date_of_birth = new DOB(){ month = 1, year = 2021 } },
                new Officer(){ name = "OFFICER 2", officer_role = "Manager", date_of_birth = new DOB(){ month = 2, year = 2021 } },
                new Officer(){ name = "ANOTHER OFFICER 1", officer_role = "Curator", date_of_birth = new DOB(){ month = 3, year = 2021 } },
            };


            mockOfficerRepository.Setup(o => o.SearchOfficers("11111", null, null))
                .Returns(Task.FromResult(officers.GetRange(0, 1)));

            mockOfficerRepository.Setup(o => o.SearchOfficers("22222", null, null))
                .Returns(Task.FromResult(officers.GetRange(1, 1)));

            mockOfficerRepository.Setup(o => o.SearchOfficers("33333", null, null))
                .Returns(Task.FromResult(officers));

            mockOfficerRepository.Setup(o => o.SearchOfficers(null, null, null))
                .Throws<ArgumentException>();

            officerService = new OfficerService(mockOfficerRepository.Object);
        }

        [Theory]
        [InlineData("11111", 1)]
        [InlineData("22222", 1)]
        [InlineData("33333", 3)]
        public async void SearchOfficers_OK(string companyNumber, int expectedCount)
        {
            var result = await officerService.SearchOfficers(companyNumber);

            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public async void SearchOfficers_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => officerService.SearchOfficers(null));
        }
    }
}
