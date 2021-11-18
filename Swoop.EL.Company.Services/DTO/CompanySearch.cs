using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Swoop.EL.Company.DAL.DTO
{
    public class CompanySearch
    {
        /// <summary>
        /// Company Name
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Company Number or Registration Number
        /// </summary>
        public string company_number { get; set; }

        /// <summary>
        /// Company creation date
        /// </summary>
        public DateTime date_of_creation { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public Address address { get; set; }
    }
}
