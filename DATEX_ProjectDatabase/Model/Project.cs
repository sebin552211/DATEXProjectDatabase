using DATEX_ProjectDatabase.Model;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mail;

namespace DATEX_ProjectDatabase.Models
{
    public class Project
    {
        // Read-only fields (received from external API)
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string DU { get; set; }
        public string DUHead { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string ProjectManager { get; set; }
        public string ContractType { get; set; }
        public int? NumberOfResources { get; set; }
        public string CustomerName { get; set; }
        public string Region { get; set; }
        public string Technology { get; set; }
        public string Status { get; set; }

        // Editable fields (managed by admin)
        public string SQA { get; set; }
        public DateTime? ForecastedEndDate { get; set; }
        public DateTime? VOCEligibilityDate { get; set; }
        public int ProjectDurationInDays { get; set; }
        public DateTime? PMInitiateDate { get; set; }
        public DateTime? VOCFeedbackReceivedDate { get; set; }
        public string PMMails { get; set; }
        public string VocRemarks { get; set; }
        public int ProjectDurationInMonths { get; set; }
        public string ProjectType { get; set; }
        public string Domain { get; set; }
        public string DatabaseUsed { get; set; }
        public string CloudUsed { get; set; }
        public string FeedbackStatus { get; set; } // (Received, Pending)
        public string MailStatus { get; set; } // (Initiated, Not Initiated)

        /*public ICollection<ProjectManagers> ProjectManagers { get; set; }*/
        public ProjectManagers ProjectManagerDetails { get; set; }

    }
}
