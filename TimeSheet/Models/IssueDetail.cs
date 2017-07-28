namespace TimeSheet.Models
{
    /// <summary>
    /// Hold issue detail
    /// </summary>
    public class IssueDetail
    {
        /// <summary>
        /// gets or sets Id
        /// </summary>
        public string IssueId { get; set; }

        /// <summary>
        /// gets or sets issue
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// gets or sets posted by
        /// </summary>
        public string PostedBy { get; set; }

        /// <summary>
        /// gets or sets email id
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// gets or sets Posted Date
        /// </summary>
        public string PostedDate { get; set; }

        /// <summary>
        /// gets or sets IsEditable
        /// </summary>
        public bool IsEditable { get; set; }
    }
}