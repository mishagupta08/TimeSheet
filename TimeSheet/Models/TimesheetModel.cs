using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TimeSheet.Repository;

namespace TimeSheet.Models
{
    public class TimesheetModel
    {
        public TimeDetail TimeEntryDetail { get; set; }

        public List<Project> ProjectList { get; set; }

        public async Task AssignProjectList()
        {
            this.ProjectList = new List<Project>();

            var dashboardRepository = new DashboardRepository();
            var list = await dashboardRepository.GetProjectList(null, null, null, null);
            if (list != null)
            {
                this.ProjectList = list;
            }
        }
    }
}