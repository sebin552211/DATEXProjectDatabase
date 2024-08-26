using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Service
{
    public interface IExternalApiService
    {
        Task<List<ProjectDto>> GetProjectsFromExternalApiAsync();
    }
}
