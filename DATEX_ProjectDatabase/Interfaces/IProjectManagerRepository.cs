using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectManagerRepository
    {
        Task<ProjectManagers> GetProjectManagerByPMNameAsync(string PMName);
        Task AddProjectManagerAsync(ProjectManagers projectManager);
        Task UpdateProjectManagerAsync(ProjectManagers projectManager);
        Task<Project> AddProjectManagerMail(string PMName, string PMEmail);
        Task<List<Project>> GetProjectManagerAsync(string PMName);
        Task DeleteProjectManagerMail(string PMName);
        Task<IEnumerable<ProjectManagers>> GetAllProjectManagers();
    }
}
