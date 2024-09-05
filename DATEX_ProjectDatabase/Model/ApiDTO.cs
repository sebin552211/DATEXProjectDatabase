namespace DATEX_ProjectDatabase.Model
{
    public class ApiResponseDto
    {
        public List<ApiProjectData> Data { get; set; }
    }

    public class ApiProjectData
    {
        public ApiProject Project { get; set; }
        public int ResourceCount { get; set; }
        public string Skills { get; set; }
    }

    public class ApiProject
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string DuName { get; set; }
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
