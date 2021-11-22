using System;
using System.Collections.Generic;
using System.Text;

namespace Swoop.EL.Company.BAL.Extensions
{
    public static class Extensions
    {
        public static DTO.Company Parse(this DAL.DTO.Company company)
        {
            if (company == null)
                return null;

            return new DTO.Company()
            {
                Name = company.Company_name,
                Address = new DTO.Address()
                {
                    Region = company.Registered_office_address?.Region,
                    PostalCode = company.Registered_office_address?.Postal_code,
                    Locality = company.Registered_office_address?.Locality,
                    Country = company.Registered_office_address?.Country,
                    AddressLine1 = company.Registered_office_address?.Address_line_1,
                    AddressLine2 = company.Registered_office_address?.Address_line_2
                },
                CompanyAge = company.Age,
                RegistrationNumber = company.Company_number
            };
        }

        public static DTO.Officer Parse(this DAL.DTO.Officer officer)
        {
            if (officer == null)
                return null;

            return new DTO.Officer()
            {
                Name = officer.Name,
                DateOfBirth = officer.Date_of_birth != null ? new DTO.DOB() { Year = officer.Date_of_birth.Year, Month = officer.Date_of_birth.Month } : default,
                Role = officer.Officer_role
            };
        }
    }
}
