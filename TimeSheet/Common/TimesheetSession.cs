using System.Collections.Generic;
using TimeSheet.Models;

namespace Timesheet.Common
{
    /// <summary>
    /// Hold timesheet session
    /// </summary>
    public static class TimesheetSession
    {
        ///// <summary>
        ///// gets or sets user detail
        ///// </summary>
        //public static UserDetail LoginUser { get; set; }

        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public static string SelectedMenu { get; set; }

        /// <summary>
        /// gets or sets roles list
        /// </summary>
        public static List<KeyValuePair> Roles { get; set; }

        public static List<Project> ProjectListSession { get; set; }
    }

}