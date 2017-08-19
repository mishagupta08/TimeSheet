namespace TimeSheet.Models
{
    #region namespace

    using System.Collections.Generic;

    #endregion namespace

    /// <summary>
    /// Hold team working hour detail
    /// </summary>
    public class TeamWorkingHour
    {
        /// <summary>
        /// gets or sets Filters
        /// </summary>
        public Filters Filters { get; set; }

        /// <summary>
        /// gets or sets work date
        /// </summary>
        public string WorkDate { get; set; }

        /// <summary>
        /// gets or sets employee id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// gets or sets employee name
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// gets or sets hour detail list
        /// </summary>
        public IEnumerable<TimeDetail> HourDetail { get; set; }

        /// <summary>
        /// gets or sets total hours
        /// </summary>
        public decimal TotalHour { get; set; }

        /// <summary>
        /// gets or sets total minutes
        /// </summary>
        public decimal TotalMinutes { get; set; }
    }
}