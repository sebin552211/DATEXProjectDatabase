using DATEX_ProjectDatabase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int projectId);
        Task<Project> GetProjectByCodeAsync(string projectCode);
        Task<List<Project>> GetProjectsByCodesAsync(IEnumerable<string> projectCodes);
        Task UpdateProjectsAsync(IEnumerable<Project> projects);
        Task SaveAsync();
        void Add(Project project);
        void Update(Project project);
        void DeleteProject(int projectId);
        IEnumerable<Project> SearchProjects(string query);
        IEnumerable<Project> GetPagedProjects(int pageNumber, int pageSize);
        int GetTotalProjectsCount();
        void AddProjectEditableFields(Project project);
        void UpdateProjectEditableFields(int id, Project project);
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
    }
}