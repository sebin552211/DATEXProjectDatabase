using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IExternalApiService
    {
        Task<List<ProjectDto>> GetProjectsFromExternalApiAsync();
    }
}
