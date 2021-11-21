using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class Officer
    {
        public string Name { get; set; }
        public DOB Date_of_birth { get; set; }
        public string Officer_role { get; set; }
        public int Age
        {
            get
            {
                return (DateTime.Now - new DateTime(Date_of_birth.Year, Date_of_birth.Month, 1)).Days / 365;
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class DOB
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
