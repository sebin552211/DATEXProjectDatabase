using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Repository
{
    public class ProjectManagerRepository : IProjectManagerRepository
    {

        private readonly ApplicationDbContext _context;


        public ProjectManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectManagers> GetProjectManagerByPMNameAsync(string PMName)
        {
            return await _context.ProjectManagers
                                 .FirstOrDefaultAsync(pm => pm.Name == PMName);
        }

        public async Task AddProjectManagerAsync(ProjectManagers projectManager)
        {
            await _context.ProjectManagers.AddAsync(projectManager);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectManagerAsync(ProjectManagers projectManager)
        {
            _context.ProjectManagers.Update(projectManager);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProjectManagers>> GetAllProjectManagers()
        {
            return await _context.ProjectManagers.ToListAsync();
        }
        public async Task<List<Project>> GetProjectManagerAsync(string PMName)
        {
            var projectmails = await _context.Projects
                .Where(p => p.ProjectManager == PMName)
                .ToListAsync();

            return projectmails;
        }
        public async Task DeleteProjectManagerMail(string PMName)
        {
            var project = await GetProjectManagerAsync(PMName);
            if (project.Any())
            {
                foreach (var proj in project)
                {
                    proj.PMMails = null; // Assuming Email is the property for the mail
                }

                await _context.SaveChangesAsync();
            }
     /*       if (project != null)
            {
               
               await _context.SaveChangesAsync();
            }
            return project;*/
        }
        public async Task<Project> AddProjectManagerMail(string PMName, string PMEmail)
        {
            // Find all projects where the ProjectManager matches PMName
            var projects = await _context.Projects
                                         .Where(p => p.ProjectManager == PMName)
                                         .ToListAsync();

            // If no matching projects are found, throw an exception
            if (projects == null || !projects.Any())
            {
                throw new KeyNotFoundException($"No projects found for Project Manager {PMName}.");
            }

            // Update PMMails for all matching projects
            foreach (var project in projects)
            {
                project.PMMails = PMEmail;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the first updated project (or any other based on your preference)
            return projects.First();
        }


    }
}
