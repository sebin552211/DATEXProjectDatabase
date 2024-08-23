using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsByCodesAsync(IEnumerable<string> projectCodes)
        {
            return await _context.Projects
                .Where(p => projectCodes.Contains(p.ProjectCode))
                .ToListAsync();
        }

        public async Task UpdateProjectsAsync(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                var existingProject = await _context.Projects
                    .FirstOrDefaultAsync(p => p.ProjectCode == project.ProjectCode);

                if (existingProject != null)
                {
                    // Update properties
                    existingProject.SQA = project.SQA;
                    existingProject.ForecastedEndDate = project.ForecastedEndDate;
                    existingProject.VOCEligibilityDate = project.VOCEligibilityDate;
                    existingProject.ProjectType = project.ProjectType;
                    existingProject.Domain = project.Domain;
                    existingProject.DatabaseUsed = project.DatabaseUsed;
                    existingProject.CloudUsed = project.CloudUsed;
                    existingProject.FeedbackStatus = project.FeedbackStatus;
                    existingProject.MailStatus = project.MailStatus;

                    // Update other properties if needed


                    _context.Projects.Update(existingProject);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Existing methods for CRUD operations with the Project entity
        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }

        public Project GetProjectById(int projectId)
        {
            return _context.Projects.Find(projectId);
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }

        public void Update(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<Project> GetProjectByCodeAsync(string projectCode)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectCode == projectCode);
        }

        public void AddProjectEditableFields(Project project)
        {
            throw new NotImplementedException();
        }

        public void UpdateProjectEditableFields(int projectId, Project project)
        {
            throw new NotImplementedException();
        }

        public Project GetProjectByCode(string projectCode)
        {
            throw new NotImplementedException();
        }
    }
}
