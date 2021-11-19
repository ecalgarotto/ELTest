using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class Company
    {
        public string company_name { get; set; }
        public string company_number { get; set; }
        public DateTime date_of_creation { get; set; }
        public int age
        {
            get
            {
                return (DateTime.Now - date_of_creation).Days / 365;
            }
        }
        public Address registered_office_address { get; set; }
    }
}
