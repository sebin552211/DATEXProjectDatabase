using Newtonsoft.Json;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Repository
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /*External Api link*/
        public async Task<List<ProjectDto>> GetProjectsFromExternalApiAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:3000/projects");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching data from external API");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            return JsonConvert.DeserializeObject<List<ProjectDto>>(content);
        }
    }
}
