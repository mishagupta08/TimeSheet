namespace TimeSheet.Models
{
    #region namespace

    using System.Collections.Generic;

    #endregion namespace

    /// <summary>
    /// Hold project detail
    /// </summary>
    public class Project
    {
        /// <summary>
        /// gets or sets Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets startdate
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// gets or sets enddate
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// gets or sets status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// gets or sets Alloted Hour
        /// </summary>
        public decimal AllottedHour { get; set; }

        /// <summary>
        /// gets or sets Remaining Hour
        /// </summary>
        public decimal RemainingHour { get; set; }

        /// <summary>
        /// gets or sets Remaining Hour
        /// </summary>
        public decimal ConsumedHours { get; set; }

        public string MobileNumber { get; set; }
        public string ClientName { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public string DomainName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ClientCoordinatorName { get; set; }
        public string ClientCoordinatorMobileNumber { get; set; }
        public string ClientCoordinatorEmail { get; set; }

        public string ClientDetail { get; set; }

        public string ClientCoordinatorDetail { get; set; }

        /// <summary>
        /// gest or sets status list
        /// </summary>
        public List<KeyValuePair> StatusList { get; set; }

        public void AssignStatusList()
        {
            this.StatusList = new List<KeyValuePair>();

            this.StatusList.Add(new KeyValuePair
            {
                Id = "0",
                Value = "-Select-"
            });

            this.StatusList.Add(new KeyValuePair
            {
                Id = "Upcoming",
                Value = "Upcoming"
            });

            this.StatusList.Add(new KeyValuePair
            {
                Id = "InProgress",
                Value = "InProgress"
            });

            this.StatusList.Add(new KeyValuePair
            {
                Id = "Pause",
                Value = "Pause"
            });

            this.StatusList.Add(new KeyValuePair
            {
                Id = "Complete",
                Value = "Complete"
            });
        }
    }
}