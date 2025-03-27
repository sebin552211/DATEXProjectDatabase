using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;

namespace DATEX_ProjectDatabase.Service
{
    public interface IProjectManagerService
    {
        Task<Project> UpsertProjectManagerAsync(Project projectManager, string PMName);
        Task<string> GetPMMailAsync(string PMName);
    }
}
