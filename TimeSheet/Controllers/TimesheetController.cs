using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;
using TimeSheet.Repository;
using TimeSheet.Properties;
using Timesheet.Common;

namespace TimeSheetControllers
{
    public class TimesheetController : Controller
    {
        TimesheetModel timesheetModel;

        public DashboardRepository dashboardRepository;

        // GET: Timesheet
        public async Task<ActionResult> Index(string editId)
        {
            timesheetModel = new TimesheetModel();
            timesheetModel.TimeEntryDetail = new TimeDetail();

            if (TimesheetSession.ProjectListSession == null || TimesheetSession.ProjectListSession.Count() == 0)
            {
                await timesheetModel.AssignProjectList();
                TimesheetSession.ProjectListSession = timesheetModel.ProjectList;
            }
            else
            {
                timesheetModel.ProjectList = TimesheetSession.ProjectListSession;
            }

            if (!string.IsNullOrEmpty(editId))
            {
                this.dashboardRepository = new DashboardRepository();
                var workDetail = await this.dashboardRepository.GetWorkDetailListByFilter(Timesheet.Common.TimesheetSession.LoginUser.EmployeeId, null, null, null, editId);
                if (workDetail != null && workDetail.Count() > 0)
                {
                    this.timesheetModel.TimeEntryDetail = workDetail.First();
                }
            }

            return View(timesheetModel);
        }

        public async Task<ActionResult> SaveTimesheetEntry(TimesheetModel timesheetEntryDetail)
        {
            try
            {
                if (timesheetEntryDetail != null || timesheetEntryDetail.TimeEntryDetail != null)
                {
                    this.dashboardRepository = new DashboardRepository();

                    timesheetEntryDetail.TimeEntryDetail.WorkProjectName = await Task.Run(() => TimesheetSession.ProjectListSession.FirstOrDefault(p => p.Id.Trim() == timesheetEntryDetail.TimeEntryDetail.ProjectName).Name);
                    var result = await dashboardRepository.AddWorkDetail(timesheetEntryDetail.TimeEntryDetail);

                    if (result == 0)
                    {
                        return Json(Resources.CommonErorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(e.Message);
            }

            return Json("Thanks You..!");
        }
    }
}