namespace DATEX_ProjectDatabase.Model
{
    public class ProjectExternal
    {

        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string DUName { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string ProjectManager { get; set; }
        public string DeliveryManager { get; set; }
        public string ContractType { get; set; }
        public string ClientName { get; set; }
        public string Region { get; set; }
        public string Currency { get; set; }
        public string ProjectDomain { get; set; }
        public string TechStack { get; set; }
        public string ProjectStatus { get; set; }
    }
}
