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
        public int age
        {
            get
            {
                return (DateTime.Now - new DateTime(date_of_birth.year, date_of_birth.month, 1)).Days / 365;
            }
        }
    }

    public class DOB
    {
        public int year { get; set; }
        public int month { get; set; }
    }
}
