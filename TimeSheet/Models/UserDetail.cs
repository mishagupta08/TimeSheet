using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    public class UserDetail
    {
        /// <summary>
        /// gets or sets FirstName
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// gets or sets RoleId
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// gets or sets EmailId
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// gets or sets contact number
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// gets or sets Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// gets or sets MaritalStatus
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// gets or sets AnniversaryDate
        /// </summary>
        public string AnniversaryDate { get; set; }

        /// <summary>
        /// gets or sets Experience
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// gets or sets ReportingTO
        /// </summary>
        public string ReportingTo { get; set; }

        /// <summary>
        /// gets or sets ReportingTO
        /// </summary>
        public string ReportingPersonName { get; set; }

        /// <summary>
        /// gets or sets JoiningDate
        /// </summary>
        public string JoiningDate { get; set; }

        /// <summary>
        /// gets or sets DOB
        /// </summary>
        public string DOB { get; set; }

        /// <summary>
        /// gets or sets Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// gets or sets Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// gets or sets is active
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}