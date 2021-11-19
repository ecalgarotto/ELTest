using Moq;
using Swoop.EL.Company.Common;
using Swoop.EL.Company.DAL.DTO;
using Swoop.EL.Company.DAL.Interfaces;
using Swoop.EL.Company.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Swoop.EL.Company.DAL.Tests
{
    public class OfficerRepositoryTests
    {
        Mock<OfficerRepository> mockOfficerRepository;
        Mock<IHttpClientFactory> mockHttpClientFactory;
        IOfficerRepository officerRepository;


        public OfficerRepositoryTests()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(new OfficerDelegatingHandlerStub());
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            IHttpClientFactory factory = mockHttpClientFactory.Object;

            mockOfficerRepository = new Mock<OfficerRepository>(factory, new CustomAppSettings()
            {
                ApiURL = "https://api.company-information.service.gov.uk",
                ApiKey = "123",
                NumberOfRecords = 5
            });

            officerRepository = mockOfficerRepository.Object;

        }

        [Fact]
        public async void SearchOfficers_OK()
        {
            var officers = await officerRepository.SearchOfficers("04362570");

            Assert.NotNull(officers);
            Assert.NotEmpty(officers);
            Assert.Equal(5, officers.Count);
        }

        [Fact]
        public async void SearchOfficers_NULL()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => officerRepository.SearchOfficers(null));
        }

        [Fact]
        public async void SearchOfficers_InvalidCompany()
        {
            var response = await officerRepository.SearchOfficers("123");

            Assert.Null(response);
        }
    }

    public class OfficerDelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public OfficerDelegatingHandlerStub()
        {
            _handlerFunc = (request, cancellationToken) => Task.FromResult(request.CreateResponse(HttpStatusCode.OK));
        }

        public OfficerDelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath == "/company/04362570/officers")
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) =>
                {

                    var response = request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(@"{
    ""links"": {
        ""self"": ""/company/11163384/officers""
    },
    ""total_results"": 5,
    ""items_per_page"": 5,
    ""items"": [
        {
                ""appointed_on"": ""2019-07-05"",
            ""officer_role"": ""director"",
            ""occupation"": ""Director"",
            ""former_names"": [
                {
                    ""forenames"": ""KEELY-JANINE"",
                    ""surname"": ""WARREN""
                }
            ],
            ""address"": {
                    ""premises"": ""Unit 81 Centaur Court"",
                ""locality"": ""Ipswich"",
                ""postal_code"": ""IP6 0NL"",
                ""address_line_2"": ""Gt. Blakenham"",
                ""region"": ""Suffolk"",
                ""address_line_1"": ""Claydon Business Park"",
                ""country"": ""United Kingdom""
            },
            ""nationality"": ""British"",
            ""date_of_birth"": {
                    ""year"": 1976,
                ""month"": 12
            },
            ""country_of_residence"": ""England"",
            ""name"": ""WARREN, Keely-Janine"",
            ""links"": {
                    ""officer"": {
                        ""appointments"": ""/officers/AHZX-9Lyvz1mptPB7MMRWkRrm-U/appointments""
                    },
                ""self"": ""/company/11163384/appointments/cwhhF35VBBoteKBxcMhPwTX1-GQ""
            }
            },
        {
                ""name"": ""HODGSON, Alison"",
            ""links"": {
                    ""self"": ""/company/11163384/appointments/03zKntw0-daxu76fykg1BXWobyM"",
                ""officer"": {
                        ""appointments"": ""/officers/genorkNTRGtTe_8aN-5XpV-HaEU/appointments""
                }
                },
            ""appointed_on"": ""2018-01-22"",
            ""resigned_on"": ""2019-03-01"",
            ""officer_role"": ""secretary"",
            ""address"": {
                    ""premises"": ""249"",
                ""address_line_1"": ""St Peters Street"",
                ""postal_code"": ""NR32 2LU"",
                ""locality"": ""Lowestoft"",
                ""country"": ""United Kingdom""
            }
            },
        {
                ""name"": ""PHILLIPS, Lee Richard James"",
            ""links"": {
                    ""officer"": {
                        ""appointments"": ""/officers/ZO4bm8M-ZG993ehoo-zKPXqz1b0/appointments""
                    },
                ""self"": ""/company/11163384/appointments/pfUP0DjSOPu4Le8C5KAws1hmr3Y""
            },
            ""appointed_on"": ""2019-03-01"",
            ""address"": {
                    ""postal_code"": ""IP22 2DJ"",
                ""address_line_2"": ""Shelfanger"",
                ""country"": ""England"",
                ""locality"": ""Diss"",
                ""premises"": ""New Horizons"",
                ""address_line_1"": ""Heywood Road""
            },
            ""officer_role"": ""secretary"",
            ""resigned_on"": ""2019-07-05""
        },
        {
                ""officer_role"": ""director"",
            ""resigned_on"": ""2019-03-01"",
            ""appointed_on"": ""2018-01-22"",
            ""occupation"": ""Homemaker"",
            ""date_of_birth"": {
                    ""month"": 9,
                ""year"": 1978
            },
            ""nationality"": ""British"",
            ""address"": {
                    ""locality"": ""Lowestoft"",
                ""country"": ""United Kingdom"",
                ""address_line_1"": ""St Peters Street"",
                ""postal_code"": ""NR32 2LU"",
                ""premises"": ""249""
            },
            ""name"": ""HODGSON, Alison"",
            ""links"": {
                    ""officer"": {
                        ""appointments"": ""/officers/M5F2pk3AlQD3qnaNcgsrsGHzXZk/appointments""
                    },
                ""self"": ""/company/11163384/appointments/OIlK7rpXinJPhrJTUePP-m_3ptA""
            },
            ""country_of_residence"": ""United Kingdom""
        },
        {
                ""officer_role"": ""director"",
            ""resigned_on"": ""2019-07-05"",
            ""appointed_on"": ""2019-03-01"",
            ""occupation"": ""Managing Director"",
            ""date_of_birth"": {
                    ""month"": 1,
                ""year"": 1974
            },
            ""address"": {
                    ""address_line_1"": ""Heywood Road"",
                ""locality"": ""Diss"",
                ""postal_code"": ""IP22 2DJ"",
                ""premises"": ""New Horizons"",
                ""address_line_2"": ""Shelfanger"",
                ""country"": ""England""
            },
            ""nationality"": ""British"",
            ""country_of_residence"": ""United Kingdom"",
            ""name"": ""PHILLIPS, Lee Richard James"",
            ""links"": {
                    ""officer"": {
                        ""appointments"": ""/officers/K2Y-hmLafm2iafjm9qamSpXqOnc/appointments""
                    },
                ""self"": ""/company/11163384/appointments/fXXaxpOTlc0GL4I8KnExO_QnCrw""
            }
            }
    ],
    ""active_count"": 0,
    ""kind"": ""officer-list"",
    ""inactive_count"": 1,
    ""resigned_count"": 4,
    ""etag"": ""0aa04cabf427400aa4a78d2f9725872cd214b7f3"",
    ""start_index"": 0
}");

                    return Task.FromResult(response);

                };


                return func(request, cancellationToken);
            }
            else
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) =>
                {

                    var response = request.CreateResponse(HttpStatusCode.NotFound);

                    return Task.FromResult(response);

                };

                return func(request, cancellationToken);
            }
        }
    }
}
