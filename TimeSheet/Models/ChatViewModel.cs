using System.Collections.Generic;

namespace TimeSheet.Models
{
    /// <summary>
    /// Hold chat view model
    /// </summary>
    public class ChatViewModel
    {
        public IssueDetail IssueDetails { get; set; }

        public List<IssueDetail> IssueDetailsList { get; set; }
    }
}