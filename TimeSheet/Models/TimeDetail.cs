using System;

namespace TimeSheet.Models
{
    /// <summary>
    /// Class to hold timesheet setail
    /// </summary>
    public class TimeDetail
    {
        /// <summary>
        /// gets or sets ProjectName
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// gets or sets WorkProjectName
        /// </summary>
        public string WorkProjectName { get; set; }

        /// <summary>
        /// gets or sets Employee Name
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// gets or sets Employee Name
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// gets or sets hours
        /// </summary>
        public string Hours { get; set; }

        /// <summary>
        /// gets or sets minutes
        /// </summary>
        public string Minutes { get; set; }

        /// <summary>
        /// gets or sets remark
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// gets or sets date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// gets or sets submit date
        /// </summary>
        public Nullable<System.DateTime> SubmitDate { get; set; }

        public DateTime AddedDate { get; set; }
    }
}