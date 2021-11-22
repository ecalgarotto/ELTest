using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string Postal_code { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string Address_line_1 { get; set; }
        public string Address_line_2 { get; set; }
        public string Premises { get; set; }
    }
}
