using DATEX_ProjectDatabase.Models;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProjects();
        Project GetProjectById(int projectId);

        // Admin operations on editable fields
        void AddProjectEditableFields(Project project);
        void UpdateProjectEditableFields(int projectId, Project project);
        void DeleteProject(int projectId);
        Project GetProjectByCode(string projectCode);

        void Add(Project project);
        void Update(Project project);

        void Save();
    }
}
