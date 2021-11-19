using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string postal_code { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string region { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string premises { get; set; }
    }
}
