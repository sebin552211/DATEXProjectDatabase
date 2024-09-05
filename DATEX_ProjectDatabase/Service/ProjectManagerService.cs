using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Service
{
    public class ProjectManagerService : IProjectManagerService
    {
        private readonly IProjectManagerRepository _projectManagerRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectManagerService(IProjectManagerRepository projectManagerRepository, IProjectRepository projectRepository)
        {
            _projectManagerRepository = projectManagerRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ProjectManagers> UpsertProjectManagerAsync(ProjectManagers projectManager, int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var existingManager = await _projectManagerRepository.GetProjectManagerByProjectIdAsync(projectId);

            if (existingManager == null)
            {
                existingManager = new ProjectManagers
                {
                    ProjectId = projectId,
                    Name = project.ProjectManager, // Use the name from the project
                    Email = projectManager.Email // Assuming email needs to be provided
                };
                await _projectManagerRepository.AddProjectManagerAsync(existingManager);
            }
            else
            {
                existingManager.Name = project.ProjectManager; // Use the name from the project
                existingManager.Email = projectManager.Email; // Update email if provided
                await _projectManagerRepository.UpdateProjectManagerAsync(existingManager);
            }

            return existingManager;
        }

        // Remove the method that only takes ProjectManagers if you are using the other overload
        // public Task<ProjectManagers> UpsertProjectManagerAsync(ProjectManagers projectManager)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
