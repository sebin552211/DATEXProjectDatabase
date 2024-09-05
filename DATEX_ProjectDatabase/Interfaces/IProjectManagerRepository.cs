using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectManagerRepository
    {
        Task<ProjectManagers> GetProjectManagerByProjectIdAsync(int projectId);
        Task AddProjectManagerAsync(ProjectManagers projectManager);
        Task UpdateProjectManagerAsync(ProjectManagers projectManager);
    }
}
