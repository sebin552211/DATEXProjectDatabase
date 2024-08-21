using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }

        public Project GetProjectById(int projectId)
        {
            return _context.Projects.Find(projectId);
        }

        public void AddProjectEditableFields(Project project)
        {
            var newProject = new Project
            {
                // Only set the editable fields
                SQA = project.SQA,
                ForecastedEndDate = project.ForecastedEndDate,
                VOCEligibilityDate = project.VOCEligibilityDate,
                ProjectDurationInDays = project.ProjectDurationInDays,
                ProjectDurationInMonths = project.ProjectDurationInMonths,
                ProjectType = project.ProjectType,
                Domain = project.Domain,
                DatabaseUsed = project.DatabaseUsed,
                CloudUsed = project.CloudUsed,
                FeedbackStatus = project.FeedbackStatus,
                MailStatus = project.MailStatus
            };

            _context.Projects.Add(newProject);
        }

        public void UpdateProjectEditableFields(int projectId, Project project)
        {
            var existingProject = _context.Projects.Find(projectId);

            if (existingProject != null)
            {
                // Update only the editable fields
                existingProject.SQA = project.SQA;
                existingProject.ForecastedEndDate = project.ForecastedEndDate;
                existingProject.VOCEligibilityDate = project.VOCEligibilityDate;
                existingProject.ProjectDurationInDays = project.ProjectDurationInDays;
                existingProject.ProjectDurationInMonths = project.ProjectDurationInMonths;
                existingProject.ProjectType = project.ProjectType;
                existingProject.Domain = project.Domain;
                existingProject.DatabaseUsed = project.DatabaseUsed;
                existingProject.CloudUsed = project.CloudUsed;
                existingProject.FeedbackStatus = project.FeedbackStatus;
                existingProject.MailStatus = project.MailStatus;
            }
        }

        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }

        public void Update(Project project) // Implement the Update method
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Project GetProjectByCode(string projectCode)
        {
            return _context.Projects.FirstOrDefault(p => p.ProjectCode == projectCode);
        }

    }


}
