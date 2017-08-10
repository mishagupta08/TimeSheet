namespace TimeSheet.Repository
{
    #region namespace

    using Models;
    using Models.DB;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Timesheet.Common;

    #endregion namespace

    public class DashboardRepository
    {
        private DtCareEntities entities = new DtCareEntities();

        public async Task<int> AddEmployee(UserDetail detail)
        {
            /*****Add user*****/
            if (string.IsNullOrEmpty(detail.EmployeeId))
            {
                var validateUser = new LoginModel();
                validateUser.username = detail.Username;
                validateUser.password = detail.Password;

                var existingUser = await this.ValidateUser(validateUser);
                if (existingUser == null)
                {
                    var emp = MapUserDetailToEmployee(detail);
                    entities.Employees.Add(emp);
                    var result = await entities.SaveChangesAsync();
                    return result;
                }
            }
            else
            {
                /****Edit user*****/
                var emp = await Task.Run(() => entities.Employees.FirstOrDefault(e => e.ID == detail.EmployeeId));

                /****Update the values and save*/

                emp.Address = detail.Address.Trim();
                emp.AnniversaryDate = detail.AnniversaryDate.Trim();
                emp.ContactNo = detail.ContactNo.Trim();
                emp.Designation = detail.Designation.Trim();
                emp.DOB = detail.DOB.Trim();
                emp.Experience = detail.Experience.Trim();
                emp.Email = detail.EmailId.Trim();
                //emp.ID = employeeDetail.ID.Trim();
                emp.JoiningDate = detail.JoiningDate.Trim();
                emp.MaritalStatus = detail.MaritalStatus.Trim();
                emp.Name = detail.Name.Trim();
                emp.ReportingTo = detail.ReportingTo.Trim();
                emp.RoleId = detail.RoleId.Trim();
                //emp.username = employeeDetail.username.Trim();
                emp.password = detail.Password.Trim();
                emp.IsActive = detail.IsActive;

                var result = await entities.SaveChangesAsync();
                return result;
                /****Update End*/
            }

            return -1;
        }

        public async Task<int> AddProject(Project detail)
        {
            if (string.IsNullOrEmpty(detail.Id))
            {
                var project = MapProjectToProjectDetail(detail);
                entities.ProjectDetails.Add(project);
                var result = await entities.SaveChangesAsync();
                return result;
            }
            else
            {
                var proj = entities.ProjectDetails.FirstOrDefault(p => p.ProjectKey.Trim() == detail.Id);

                proj.StartDate = detail.StartDate;
                proj.EndDate = detail.EndDate;
                proj.Name = detail.Name;
                proj.Status = detail.Status;

                proj.City = detail.City;
                proj.MobileNumber = detail.MobileNumber;
                proj.ClientName = detail.ClientName;
                proj.Email = detail.Email;
                proj.Address = detail.Address;
                proj.DomainName = detail.DomainName;
                proj.State = detail.State;
                proj.ClientCoordinatorName = detail.ClientCoordinatorName;
                proj.ClientCoordinatorMobileNumber = detail.ClientCoordinatorMobileNumber;
                proj.ClientCoordinatorEmail = detail.ClientCoordinatorEmail;

                var result = await entities.SaveChangesAsync();
                return result;
            }
        }

        public async Task<string> DeleteProject(string projectId)
        {
            var projectList = await Task.Run(() => entities.ProjectDetails.ToList());
            if (projectList == null || projectList.Count == 0)
            {
                return "Project List is Empty";
            }

            var projectDetail = projectList.FirstOrDefault(p => p.ProjectKey.Trim() == projectId);

            if (projectDetail == null)
            {
                return "Project Not Found!";
            }

            entities.ProjectDetails.Remove(projectDetail);
            var response = await entities.SaveChangesAsync();
            if (response > 0)
            {
                return string.Empty;
            }

            return "Something went wrong. Please try again later.";
        }

        public async Task<string> DeleteWorkDetail(string workId)
        {
            var workList = await Task.Run(() => entities.WorkDetails.ToList());
            if (workList == null || workList.Count == 0)
            {
                return "Work Detail List is Empty";
            }

            var workDetail = workList.FirstOrDefault(p => p.WorkDetailId.Trim() == workId);

            if (workDetail == null)
            {
                return "Work Detail Not Found!";
            }

            entities.WorkDetails.Remove(workDetail);
            var response = await entities.SaveChangesAsync();
            if (response > 0)
            {
                return string.Empty;
            }

            return "Something went wrong. Please try again later.";
        }

        public async Task<List<Project>> GetProjectList(string projectId, string startDate, string endDate, string status)
        {
            var projectList = await Task.Run(() => entities.ProjectDetails.ToList());
            if (projectList == null || projectList.Count == 0)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(status))
            {
                status = status.Trim();
            }

            if (!string.IsNullOrEmpty(projectId))
            {
                projectId = projectId.Trim();
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                startDate = startDate.Trim();

                String format = "dd-MM-yyyy";
                DateTime d1 = DateTime.ParseExact(startDate, format, CultureInfo.CurrentCulture);

                projectList = projectList.Where(p => DateTime.ParseExact(p.StartDate.Trim(), format, CultureInfo.CurrentCulture) >= d1).ToList();
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                endDate = endDate.Trim();

                String format = "dd-MM-yyyy";
                DateTime d2 = DateTime.ParseExact(endDate, format, null);
                projectList = projectList.Where(p => DateTime.ParseExact(p.EndDate.Trim(), format, null) <= d2).ToList();
            }

            if (!(string.IsNullOrEmpty(status) || status == "0"))
            {
                projectList = projectList.Where(p => p.Status.Trim() == status).ToList();
            }

            if (!(string.IsNullOrEmpty(projectId) || projectId == "0"))
            {
                projectList = projectList.Where(p => p.ProjectKey.Trim() == projectId).ToList();
            }

            if (projectList == null || projectList.Count == 0)
            {
                return null;
            }

            var projects = new List<Project>();
            foreach (var emp in projectList)
            {
                var p = new Project();
                p = await MapProjectDetailToProject(emp);
                projects.Add(p);
            }

            return projects.OrderBy(p => p.Name).ToList();
        }

        public async Task<UserDetail> ValidateUser(LoginModel model)
        {
            var employeeList = await Task.Run(() => entities.Employees.ToList());
            if (employeeList == null || employeeList.Count == 0)
            {
                return null;
            }

            var emp = employeeList.FirstOrDefault(e => e.username.Trim() == model.username.Trim() && e.password.Trim() == model.password.Trim());
            if (emp == null)
            {
                return null;
            }

            var userDetail = MapEmployeeToUserDetail(emp);
            return userDetail;
        }

        public async Task<int> AddWorkDetail(TimeDetail detail)
        {
            if (detail.Id == null)
            {
                var emp = MapTimeDetailToWork(detail);
                entities.WorkDetails.Add(emp);
                var result = await entities.SaveChangesAsync();
                return result;
            }
            else
            {
                var workDetail = await Task.Run(() => entities.WorkDetails.FirstOrDefault(w => w.WorkDetailId.Trim() == detail.Id));
                if (workDetail != null)
                {
                    if (detail.Date.Contains("("))
                    {
                        string date = detail.Date.Substring(0, detail.Date.IndexOf("(")-1);
                        workDetail.Date = date;
                    }
                    else
                    {
                        workDetail.Date = detail.Date;
                    }
                    workDetail.ProjectName = detail.ProjectName;
                    workDetail.Remarks = detail.Remarks;
                    workDetail.Hours = detail.Hours;
                    workDetail.EmployeeId = detail.EmployeeId;
                    workDetail.EmployeeName = detail.EmployeeName;

                    var result = await entities.SaveChangesAsync();

                    return result;
                }
            }

            return -1;
        }

        public async Task<UserDetail> GetEmployeeById(string id)
        {
            id = id.Trim();
            var employeeList = await Task.Run(() => entities.Employees.ToList());
            if (employeeList == null || employeeList.Count == 0)
            {
                return null;
            }

            var emp = employeeList.FirstOrDefault(e => e.ID.Trim() == id);
            var user = new UserDetail();

            if (emp == null)
            {
                return null;
            }

            user = MapEmployeeToUserDetail(emp);
            return user;
        }

        public async Task<List<UserDetail>> GetEmployeeList()
        {
            var employeeList = await Task.Run(() => entities.Employees.ToList());
            if (employeeList == null || employeeList.Count == 0)
            {
                return null;
            }
            var users = new List<UserDetail>();
            employeeList = employeeList.OrderBy(e => e.Name).ToList();

            foreach (var emp in employeeList)
            {
                var user = new UserDetail();
                user = MapEmployeeToUserDetail(emp);
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// get role list
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair>> GetRoleList()
        {
            var roleList = await Task.Run(() => entities.Roles.ToList());
            var roles = new List<KeyValuePair>();
            foreach (var role in roleList)
            {
                var pair = new KeyValuePair
                {
                    Id = role.RoleId.Trim(),
                    Value = role.Role1.Trim()
                };

                roles.Add(pair);
            }

            return roles;
        }

        public async Task<List<WorkDetail>> GetDefaulterWorkDetail(string employeeId, string date)
        {
            var workList = await Task.Run(() => entities.WorkDetails.ToList());

            if (!string.IsNullOrEmpty(date))
            {
                date = date.Trim();
                workList = workList.Where(detail => detail.Date.Trim() == date).ToList();
            }

            if (!(string.IsNullOrEmpty(employeeId) || employeeId == "0"))
            {
                employeeId = employeeId.Trim();
                workList = workList.Where(detail => detail.EmployeeId.Trim() == employeeId).ToList();
            }

            return workList;
        }

        public async Task<List<TimeDetail>> GetWorkDetailListByFilter(string employeeId, string date, string endDate, string projectId, string workId)
        {
            string format = "dd-MM-yyyy";
            var workList = await Task.Run(() => entities.WorkDetails.ToList());
            workList = workList.OrderByDescending(p => DateTime.ParseExact(p.Date.Trim(), format, CultureInfo.CurrentCulture)).ToList();
            if (!string.IsNullOrEmpty(date))
            {
                //date = date.Trim();
                //workList = workList.Where(detail => detail.Date.Trim() == date).ToList();

                date = date.Trim();

                
                DateTime d1 = DateTime.ParseExact(date, format, CultureInfo.CurrentCulture);

                workList = workList.Where(p => DateTime.ParseExact(p.Date.Trim(), format, CultureInfo.CurrentCulture) >= d1).ToList();
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                endDate = endDate.Trim();
               
                DateTime d2 = DateTime.ParseExact(endDate, format, CultureInfo.CurrentCulture);
                workList = workList.Where(p => DateTime.ParseExact(p.Date.Trim(), format, CultureInfo.CurrentCulture) <= d2).ToList();
            }

            if (!(string.IsNullOrEmpty(employeeId) || employeeId == "0"))
            {
                employeeId = employeeId.Trim();
                workList = workList.Where(detail => detail.EmployeeId.Trim() == employeeId).ToList();
            }

            if (!(string.IsNullOrEmpty(projectId) || projectId == "0"))
            {
                projectId = projectId.Trim();
                workList = workList.Where(detail => detail.ProjectName.Trim() == projectId).ToList();
            }

            if (!string.IsNullOrEmpty(workId))
            {
                workId = workId.Trim();
                workList = workList.Where(detail => detail.WorkDetailId.Trim() == workId).ToList();
            }

            if (workList == null)
            {
                return null;
            }

            var workSummary = new List<TimeDetail>();

            foreach (var work in workList)
            {
                var w = MapWorkDetailToTimeDetail(work);
                workSummary.Add(w);
            }

            return workSummary;
        }

        public async Task<int> SaveChatForum(IssueDetail detail)
        {
            if (string.IsNullOrEmpty(detail.IssueId))
            {
                var issueDetail = MapIssueDetailToIssueDetailTable(detail);
                var workDetail = await Task.Run(() => entities.IssueDetailTables.Add(issueDetail));
                var result = await entities.SaveChangesAsync();
                return result;
            }
            else
            {
                var issueDetails = await Task.Run(() => entities.IssueDetailTables.FirstOrDefault(w => w.IssueId.Trim() == detail.IssueId));
                if (issueDetails != null)
                {
                    issueDetails.Issue = detail.Issue;
                    var result = await entities.SaveChangesAsync();

                    return result;
                }
            }

            return -1;
        }

        /// <summary>
        /// get role list
        /// </summary>
        /// <returns></returns>
        public async Task<List<IssueDetail>> GetChatForumIssueList(string editId)
        {
            if (string.IsNullOrEmpty(editId))
            {
                var issueList = await Task.Run(() => entities.IssueDetailTables.ToList());
                if (issueList != null && issueList.Count() > 0)
                {
                    var issues = new List<IssueDetail>();
                    foreach (var detail in issueList)
                    {
                        var p = new IssueDetail();
                        p = MapIssueDetailTableToIssueDetail(detail);
                        issues.Add(p);
                    }

                    // String format = "dd-MM-yyyy";

                    //   issues = issues.OrderBy(p => DateTime.ParseExact(p.PostedDate.Trim(), format, CultureInfo.CurrentCulture)).ToList();
                    return issues;
                }
            }
            else
            {
                var issue = await Task.Run(() => entities.IssueDetailTables.FirstOrDefault(i => i.IssueId.Trim() == editId));
                if (issue != null)
                {
                    var issues = new List<IssueDetail>();

                    var p = new IssueDetail();
                    p = MapIssueDetailTableToIssueDetail(issue);
                    issues.Add(p);

                    // String format = "dd-MM-yyyy";

                    //   issues = issues.OrderBy(p => DateTime.ParseExact(p.PostedDate.Trim(), format, CultureInfo.CurrentCulture)).ToList();
                    return issues;
                }
            }

            return null;
        }

        /*****mapper classes******/

        public IssueDetailTable MapIssueDetailToIssueDetailTable(IssueDetail detail)
        {
            var issueDetail = new IssueDetailTable();
            issueDetail.IssueId = Guid.NewGuid().ToString().Substring(0, 3);
            issueDetail.Issue = detail.Issue;
            issueDetail.PostedBy = detail.PostedBy;
            issueDetail.PostedDate = DateTime.Now.ToString();
            issueDetail.EmailId = detail.EmailId;

            return issueDetail;
        }

        public IssueDetail MapIssueDetailTableToIssueDetail(IssueDetailTable detail)
        {
            var issueDetail = new IssueDetail();
            issueDetail.IssueId = detail.IssueId;
            issueDetail.Issue = detail.Issue;
            issueDetail.PostedBy = detail.PostedBy;
            issueDetail.PostedDate = DateTime.Now.ToString();
            issueDetail.EmailId = detail.EmailId;

            return issueDetail;

        }

        /// <summary>
        /// method for mapping to project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<Project> MapProjectDetailToProject(ProjectDetail project)
        {
            Project detail = new Project();
            detail.StartDate = project.StartDate.Trim();
            detail.EndDate = project.EndDate.Trim();
            detail.Name = project.Name.Trim();
            detail.Status = project.Status.Trim();
            detail.Id = project.ProjectKey.Trim();
            detail.AllottedHour = project.AllottedHours ?? 0;

            detail.ClientDetail = string.Empty;
            if (!string.IsNullOrEmpty(project.ClientName))
            {
                detail.ClientDetail += project.ClientName + "<br/>";
            }

            if (!string.IsNullOrEmpty(project.MobileNumber))
            {
                detail.ClientDetail += project.MobileNumber + "<br/>";
            }

            if (!string.IsNullOrEmpty(project.Email))
            {
                detail.ClientDetail += project.Email + "<br/>";
            }

            if (!string.IsNullOrEmpty(project.ClientCoordinatorName))
            {
                detail.ClientCoordinatorDetail += project.ClientCoordinatorName + "<br/>";
            }

            if (!string.IsNullOrEmpty(project.MobileNumber))
            {
                detail.ClientCoordinatorDetail += project.MobileNumber + "<br/>";
            }

            if (!string.IsNullOrEmpty(project.ClientCoordinatorEmail))
            {
                detail.ClientCoordinatorDetail += project.ClientCoordinatorEmail + "<br/>";
            }

            detail.City = project.City;
            detail.MobileNumber = project.MobileNumber;
            detail.ClientName = project.ClientName;
            detail.Email = project.Email;
            detail.Address = project.Address;
            detail.DomainName = project.DomainName;
            detail.State = project.State;
            detail.ClientCoordinatorName = project.ClientCoordinatorName;
            detail.ClientCoordinatorMobileNumber = project.ClientCoordinatorMobileNumber;
            detail.ClientCoordinatorEmail = project.ClientCoordinatorEmail;

            /**code to get remaning hours**/

            var list = await Task.Run(() => entities.WorkDetails.Where(w => w.ProjectName == detail.Id));
            if (list != null)
            {
                foreach (var work in list)
                {
                    detail.ConsumedHours += Convert.ToDecimal(work.Hours.Trim());
                }

                detail.RemainingHour = detail.AllottedHour - detail.ConsumedHours;
            }

            /**code to get remaning hours**/

            return detail;
        }

        /// <summary>
        /// method for mapping to project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ProjectDetail MapProjectToProjectDetail(Project project)
        {
            ProjectDetail detail = new ProjectDetail();
            detail.StartDate = project.StartDate;
            detail.EndDate = project.EndDate;
            detail.Name = project.Name;
            detail.Status = project.Status;
            detail.ProjectKey = Guid.NewGuid().ToString().Substring(0, 3);

            detail.City = project.City;
            detail.MobileNumber = project.MobileNumber;
            detail.ClientName = project.ClientName;
            detail.Email = project.Email;
            detail.Address = project.Address;
            detail.DomainName = project.DomainName;
            detail.State = project.State;
            detail.ClientCoordinatorName = project.ClientCoordinatorName;
            detail.ClientCoordinatorMobileNumber = project.ClientCoordinatorMobileNumber;
            detail.ClientCoordinatorEmail = project.ClientCoordinatorEmail;

            String format = "dd-MM-yyyy";
            DateTime dateStart = DateTime.ParseExact(detail.StartDate, format, CultureInfo.CurrentCulture);
            DateTime dateEnd = DateTime.ParseExact(detail.EndDate, format, CultureInfo.CurrentCulture);
            /**Calculate total alloted hours**/
            var perday = Convert.ToInt32(ConfigurationManager.AppSettings["PerDayWorkingHour"]);
            var totalHour = 0;
            while (dateStart <= dateEnd)
            {
                if (!(dateStart.DayOfWeek == DayOfWeek.Sunday))
                {
                    totalHour += perday;
                }

                dateStart = dateStart.AddDays(1);
            }

            detail.AllottedHours = totalHour;
            return detail;
        }

        /// <summary>
        /// Method to map user detail to employee
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        public Employee MapUserDetailToEmployee(UserDetail userDetail)
        {
            var eId = Guid.NewGuid();
            Employee emp = new Employee();
            emp.Address = userDetail.Address.Trim();
            emp.AnniversaryDate = userDetail.AnniversaryDate.Trim();
            emp.ContactNo = userDetail.ContactNo.Trim();
            emp.Designation = userDetail.Designation.Trim();
            emp.DOB = userDetail.DOB.Trim();
            emp.Email = userDetail.EmailId.Trim();
            emp.Experience = userDetail.Experience.Trim();
            emp.ID = eId.ToString().Substring(0, 3).Trim();
            emp.JoiningDate = userDetail.JoiningDate.Trim();
            emp.MaritalStatus = userDetail.MaritalStatus.Trim();
            emp.Name = userDetail.Name.Trim();
            emp.ReportingTo = userDetail.ReportingTo.Trim();
            emp.RoleId = userDetail.RoleId.Trim();
            emp.username = userDetail.Username.Trim();
            emp.password = userDetail.Password.Trim();
            emp.IsActive = userDetail.IsActive;

            return emp;
        }

        /// <summary>
        /// Method to map user detail to employee
        /// </summary>
        /// <param name="employeeDetail"></param>
        /// <returns></returns>
        public UserDetail MapEmployeeToUserDetail(Employee employeeDetail)
        {
            var eId = Guid.NewGuid();
            UserDetail user = new UserDetail();
            user.Address = employeeDetail.Address.Trim();
            user.AnniversaryDate = employeeDetail.AnniversaryDate.Trim();
            user.ContactNo = employeeDetail.ContactNo.Trim();
            user.Designation = employeeDetail.Designation.Trim();
            user.DOB = employeeDetail.DOB.Trim();
            user.Experience = employeeDetail.Experience.Trim();
            user.EmailId = employeeDetail.Email.Trim();
            user.EmployeeId = employeeDetail.ID.Trim();
            user.JoiningDate = employeeDetail.JoiningDate.Trim();
            user.MaritalStatus = employeeDetail.MaritalStatus.Trim();
            user.Name = employeeDetail.Name.Trim();
            user.ReportingTo = employeeDetail.ReportingTo.Trim();
            user.RoleId = employeeDetail.RoleId.Trim();
            user.Username = employeeDetail.username.Trim();
            user.Password = employeeDetail.password.Trim();
            user.IsActive = employeeDetail.IsActive ?? false;
            return user;
        }

        public WorkDetail MapTimeDetailToWork(TimeDetail detail)
        {
            var wId = Guid.NewGuid();
            WorkDetail work = new WorkDetail();
            work.Date = detail.Date;
            work.ProjectName = detail.ProjectName;
            work.Remarks = detail.Remarks;
            work.WorkDetailId = wId.ToString().Substring(0, 3); ;
            work.Hours = detail.Hours;
            work.EmployeeId = detail.EmployeeId;
            work.EmployeeName = detail.EmployeeName;
            work.WorkProjectName = detail.WorkProjectName;
            return work;
        }

        public TimeDetail MapWorkDetailToTimeDetail(WorkDetail detail)
        {
            TimeDetail timeDetail = new TimeDetail();
            timeDetail.Date = detail.Date.Trim();

            try
            {
                String format = "dd-MM-yyyy";
                DateTime d1 = DateTime.ParseExact(timeDetail.Date, format, CultureInfo.CurrentCulture);

                timeDetail.Date += " (" + d1.DayOfWeek.ToString() + ")";
            }
            catch (Exception e)
            { }

            timeDetail.ProjectName = detail.ProjectName.Trim();
            timeDetail.Remarks = detail.Remarks.Trim();
            timeDetail.Id = detail.WorkDetailId.Trim();
            timeDetail.Hours = detail.Hours.Trim();
            timeDetail.EmployeeId = detail.EmployeeId.Trim();
            timeDetail.EmployeeName = detail.EmployeeName.Trim();
            timeDetail.WorkProjectName = detail.WorkProjectName;

            return timeDetail;
        }

        /*****mapper classes******/
    }
}