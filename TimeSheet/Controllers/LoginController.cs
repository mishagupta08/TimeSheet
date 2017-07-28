using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;
using Timesheet.Common;
using TimeSheet.Repository;
using System.Web.Security;

namespace TimeSheetControllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// Gets or sets Login Model
        /// </summary>
        LoginModel loginModel;

        /// <summary>
        /// for access dashboard rempository methods
        /// </summary>
        public DashboardRepository dashboardRepository;

        // GET: Login
        public ActionResult Index()
        {
            this.loginModel = new LoginModel();
            return View(this.loginModel);
        }

        /// <summary>
        /// validate user detail
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>validation result</returns>
        public async Task<ActionResult> ValidateUser(LoginModel loginDetail)
        {
            try
            {
                if (loginDetail == null || string.IsNullOrEmpty(loginDetail.username))
                {
                    return Json("Username is empty.");
                }

                if (string.IsNullOrEmpty(loginDetail.password))
                {
                    return Json("Password is empty.");
                }

                this.dashboardRepository = new DashboardRepository();
                //var user = await this.dashboardRepository.ValidateUser(loginDetail);
                LoginApi api = new LoginApi();
                var user = await api.LoginWithApi(loginDetail);
                if (user == null)
                {
                    return Json("Invalid Username and Password.");
                }

                Session["LoginUserRoleId"] = user.RoleId;
                Session["LoginUserId"] = user.EmployeeId;

                TimesheetSession.LoginUser = user;
                Session["LoginUser"] = user.Name;
                FormsAuthentication.SetAuthCookie(TimesheetSession.LoginUser.Username, false);

                /****Delete It*****/

                //TimesheetSession.LoginUser = new UserDetail();
                //TimesheetSession.LoginUser.Designation = "Developer";
                //TimesheetSession.LoginUser.Name = "Bindu Kasera";
                //TimesheetSession.LoginUser.RoleId = "1";

                /****Delete It*****/

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return Json(string.Empty);
        }
    }
}