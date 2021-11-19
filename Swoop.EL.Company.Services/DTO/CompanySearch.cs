using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class CompanySearch
    {
        public string title { get; set; }

        public string company_number { get; set; }

        public DateTime date_of_creation { get; set; }

        public Address address { get; set; }
    }
}
