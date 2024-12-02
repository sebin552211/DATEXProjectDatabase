namespace DATEX_ProjectDatabase.Model
{
    public class ProjectDto
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string DU { get; set; }
        public string DUHead { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ProjectManager { get; set; }
        public string ContractType { get; set; }
        public int? NumberOfResources { get; set; }
        public string CustomerName { get; set; }
        public string Region { get; set; }
      /*  public string Technology { get; set; }*/
        public string Status { get; set; }
    }

}
