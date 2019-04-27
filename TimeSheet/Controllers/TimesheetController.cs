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

            this.timesheetModel.TimeEntryDetail.Minutes = "0";
            this.timesheetModel.TimeEntryDetail.SubmitDate = DateTime.Now;
            if (!string.IsNullOrEmpty(editId))
            {
                this.dashboardRepository = new DashboardRepository();
                var workDetail = await this.dashboardRepository.GetWorkDetailListByFilter(Session["LoginUserId"].ToString(), null, null, null, editId);
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

                    if (Session["LoginUserId"] == null || string.IsNullOrEmpty(Session["LoginUserId"].ToString()))
                    {
                        return RedirectToAction("Logout", "Dashboard");
                    }
                    else
                    {
                        timesheetEntryDetail.TimeEntryDetail.EmployeeId = Session["LoginUserId"].ToString();
                        timesheetEntryDetail.TimeEntryDetail.EmployeeName = Session["LoginUserName"].ToString();
                    }

                    //if (string.IsNullOrEmpty(timesheetEntryDetail.TimeEntryDetail.Minutes))
                    //{
                    //    timesheetEntryDetail.TimeEntryDetail.Minutes = "0";
                    //}

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

        public async Task<ActionResult> DeleteWorkDetail(string deleteId)
        {
            try
            {
                if (string.IsNullOrEmpty(deleteId))
                {
                    return Json("Work detail Id Is Empty");
                }

                this.dashboardRepository = new DashboardRepository();

                var result = await this.dashboardRepository.DeleteWorkDetail(deleteId);

                return Json(result);

            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}