using DATEX_ProjectDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectRepository
    {
   Task<IEnumerable<Project>> GetAllProjectsAsync();
        Project GetProjectById(int projectId);

        // Admin operations on editable fields
        void AddProjectEditableFields(Project project);
        void UpdateProjectEditableFields(int projectId, Project project);
        void DeleteProject(int projectId);
        Project GetProjectByCode(string projectCode);
        public IEnumerable<Project> SearchProjects(string query);
        IEnumerable<Project> GetPagedProjects(int pageNumber, int pageSize);
        int GetTotalProjectsCount();

        Task<Project> GetProjectByIdAsync(int projectId);

        Task<IEnumerable<Project>> GetProjectsWithVocEligibilityDateAsync(DateTime date);

        Task<List<Project>> GetProjectsByCodesAsync(IEnumerable<string> projectCodes);
        Task UpdateProjectsAsync(IEnumerable<Project> projects);

        void Add(Project project);
        void Update(Project project);

        void Save();
       

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

        // Other existing methods...
    }
}
