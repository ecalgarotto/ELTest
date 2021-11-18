using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.BAL.DTO
{
    public class Officer
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { 
            get
            {
                return (DateTime.Now - DateOfBirth).Days / 365;
            }
        }
        public string Role { get; set; }
    }
}
