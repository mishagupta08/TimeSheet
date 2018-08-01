using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.Common;
using TimeSheet.Models;
using TimeSheet.Repository;
using System.Linq;

namespace TimeSheet.Models
{
    /// <summary>
    /// Hold Dashbaord Detail
    /// </summary>
    public class DashboardViewModel
    {

        private const string ACTIVESTATUS = "1";
        /// <summary>
        /// gets or sets team work detail list
        /// </summary>
        public IList<TeamWorkingHour> TeamWorkingHourDetail { get; set; }

        /// <summary>
        /// gets or sets project list
        /// </summary>
        public List<Project> ProjectList { get; set; }

        /// <summary>
        /// gets or sets Filters
        /// </summary>
        public Filters Filters { get; set; }

        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public string SelectedMenu { get; set; }

        /// <summary>
        /// gets or sets login user name
        /// </summary>
        public string LogedinUser { get; set; }

        /// <summary>
        /// gets or sets user Detail
        /// </summary>
        public UserDetail EmployeeDetail { get; set; }

        /// <summary>
        /// gets or sets employee List
        /// </summary>
        public IList<UserDetail> EmployeeList { get; set; }

        /// <summary>
        /// gets or sets employee working hour detail
        /// </summary>
        public IList<TimeDetail> EmployeeWorkingHourList { get; set; }

        /// <summary>
        /// gets or sets roles list
        /// </summary>
        public List<KeyValuePair> Roles { get; set; }

        /// <summary>
        /// gets or sets roles list
        /// </summary>
        public List<KeyValuePair> ReportingEmployeeList { get; set; }

        /// <summary>
        /// gets or sets roles list
        /// </summary>
        public List<KeyValuePair> MaritalStatusList { get; set; }

        /// <summary>
        /// gets or sets Project Detail
        /// </summary>
        public Project ProjectDetails { get; set; }

        /// <summary>
        /// method to set role list
        /// </summary>
        public async Task AssignReportingList()
        {
            this.ReportingEmployeeList = new List<KeyValuePair>();

            LoginApi api = new LoginApi();
            var empList = await api.EmployeeListApi();

            if (empList != null)
            {
                empList = empList.OrderBy(e => e.name).ToList();
                this.ReportingEmployeeList = new List<KeyValuePair>();

                foreach (var emp in empList)
                {                    
                        this.ReportingEmployeeList.Add(new KeyValuePair
                        {
                            Id = emp.id,
                            Value = emp.name
                        });                    
                }
            }

            var defaultEmployee = new KeyValuePair { Id = "0", Value = "-Select Employee-" };
            this.ReportingEmployeeList.Insert(0, defaultEmployee);
        }

        /// <summary>
        /// method to set role list
        /// </summary>
        public async Task AssignRoleList()
        {
            this.Roles = new List<KeyValuePair>();
            if (TimesheetSession.Roles == null || TimesheetSession.Roles.Count > 0)
            {
                var dashboardRepository = new DashboardRepository();
                TimesheetSession.Roles = await dashboardRepository.GetRoleList();
            }

            this.Roles = TimesheetSession.Roles;
        }

        public async Task AssignProjectList()
        {
            this.ProjectList = new List<Project>();

            var dashboardRepository = new DashboardRepository();
            var list = await dashboardRepository.GetProjectList(null, null, null, null);
            if (list != null)
            {
                this.ProjectList = list;
            }

            var defaultProject = new Project { Id = "0", Name = "-Select project-" };
            this.ProjectList.Insert(0, defaultProject);
        }

        /// <summary>
        /// method to set marital status list
        /// </summary>
        public void AssignMaritalList()
        {
            this.MaritalStatusList = new List<KeyValuePair>();
            this.MaritalStatusList.Add(new KeyValuePair { Id = "Single", Value = "Single" });
            this.MaritalStatusList.Add(new KeyValuePair { Id = "Married", Value = "Married" });
        }
    }
}