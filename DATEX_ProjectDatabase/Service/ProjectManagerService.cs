using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<string> GetPMMailAsync(string PMName)
        {
            var ProjectManager1 = await _projectManagerRepository.GetProjectManagerByPMNameAsync(PMName);
            var ProjectManager2 = await _projectRepository.GetProjectsByPMNameAsync(PMName);

            if (ProjectManager1 != null && ProjectManager2 != null &&
                ProjectManager1.Name == ProjectManager2.ProjectManager)
            {
                ProjectManager2.PMMails = ProjectManager1.Email;
                return ProjectManager2.PMMails;
            }

            return null;
        }
     
        public async Task<ProjectManagers> UpsertProjectManagerAsync(ProjectManagers projectManager, string PMName)
        {
            var project = await _projectRepository.GetProjectsByPMNameAsync(PMName);

            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var existingManager = await _projectManagerRepository.GetProjectManagerByPMNameAsync(PMName);

            if (existingManager == null)
            {
                existingManager = new ProjectManagers
                {
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
    }
}
