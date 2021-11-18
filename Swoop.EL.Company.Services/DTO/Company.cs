﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    public class Company
    {
        public string company_name { get; set; }
        public string company_number { get; set; }
        public DateTime date_of_creation { get; set; }
        public Address registered_office_address { get; set; }
    }
}