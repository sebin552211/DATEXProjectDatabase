using Newtonsoft.Json;
using DATEX_ProjectDatabase.Model;
using System.Net.Http.Headers;


namespace DATEX_ProjectDatabase.Service
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        /*private const string ApiScope = "api://80b64e33-0210-4e19-be18-56028882483d/Read";*/

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<List<ProjectDto>> GetProjectsFromExternalApiAsync()
        {
                 

            // Set the Authorization header with the Bearer token
            

            var response = await _httpClient.GetAsync("https://api-rmtool.experionglobal.dev/api/projects/bugudomainregion/list");  /*https://api-rmtool.experionglobal.dev/api/projectsdetails/list*/

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching data from external API");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            // Deserialize the content into ApiResponse model
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

            // Extract and map the project data to ProjectDto
            var projectDtos = apiResponse.Data.Select(projectData => new ProjectDto
            {
                ProjectCode = projectData.Project.ProjectCode,
                ProjectName = projectData.Project.ProjectName,
                DU = projectData.Project.DUName,
                DUHead = projectData.Project.DeliveryManager,
                ProjectStartDate = projectData.Project.ProjectStartDate ?? DateTime.MinValue,
                ProjectEndDate = projectData.Project.ProjectEndDate ?? DateTime.MinValue,
                ProjectManager = projectData.Project.ProjectManager,
                ContractType = projectData.Project.ContractType,
                NumberOfResources = projectData.ResourceCount,
                CustomerName = projectData.Project.ClientName,
                Region = projectData.Project.Region,    
                Status = projectData.Project.ProjectStatus
            }).ToList();

            return projectDtos;
        }

    }
}
