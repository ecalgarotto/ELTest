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
    public class CompanyRepositoryTests
    {
        Mock<CompanyRepository> mockCompanyRepository;
        Mock<IHttpClientFactory> mockHttpClientFactory;
        ICompanyRepository companyRepository;


        public CompanyRepositoryTests()
        {
            SetupConfiguration();

            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(new DelegatingHandlerStub());            
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            IHttpClientFactory factory = mockHttpClientFactory.Object;
            Mock<IOfficerRepository> mockOfficerRepository = new Mock<IOfficerRepository>();

            List<Officer> officers = new List<Officer>()
            {
                new Officer(){ name= "Everton", date_of_birth = new DOB(){ month = 2, year = 1986 }, officer_role ="Manager" },
                new Officer(){ name= "Karry", date_of_birth = new DOB(){ month = 11, year = 1976 }, officer_role ="Director" }
            };


            mockOfficerRepository.Setup(o => o.SearchOfficers(It.IsAny<string>(), It.IsAny<int>(), null, null))
                .Returns(Task.FromResult(officers));

            mockCompanyRepository = new Mock<CompanyRepository>(factory, new CustomAppSettings()
            {
                ApiURL = "https://api.company-information.service.gov.uk",
                ApiKey = "123",
                NumberOfRecords = 5
            }, mockOfficerRepository.Object);
            companyRepository = mockCompanyRepository.Object;
            
        }

        private void SetupConfiguration()
        {

        }

        [Fact]
        public async void GetCompaniesByName_OK()
        {
            var companies = await companyRepository.GetCompaniesByName("SWOOP FINANCE LIMITED");

            Assert.NotNull(companies);
            Assert.NotEmpty(companies);
            Assert.Equal(1, companies.Count);
        }

        [Fact]
        public async void GetCompaniesByName_NULL()
        {
            var companies = await companyRepository.GetCompaniesByName("");

            Assert.NotNull(companies);
            Assert.Empty(companies);
        }

        [Fact]
        public async void GetCompanyByNumber_OK()
        {
            var company = await companyRepository.GetCompanyByNumber("04362570");

            Assert.NotNull(company);
            Assert.Equal("SWOOP FINANCE LIMITED", company.company_name);
        }

        [Fact]
        public async void GetCompanyByNumber_NULL()
        {
            await Assert.ThrowsAsync<Exception>(() => companyRepository.GetCompanyByNumber("123"));
        }

        [Fact]
        public async void SearchCompany()
        {
            var company = await companyRepository.SearchCompany("SWOOP LIMITED", "BURKE, Ciaran Gerard");

            Assert.NotNull(company);
            Assert.Equal("SWOOP LIMITED", company.company_name);
        }
    }

    public class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub()
        {
            _handlerFunc = (request, cancellationToken) => Task.FromResult(request.CreateResponse(HttpStatusCode.OK));
        }

        public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath == "/company/04362570")
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) => {

                    var response = request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(@"{
    ""has_insolvency_history"": false,
    ""company_number"": ""11163382"",
    ""registered_office_is_in_dispute"": false,
    ""confirmation_statement"": {
                    ""last_made_up_to"": ""2021-03-26"",
        ""overdue"": false,
        ""next_made_up_to"": ""2022-03-26"",
        ""next_due"": ""2022-04-09""
    },
    ""type"": ""ltd"",
    ""undeliverable_registered_office_address"": false,
    ""sic_codes"": [
        ""66190""
    ],
    ""accounts"": {
                    ""last_accounts"": {
                        ""type"": ""total-exemption-full"",
            ""period_start_on"": ""2019-04-01"",
            ""period_end_on"": ""2020-03-31"",
            ""made_up_to"": ""2020-03-31""
                    },
        ""accounting_reference_date"": {
                        ""day"": ""31"",
            ""month"": ""03""
        },
        ""overdue"": false,
        ""next_made_up_to"": ""2021-03-31"",
        ""next_due"": ""2021-12-31"",
        ""next_accounts"": {
                        ""due_on"": ""2021-12-31"",
            ""period_start_on"": ""2020-04-01"",
            ""period_end_on"": ""2021-03-31"",
            ""overdue"": false
        }
                },
    ""company_status"": ""active"",
    ""company_name"": ""SWOOP FINANCE LIMITED"",
    ""has_charges"": false,
    ""date_of_creation"": ""2018-01-22"",
    ""etag"": ""dcc62d38935a087dff23cccf0360a6deff9c626e"",
    ""jurisdiction"": ""england-wales"",
    ""links"": {
                    ""self"": ""/company/11163382"",
        ""persons_with_significant_control"": ""/company/11163382/persons-with-significant-control"",
        ""filing_history"": ""/company/11163382/filing-history"",
        ""officers"": ""/company/11163382/officers""
    },
    ""registered_office_address"": {
                    ""postal_code"": ""MK11 1BN"",
        ""country"": ""England"",
        ""locality"": ""Milton Keynes"",
        ""region"": ""Buckinghamshire"",
        ""address_line_2"": ""Stony Stratford"",
        ""address_line_1"": ""The Stable Yard Vicarage Road""
    },
    ""has_super_secure_pscs"": false,
    ""can_file"": true
}");

                    return Task.FromResult(response);

                };


                return func(request, cancellationToken);
            }
            else if (request.RequestUri.AbsolutePath == "/company/123")
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) => {

                    var response = request.CreateResponse(HttpStatusCode.NotFound);

                    return Task.FromResult(response);

                };


                return func(request, cancellationToken);
            }
            else if (request.RequestUri.AbsolutePath == "/search/companies" && request.RequestUri.Query == "?q=&items_per_page=5")
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) => {

                    var response = request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(@"{
    ""start_index"": 0,
    ""page_number"": 1,
    ""items_per_page"": 5,
    ""items"": [],
    ""kind"": ""search#companies"",
    ""total_results"": 0
}");

                    return Task.FromResult(response);

                };
                return func(request, cancellationToken);
            }
            else if (request.RequestUri.AbsolutePath == "/search/companies" && request.RequestUri.Query == "?q=SWOOP%20FINANCE%20LIMITED&items_per_page=5")
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) => {

                    var response = request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(@"{
    ""total_results"": 44,
    ""items_per_page"": 5,
    ""start_index"": 0,
    ""kind"": ""search#companies"",
    ""items"": [
        {
                    ""date_of_creation"": ""2020-06-30"",
            ""links"": {
                        ""self"": ""/company/12704979""
            },
            ""company_status"": ""active"",
            ""company_type"": ""ltd"",
            ""address_snippet"": ""2 Hilliards Court, Chester Business Park, Chester, Cheshire, United Kingdom, CH4 9PX"",
            ""title"": ""SWOOP FINANCE LIMITED"",
            ""address"": {
                        ""address_line_2"": ""Chester Business Park"",
                ""premises"": ""2"",
                ""country"": ""United Kingdom"",
                ""locality"": ""Chester"",
                ""region"": ""Cheshire"",
                ""postal_code"": ""CH4 9PX"",
                ""address_line_1"": ""Hilliards Court""
            },
            ""kind"": ""searchresults#company"",
            ""snippet"": """",
            ""description_identifier"": [
                ""incorporated-on""
            ],
            ""matches"": {
                        ""snippet"": [],
                ""title"": [
                    1,
                    5
                ]
            },
            ""description"": ""12704979 - Incorporated on 30 June 2020"",
            ""company_number"": ""12704979""
        }
    ],
    ""page_number"": 1
}");

                    return Task.FromResult(response);

                };
                return func(request, cancellationToken);
            }

            else
            {
                Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func = (request, cancellationToken) => {

                    var response = request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(@"{
    ""total_results"": 44,
    ""items_per_page"": 5,
    ""start_index"": 0,
    ""kind"": ""search#companies"",
    ""items"": [
        {
                    ""description_identifier"": [
                        ""incorporated-on""
            ],
            ""snippet"": """",
            ""address"": {
                        ""locality"": ""Darlington"",
                ""country"": ""England"",
                ""premises"": ""55"",
                ""address_line_2"": ""Hurworth"",
                ""address_line_1"": ""The Green The Green"",
                ""postal_code"": ""DL2 2JA""
            },
            ""kind"": ""searchresults#company"",
            ""title"": ""SWOOP LIMITED"",
            ""address_snippet"": ""55 The Green The Green, Hurworth, Darlington, England, DL2 2JA"",
            ""date_of_creation"": ""2002-01-29"",
            ""links"": {
                        ""self"": ""/company/04362570""
            },
            ""company_status"": ""active"",
            ""company_type"": ""ltd"",
            ""company_number"": ""04362570"",
            ""description"": ""04362570 - Incorporated on 29 January 2002"",
            ""matches"": {
                        ""title"": [
                            1,
                    5
                ],
                ""snippet"": []
            }
                },
        {
                    ""matches"": {
                        ""snippet"": [],
                ""title"": [
                    1,
                    5
                        ]
            },
            ""description"": ""12731740 - Incorporated on  9 July 2020"",
            ""company_number"": ""12731740"",
            ""company_status"": ""active"",
            ""company_type"": ""ltd"",
            ""date_of_creation"": ""2020-07-09"",
            ""links"": {
                        ""self"": ""/company/12731740""
            },
            ""address_snippet"": ""71-75 Shelton Street, London, England, WC2H 9JQ"",
            ""title"": ""SWOOP AERO LTD"",
            ""kind"": ""searchresults#company"",
            ""address"": {
                        ""country"": ""England"",
                ""premises"": ""71-75"",
                ""locality"": ""London"",
                ""postal_code"": ""WC2H 9JQ"",
                ""address_line_1"": ""Shelton Street""
            },
            ""description_identifier"": [
                ""incorporated-on""
            ],
            ""snippet"": """"
        },
        {
                    ""snippet"": """",
            ""description_identifier"": [
                ""incorporated-on""
            ],
            ""kind"": ""searchresults#company"",
            ""address"": {
                        ""region"": ""Leicestershire"",
                ""postal_code"": ""LE11 2RY"",
                ""address_line_1"": ""Tiverton Road"",
                ""premises"": ""66"",
                ""country"": ""England"",
                ""locality"": ""Loughborough""
            },
            ""title"": ""SWOOP BOOKS LTD"",
            ""address_snippet"": ""66 Tiverton Road, Loughborough, Leicestershire, England, LE11 2RY"",
            ""company_type"": ""ltd"",
            ""company_status"": ""active"",
            ""links"": {
                        ""self"": ""/company/12830060""
            },
            ""date_of_creation"": ""2020-08-21"",
            ""company_number"": ""12830060"",
            ""description"": ""12830060 - Incorporated on 21 August 2020"",
            ""matches"": {
                        ""title"": [
                            1,
                    5
                ],
                ""snippet"": []
            }
                },
        {
                    ""date_of_creation"": ""2021-10-02"",
            ""links"": {
                        ""self"": ""/company/13657648""
            },
            ""company_status"": ""active"",
            ""company_type"": ""ltd"",
            ""title"": ""SWOOP COACHING LTD"",
            ""address_snippet"": ""Whiteneys Whitelea Lane, Tansley, Matlock, England, DE4 5FL"",
            ""address"": {
                        ""address_line_1"": ""Tansley"",
                ""postal_code"": ""DE4 5FL"",
                ""locality"": ""Matlock"",
                ""premises"": ""Whiteneys Whitelea Lane"",
                ""country"": ""England""
            },
            ""kind"": ""searchresults#company"",
            ""snippet"": """",
            ""description_identifier"": [
                ""incorporated-on""
            ],
            ""matches"": {
                        ""title"": [
                            1,
                    5
                ],
                ""snippet"": []
            },
            ""description"": ""13657648 - Incorporated on  2 October 2021"",
            ""company_number"": ""13657648""
        },
        {
                    ""date_of_creation"": ""2020-06-30"",
            ""links"": {
                        ""self"": ""/company/12704979""
            },
            ""company_status"": ""active"",
            ""company_type"": ""ltd"",
            ""address_snippet"": ""2 Hilliards Court, Chester Business Park, Chester, Cheshire, United Kingdom, CH4 9PX"",
            ""title"": ""EVERTON CREATIVE LIMITED"",
            ""address"": {
                        ""address_line_2"": ""Chester Business Park"",
                ""premises"": ""2"",
                ""country"": ""United Kingdom"",
                ""locality"": ""Chester"",
                ""region"": ""Cheshire"",
                ""postal_code"": ""CH4 9PX"",
                ""address_line_1"": ""Hilliards Court""
            },
            ""kind"": ""searchresults#company"",
            ""snippet"": """",
            ""description_identifier"": [
                ""incorporated-on""
            ],
            ""matches"": {
                        ""snippet"": [],
                ""title"": [
                    1,
                    5
                ]
            },
            ""description"": ""12704979 - Incorporated on 30 June 2020"",
            ""company_number"": ""12704979""
        }
    ],
    ""page_number"": 1
}");

                    return Task.FromResult(response);

                };
                

                return func(request, cancellationToken);
            }            
        }
    }
}
