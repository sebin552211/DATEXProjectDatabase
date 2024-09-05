namespace DATEX_ProjectDatabase.Model
{
    public class VOCAnalysis
    {
        public int Id { get; set; }  // Primary key
        public string CustomerFocus { get; set; }
        public string PlanningAndControl { get; set; }
        public string Quality { get; set; }
        public string Communication { get; set; }
        public string Knowledge { get; set; }
        public string EngageService { get; set; }
        public int Score { get; set; }

    }
}
