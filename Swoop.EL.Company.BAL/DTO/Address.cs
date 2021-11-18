using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.BAL.DTO
{
    public class Address
    {
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
