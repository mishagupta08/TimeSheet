using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TimeSheet.Models;
using TimeSheet.Repository;

namespace TimeSheet.Controllers
{
    public class TimsheetApiController : ApiController
    {
        DashboardRepository dashboardRepository;

        [HttpPost, Route("api/TimsheetApi/AddTimesheetDetail")]
        public async Task<IHttpActionResult> AddTimesheetDetail()
        {
            var detail = await Request.Content.ReadAsStringAsync();
            var timeDetail = JsonConvert.DeserializeObject<TimeDetail>(detail);
            this.dashboardRepository = new DashboardRepository();
            var result = await this.dashboardRepository.AddTimesheetApi(timeDetail);
            return Content(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/TimsheetApi/GetTimesheetWorkDetail")]
        public async Task<IHttpActionResult> GetTimesheetWorkDetail()
        {
            var detail = await Request.Content.ReadAsStringAsync();
            var timeDetail = JsonConvert.DeserializeObject<Filters>(detail);
            this.dashboardRepository = new DashboardRepository();
            var result = await this.dashboardRepository.GetTimesheetWorkDetail(timeDetail);
            return Content(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet, Route("api/TimsheetApi/DeleteWork/{workId}")]
        public async Task<IHttpActionResult> DeleteWork(string workId)
        {
            this.dashboardRepository = new DashboardRepository();
            var result = await this.dashboardRepository.DeleteWorkDetail(workId);
            return Content(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/TimsheetApi/GetProjectList")]
        public async Task<IHttpActionResult> GetProjectList()
        {
            var detail = await Request.Content.ReadAsStringAsync();
            var timeDetail = JsonConvert.DeserializeObject<Filters>(detail);
            this.dashboardRepository = new DashboardRepository();
            var result = await this.dashboardRepository.GetProjectList(timeDetail);
            return Content(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
    }
}