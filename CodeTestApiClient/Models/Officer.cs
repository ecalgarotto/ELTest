using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Swoop.EL.CodeTestApiClient.Models
{
    [ExcludeFromCodeCoverage]
    public class Officer
    {
        public string Name { get; set; }
        public DOB DateOfBirth { get; set; }
        public int Age { 
            get
            {
                return DateOfBirth!= null ? (DateTime.Now - new DateTime(DateOfBirth.Year, DateOfBirth.Month, DateTime.Now.Day)).Days / 365 : 0;
            }
        }
        public string Role { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DOB
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
