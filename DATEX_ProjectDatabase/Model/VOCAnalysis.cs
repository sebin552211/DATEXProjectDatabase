using DATEX_ProjectDatabase.Models;

namespace DATEX_ProjectDatabase.Model
{
    public class VOCAnalysis
    {
        public int Id { get; set; }  // Primary key
        public string ResponseId { get; set; }
        public string SurveyId { get; set; }
        public List<string> CustomerFocus { get; set; }
        public List<string> PlanningAndControl { get; set; }
        public string Quality { get; set; }
        public List<string> Communication { get; set; }
        public string Knowledge { get; set; }
        public string EngageService { get; set; }
        public int Score { get; set; }
        public DateTime? Response_Completion_Time { get; set; } 
        public string DU { get; set; }
        //public DateTime PMInitiateDate { get; set; }
    }

    public static class FileStorage
    {
        public static string UploadedFilePath { get; set; }
    }
}
