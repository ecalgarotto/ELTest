using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Swoop.EL.Company.DAL.DTO
{
    public class SearchOfficerResult
    {
        public int total_results { get; set; }
        public int items_per_page { get; set; }
        public int active_count { get; set; }
        public int inactive_count { get; set; }
        public Officer[] items { get; set; }
    }
}
