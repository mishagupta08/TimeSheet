namespace TimeSheet.Controllers
{
    using Properties;
    #region namespace

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using Repository;
    using System.Linq;
    using Timesheet.Common;

    #endregion namespae

    /// <summary>
    /// Hold Chat Forum Controller
    /// </summary>
    public class ChatForumController : Controller
    {
        /// <summary>
        /// holds chat detail model
        /// </summary>
        ChatViewModel chatDetailModel;

        /// <summary>
        /// dashboard Repository for database function
        /// </summary>
        DashboardRepository dashboardRepository;

        // GET: ChatForum
        public async Task<ActionResult> Index(string editId)
        {
            this.chatDetailModel = new ChatViewModel();

            if (!string.IsNullOrEmpty(editId))
            {
                this.dashboardRepository = new DashboardRepository();
                var issueDetailsList = await dashboardRepository.GetChatForumIssueList(editId);
                if (issueDetailsList != null)
                {
                    this.chatDetailModel.IssueDetails = issueDetailsList.First();
                }
            }

            return View(chatDetailModel);
        }

        /// <summary>
        /// method to save chat
        /// </summary>
        /// <param name="chatDetailModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> SaveChatForum(ChatViewModel chatDetailModel)
        {
            try
            {
                if (chatDetailModel == null || chatDetailModel.IssueDetails == null)
                {
                    return Json("Something went wrong.");
                }

                this.dashboardRepository = new DashboardRepository();

                var eId = await dashboardRepository.SaveChatForum(chatDetailModel.IssueDetails);

                if (!string.IsNullOrEmpty(chatDetailModel.IssueDetails.IssueId) && eId == 0)
                {
                    return Json("Issue updated successfully.");
                }
                else if (eId == 0 || eId == -1)
                {
                    return Json(Resources.CommonErorMessage);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return Json("Issue Added Successfully!");
        }

        public async Task<ActionResult> GetChatForumDetailList()
        {
            try
            {
                this.dashboardRepository = new DashboardRepository();
                this.chatDetailModel = new ChatViewModel();
                this.chatDetailModel.IssueDetailsList = await dashboardRepository.GetChatForumIssueList(null);

                foreach (var issue in this.chatDetailModel.IssueDetailsList)
                {
                    issue.IsEditable = TimesheetSession.LoginUser.Name == issue.PostedBy ? true : false;
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return View("chatForumView", this.chatDetailModel);
        }
    }
}