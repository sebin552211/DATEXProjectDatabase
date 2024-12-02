namespace DATEX_ProjectDatabase.Model
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public List<ProjectData> Data { get; set; }
    }
}
