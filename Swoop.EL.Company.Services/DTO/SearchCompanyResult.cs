using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class SearchCompanyResult
    {
        public int total_results { get; set; }
        public int items_per_page { get; set; }
        public CompanySearch[] items { get; set; }
    }
}
