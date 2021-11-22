using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Swoop.EL.Company.DAL.DTO
{
    [ExcludeFromCodeCoverage]
    public class SearchOfficerResult
    {
        public int Total_results { get; set; }
        public int Items_per_page { get; set; }
        public int Active_count { get; set; }
        public int Inactive_count { get; set; }
        public Officer[] Items { get; set; }
    }
}
