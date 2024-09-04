using DATEX_ProjectDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int projectId);
        Task<Project> GetProjectByCodeAsync(string projectCode);
        Task<IEnumerable<Project>> GetProjectsWithVocEligibilityDateAsync(DateTime date);
        Task<List<Project>> GetProjectsByCodesAsync(IEnumerable<string> projectCodes);
        Task UpdateProjectsAsync(IEnumerable<Project> projects);
        Task UpdateProjectAsync(Project project);
        Task<IEnumerable<Project>> GetFilteredProjectsAsync(
            string du = null,
            string duHead = null,
            DateTime? projectStartDate = null,
            DateTime? projectEndDate = null,
            string projectManager = null,
            string contractType = null,
            string customerName = null,
            string region = null,
            string technology = null,
            string status = null,
            string sqa = null,
            DateTime? vocEligibilityDate = null,
            string projectType = null,
            string domain = null,
            string databaseUsed = null,
            string cloudUsed = null,
            string feedbackStatus = null,
            string mailStatus = null);

        // Synchronous methods (if needed)
        Project GetProjectById(int projectId);
        Project GetProjectByCode(string projectCode);
        void Add(Project project);
        void AddProjectEditableFields(Project project);
        void Update(Project project);
        void UpdateProjectEditableFields(int projectId, Project project);
        void DeleteProject(int projectId);
        IEnumerable<Project> SearchProjects(string query);
        IEnumerable<Project> GetPagedProjects(int pageNumber, int pageSize);
        int GetTotalProjectsCount();

        void Save();
        Task SaveAsync(); // Add this line
    }

}
