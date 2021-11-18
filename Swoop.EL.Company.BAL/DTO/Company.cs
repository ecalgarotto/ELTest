using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.BAL.DTO
{
    public class Company
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public int CompanyAge { get; set; }
        public Address Address { get; set; }
    }
}
