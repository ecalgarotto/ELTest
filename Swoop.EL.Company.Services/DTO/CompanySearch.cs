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
        public string Title { get; set; }
        public string Company_number { get; set; }
        public DateTime Date_of_creation { get; set; }
        public Address Address { get; set; }
    }
}
