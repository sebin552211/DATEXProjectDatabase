using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Service
{
    public interface IProjectManagerService
    {
        Task<ProjectManagers> UpsertProjectManagerAsync(ProjectManagers projectManager, int projectId);
    }
}
