using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class Company
    {
        public string Company_name { get; set; }
        public string Company_number { get; set; }
        public DateTime Date_of_creation { get; set; }
        public int Age
        {
            get
            {
                return (DateTime.Now - Date_of_creation).Days / 365;
            }
        }
        public Address Registered_office_address { get; set; }
    }
}
