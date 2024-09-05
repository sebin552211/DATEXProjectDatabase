using Newtonsoft.Json;
using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Service
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /*api link*/
        public async Task<List<ProjectDto>> GetProjectsFromExternalApiAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:4000/projects");

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
