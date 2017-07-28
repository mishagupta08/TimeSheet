using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    public class Filters
    {
        public string Date { get; set; }

        public string ProjectId { get; set; }

        public string EmployeeId { get; set; }

        public string EndDate { get; set; }

        public string ProjectStatus { get; set; }
    }
}