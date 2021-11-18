using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    public class Officer
    {
        public string name { get; set; }
        public DOB date_of_birth { get; set; }
        public string officer_role { get; set; }
    }

    public class DOB
    {
        public int year { get; set; }
        public int month { get; set; }
    }
}
