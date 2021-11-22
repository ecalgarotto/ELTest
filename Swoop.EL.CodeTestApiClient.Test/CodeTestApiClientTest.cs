using System;
using Xunit;

namespace Swoop.EL.CodeTestApiClient.Test
{
    public class CodeTestApiClientTest
    {
        [Fact]
        public async void TestConsumer()
        {
            var settings = new CodeTestApiClientSettings { Endpoint = "https://localhost:5001/Officer/SearchOfficers" };
            var client = new CodeTestApiClient(settings);
            var officers = await client.GetOfficers("04362570", null);

            Assert.NotNull(officers);
        }
    }
}


