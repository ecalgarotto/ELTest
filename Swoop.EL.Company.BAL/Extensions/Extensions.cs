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
                Name = company.company_name,
                Address = new DTO.Address()
                {
                    Region = company.registered_office_address?.region,
                    PostalCode = company.registered_office_address?.postal_code,
                    Locality = company.registered_office_address?.locality,
                    Country = company.registered_office_address?.country,
                    AddressLine1 = company.registered_office_address?.address_line_1,
                    AddressLine2 = company.registered_office_address?.address_line_2
                },
                CompanyAge = company.age,
                RegistrationNumber = company.company_number
            };
        }

        public static DTO.Officer Parse(this DAL.DTO.Officer officer)
        {
            if (officer == null)
                return null;

            return new DTO.Officer()
            {
                Name = officer.name,
                DateOfBirth = new DateTime(officer.date_of_birth.year, officer.date_of_birth.month, 1),
                Role = officer.officer_role
            };
        }
    }
}
