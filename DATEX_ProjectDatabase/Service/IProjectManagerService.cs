using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Service
{
    public interface IProjectManagerService
    {
        Task<ProjectManagers> UpsertProjectManagerAsync(ProjectManagers projectManager, string PMName);
        Task<string> GetPMMailAsync(string PMName);
    }
}
