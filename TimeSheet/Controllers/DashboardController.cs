namespace TimeSheetControllers
{
    #region namsepace

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using Timesheet.Common;
    using TimeSheet.Models;
    using TimeSheet.Repository;
    using TimeSheet.Properties;

    #endregion namsepace

    public class DashboardController : Controller
    {
        public DashboardViewModel dashboardViewModel;

        public DashboardRepository dashboardRepository;

        // GET: Dashboard
        public ActionResult Index()
        {
            TimesheetSession.ProjectListSession = null;
            this.dashboardViewModel = new DashboardViewModel();
            this.dashboardViewModel.LogedinUser = Session["LoginUserName"].ToString();
            return View(this.dashboardViewModel);
        }

        public async Task<ActionResult> GetDashboardMenu(string SelectedMenu, string editId)
        {
            TimesheetSession.ProjectListSession = null;
            try
            {
                if (string.IsNullOrEmpty(SelectedMenu))
                {
                    return null;
                }

                this.dashboardViewModel = new DashboardViewModel();
                if (SelectedMenu.Equals(Resources.AddEmployee))
                {
                    this.dashboardViewModel.AssignMaritalList();
                    await this.dashboardViewModel.AssignRoleList();
                    await this.dashboardViewModel.AssignReportingList();

                    if (!string.IsNullOrEmpty(editId))
                    {
                        this.dashboardRepository = new DashboardRepository();
                        this.dashboardViewModel.EmployeeDetail = await this.dashboardRepository.GetEmployeeById(editId);
                    }

                    return View("_addEmployeeDetailView", this.dashboardViewModel);
                }
                else if (SelectedMenu.Equals(Resources.AddProject))
                {
                    this.dashboardViewModel.ProjectDetails = new Project();

                    if (!string.IsNullOrEmpty(editId))
                    {
                        this.dashboardRepository = new DashboardRepository();
                        var projectList = await this.dashboardRepository.GetProjectList(editId, null, null, null);

                        if (projectList != null)
                        {
                            this.dashboardViewModel.ProjectDetails = projectList.First();
                        }
                    }

                    this.dashboardViewModel.ProjectDetails.AssignStatusList();

                    return View("_addProjectView", this.dashboardViewModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Json("Thanks You..!");
        }

        public async Task<ActionResult> GetFilterProjectList(DashboardViewModel dashboardFilterkModel)
        {
            try
            {
                if (dashboardFilterkModel == null)
                {
                    return Json("Something went wrong.");
                }

                this.dashboardViewModel = new DashboardViewModel();
                this.dashboardViewModel.ProjectList = new List<Project>();
                this.dashboardRepository = new DashboardRepository();

                if (dashboardFilterkModel.Filters == null || (string.IsNullOrEmpty(dashboardFilterkModel.Filters.Date) && string.IsNullOrEmpty(dashboardFilterkModel.Filters.EndDate) && (string.IsNullOrEmpty(dashboardFilterkModel.Filters.ProjectStatus) || dashboardFilterkModel.Filters.ProjectStatus == "0")))
                {
                    this.dashboardViewModel.ProjectList = await this.dashboardRepository.GetProjectList(null, null, null, null);
                }
                else
                {
                    this.dashboardViewModel.ProjectList = await this.dashboardRepository.GetProjectList(null, dashboardFilterkModel.Filters.Date, dashboardFilterkModel.Filters.EndDate, dashboardFilterkModel.Filters.ProjectStatus);
                }

            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return View("_projectListPartialView", this.dashboardViewModel);
        }

        public async Task<ActionResult> GetFilterWorkList(DashboardViewModel dashboardFilterkModel)
        {
            try
            {
                if (dashboardFilterkModel == null)
                {
                    return Json("Something went wrong.");
                }

                this.dashboardRepository = new DashboardRepository();
                this.dashboardViewModel = new DashboardViewModel();
                if (dashboardFilterkModel.Filters == null || (dashboardFilterkModel.Filters.Date == null && (dashboardFilterkModel.Filters.EmployeeId == null || dashboardFilterkModel.Filters.EmployeeId == "0") && (dashboardFilterkModel.Filters.ProjectId == null || dashboardFilterkModel.Filters.ProjectId == "0")))
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = await this.dashboardRepository.GetWorkDetailListByFilter(null, null, null, null, null);
                }
                else
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = await this.dashboardRepository.GetWorkDetailListByFilter(dashboardFilterkModel.Filters.EmployeeId, dashboardFilterkModel.Filters.Date, dashboardFilterkModel.Filters.EndDate, dashboardFilterkModel.Filters.ProjectId, null);
                }

                if (!(this.dashboardViewModel.EmployeeWorkingHourList == null || this.dashboardViewModel.EmployeeWorkingHourList.Count == 0))
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = this.dashboardViewModel.EmployeeWorkingHourList.OrderByDescending(o => o.AddedDate).ToList();
                    var consolidatedChildren = from c in this.dashboardViewModel.EmployeeWorkingHourList
                                               group c by new { c.Date, c.EmployeeId, c.EmployeeName } into gcs
                                               select new TeamWorkingHour() { WorkDate = gcs.Key.Date, EmployeeId = gcs.Key.EmployeeId, EmployeeName = gcs.Key.EmployeeName, TotalHour = gcs.Sum(r => Convert.ToDecimal(r.Hours)), TotalMinutes = gcs.Sum(r => Convert.ToDecimal(r.Minutes)), HourDetail = gcs.ToList() };

                    this.dashboardViewModel.TeamWorkingHourDetail = consolidatedChildren.ToList();
                    for (short i = 0; i < this.dashboardViewModel.TeamWorkingHourDetail.Count; i++)
                    {
                        while (this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes >= 60)
                        {
                            this.dashboardViewModel.TeamWorkingHourDetail[i].TotalHour = this.dashboardViewModel.TeamWorkingHourDetail[i].TotalHour + 1;
                            this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes = this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes - 60;
                        }
                    }

                    //await this.dashboardViewModel.AssignProjectList();
                    // await this.dashboardViewModel.AssignReportingList();
                    return View("_employeeWorkListPartialView", this.dashboardViewModel);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return View("_employeeWorkListPartialView", null);
        }

        public async Task<ActionResult> SaveEmployeeDetail(DashboardViewModel userDetailModel)
        {
            try
            {
                if (userDetailModel == null || userDetailModel.EmployeeDetail == null)
                {
                    return Json("Something went wrong.");
                }

                this.dashboardRepository = new DashboardRepository();
                var eId = await dashboardRepository.AddEmployee(userDetailModel.EmployeeDetail);

                if (!string.IsNullOrEmpty(userDetailModel.EmployeeDetail.EmployeeId) && eId == 0)
                {
                    return Json("User updated successfully.");
                }
                else if (eId == 0)
                {
                    return Json(Resources.CommonErorMessage);
                }
                else if (eId == -1)
                {
                    return Json("User Already Exist.");
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return Json("Employee Added Successfully!");
        }

        public async Task<ActionResult> GetEmployeeList()
        {
            try
            {
                this.dashboardViewModel = new DashboardViewModel();
                this.dashboardViewModel.EmployeeList = new List<UserDetail>();
                this.dashboardRepository = new DashboardRepository();
                this.dashboardViewModel.EmployeeList = await this.dashboardRepository.GetEmployeeList();

                if (this.dashboardViewModel.EmployeeList != null)
                {
                    foreach (var emp in this.dashboardViewModel.EmployeeList)
                    {
                        var name = from p in this.dashboardViewModel.EmployeeList where p.EmployeeId == emp.ReportingTo select p.Name;

                        if (name != null && name.Count() > 0)
                        {
                            emp.ReportingPersonName = name.First();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View("_employeeList", this.dashboardViewModel);
        }

        public async Task<ActionResult> SaveProjectDetail(DashboardViewModel projectDetailModel)
        {
            try
            {
                if (projectDetailModel == null || projectDetailModel.ProjectDetails == null)
                {
                    return Json("Something went wrong.");
                }

                this.dashboardRepository = new DashboardRepository();
                var eId = await dashboardRepository.AddProject(projectDetailModel.ProjectDetails);

                if (eId == 0)
                {
                    return Json(Resources.CommonErorMessage);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return Json("Project Added Successfully!");
        }

        /// <summary>
        /// Method for signout
        /// </summary>
        public ActionResult LogOut()
        {
            Session["LoginUserRoleId"] = null;
            Session["LoginUserId"] = null;
            Session["LoginUserName"] = null;

            TimesheetSession.SelectedMenu = null;

            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Method to get projectList
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetProjectList()
        {
            try
            {
                this.dashboardViewModel = new DashboardViewModel();
                this.dashboardViewModel.ProjectList = new List<Project>();
                this.dashboardRepository = new DashboardRepository();
                this.dashboardViewModel.ProjectList = await this.dashboardRepository.GetProjectList(null, null, null, null);
                this.dashboardViewModel.ProjectDetails = new Project();
                this.dashboardViewModel.ProjectDetails.AssignStatusList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View("_projectList", this.dashboardViewModel);
        }

        public async Task<ActionResult> GetWorkingHourList()
        {
            try
            {
                this.dashboardViewModel = new DashboardViewModel();
                this.dashboardViewModel.Filters = new Filters();

                this.dashboardViewModel.EmployeeWorkingHourList = new List<TimeDetail>();
                this.dashboardRepository = new DashboardRepository();

                if (Session["LoginUserRoleId"].ToString() == "1")
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = await this.dashboardRepository.GetWorkDetailListByFilter(null, null, null, null, null);
                }
                else
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = await this.dashboardRepository.GetWorkDetailListByFilter(Session["LoginUserId"].ToString(), null, null, null, null);
                }

                if (!(this.dashboardViewModel.EmployeeWorkingHourList == null || this.dashboardViewModel.EmployeeWorkingHourList.Count == 0))
                {
                    this.dashboardViewModel.EmployeeWorkingHourList = this.dashboardViewModel.EmployeeWorkingHourList.OrderByDescending(e => e.AddedDate).ToList();
                    var consolidatedChildren = from c in this.dashboardViewModel.EmployeeWorkingHourList
                                               group c by new { c.Date, c.EmployeeId, c.EmployeeName } into gcs
                                               select new TeamWorkingHour() { WorkDate = gcs.Key.Date, EmployeeId = gcs.Key.EmployeeId, EmployeeName = gcs.Key.EmployeeName, TotalHour = gcs.Sum(r => Convert.ToDecimal(r.Hours)), TotalMinutes = gcs.Sum(r => Convert.ToDecimal(r.Minutes)), HourDetail = gcs.ToList() };

                    this.dashboardViewModel.TeamWorkingHourDetail = consolidatedChildren.ToList();

                    for (short i = 0; i < this.dashboardViewModel.TeamWorkingHourDetail.Count; i++)
                    {
                        while (this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes >= 60)
                        {
                            this.dashboardViewModel.TeamWorkingHourDetail[i].TotalHour = this.dashboardViewModel.TeamWorkingHourDetail[i].TotalHour + 1;
                            this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes = this.dashboardViewModel.TeamWorkingHourDetail[i].TotalMinutes - 60;
                        }
                    }

                    await this.dashboardViewModel.AssignProjectList();
                    if (Session["LoginUserRoleId"].ToString() == "1")
                    {
                        await this.dashboardViewModel.AssignReportingList();
                    }
                    else
                    {
                        this.dashboardViewModel.ReportingEmployeeList = new List<KeyValuePair>();
                    }

                    return View("_employeeWorkingHourReport", this.dashboardViewModel);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View("_employeeWorkingHourReport", null);
        }

        public async Task<ActionResult> DeleteProject(string editId)
        {
            try
            {
                if (string.IsNullOrEmpty(editId))
                {
                    return Json("Project Id Is Empty");
                }

                this.dashboardRepository = new DashboardRepository();

                var result = await this.dashboardRepository.DeleteProject(editId);

                return Json(result);

            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}