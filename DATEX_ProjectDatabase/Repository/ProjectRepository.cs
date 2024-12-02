using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        return await _context.Projects.FindAsync(projectId);
    }

    public async Task<Project> GetProjectByCodeAsync(string projectCode)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectCode == projectCode);
    }

    public async Task<Project> GetProjectsByPMNameAsync(string PMName)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectManager == PMName);
    }

    public async Task<Project> AddPMIntitiateDate(int projectId, DateTime? PMInitiateDate)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }
        project.PMInitiateDate = PMInitiateDate;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> DeletePMIntitiateDate(int projectId)
    {
        var project = await GetProjectByIdAsync(projectId);

        if (project != null)
        {
            project.PMInitiateDate = null;
            await _context.SaveChangesAsync();
        }

        return project;

    }

    public async Task<Project> DeleteVOCFeedbackReceivedDateByProjectIdAsync(int projectId)
    {
        var project = await GetProjectByIdAsync(projectId);

        if (project != null)
        {
            project.VOCFeedbackReceivedDate = null;
            await _context.SaveChangesAsync(); 
        }

        return project;
    }

    public async Task AddVOCFeedbackReceivedDateAsync(int projectId, DateTime? vocFeedbackReceivedDate)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        project.VOCFeedbackReceivedDate = vocFeedbackReceivedDate;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project> AddProjectRemarksAsync(int projectId, string remarks)
    {
        var project = await GetProjectByIdAsync(projectId);

        if (project != null)
        {
            project.VocRemarks = remarks; // Update the remarks
            await _context.SaveChangesAsync(); // Save the changes
        }

        return project; // Return the updated project
    }

    public async Task<Project> UpdateProjectRemarksAsync(int projectId, string remarks)
    {
        var project = await GetProjectByIdAsync(projectId);

        if (project != null)
        {
            project.VocRemarks = remarks; // Update the remarks
            await _context.SaveChangesAsync(); // Save the changes
        }

        return project; // Return the updated project
    }

    public async Task<Project> DeleteProjectRemarksAsync(int projectId)
    {
        var project = await GetProjectByIdAsync(projectId);

        if (project != null)
        {
            project.VocRemarks = null; // Clear the remarks
            await _context.SaveChangesAsync(); // Save the changes
        }

        return project; // Return the updated project
    }

    public async Task<List<Project>> GetProjectsByCodesAsync(IEnumerable<string> projectCodes)
    {
        return await _context.Projects.Where(p => projectCodes.Contains(p.ProjectCode)).ToListAsync();
    }
    public async Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Projects
            .Where(p => p.VOCEligibilityDate >= startDate && p.VOCEligibilityDate <= endDate)
            .ToListAsync();
    }
    public async Task UpdateProjectsAsync(IEnumerable<Project> projects)
    {
        _context.Projects.UpdateRange(projects);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Add(Project project)
    {
        _context.Projects.Add(project);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void DeleteProject(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
        }
    }

    public IEnumerable<Project> SearchProjects(string query)
    {
        return _context.Projects
            .Where(p => p.ProjectName.Contains(query) || p.ProjectCode.Contains(query))
            .ToList();
    }

    public IEnumerable<Project> GetPagedProjects(int pageNumber, int pageSize)
    {
        return _context.Projects
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetTotalProjectsCount()
    {
        return _context.Projects.Count();
    }

    public void AddProjectEditableFields(Project project)
    {
        _context.Projects.Add(project);
    }

    public void UpdateProjectEditableFields(int id, Project project)
    {
        var existingProject = _context.Projects.Find(id);
        if (existingProject != null)
        {
            existingProject.SQA = project.SQA;
            existingProject.ForecastedEndDate = project.ForecastedEndDate;
            existingProject.ProjectType = project.ProjectType;
            existingProject.Domain = project.Domain;
            existingProject.DatabaseUsed = project.DatabaseUsed;
            existingProject.CloudUsed = project.CloudUsed;
            existingProject.Technology = project.Technology;

            // Check if feedback status is being changed to "Received"
            if (project.FeedbackStatus == "Received" && existingProject.FeedbackStatus != "Received")
            {
                existingProject.VOCEligibilityDate = DateTime.Now.AddMonths(6);
            }
            else if (project.FeedbackStatus != "Received")
            {
                // Reset VOCEligibilityDate if feedback status is changed from "Received" to something else
                existingProject.VOCEligibilityDate = null;
            }

            existingProject.FeedbackStatus = project.FeedbackStatus;
            existingProject.MailStatus = project.MailStatus;
        }
    }


    public async Task<IEnumerable<Project>> GetFilteredProjectsAsync(string du = null, string duHead = null, DateTime? projectStartDate = null, DateTime? projectEndDate = null, string projectManager = null, string contractType = null, string customerName = null, string region = null, string technology = null, string status = null, string sqa = null, DateTime? vocEligibilityDate = null, string projectType = null, string domain = null, string databaseUsed = null, string cloudUsed = null, string feedbackStatus = null, string mailStatus = null)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(du))
            query = query.Where(p => p.DU == du);

        if (!string.IsNullOrEmpty(duHead))
            query = query.Where(p => p.DUHead == duHead);

        if (projectStartDate.HasValue)
            query = query.Where(p => p.ProjectStartDate >= projectStartDate.Value);

        if (projectEndDate.HasValue)
            query = query.Where(p => p.ProjectEndDate <= projectEndDate.Value);

        if (!string.IsNullOrEmpty(projectManager))
            query = query.Where(p => p.ProjectManager == projectManager);

        if (!string.IsNullOrEmpty(contractType))
            query = query.Where(p => p.ContractType == contractType);

        if (!string.IsNullOrEmpty(customerName))
            query = query.Where(p => p.CustomerName == customerName);

        if (!string.IsNullOrEmpty(region))
            query = query.Where(p => p.Region == region);

        if (!string.IsNullOrEmpty(technology))
            query = query.Where(p => p.Technology == technology);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrEmpty(sqa))
            query = query.Where(p => p.SQA == sqa);

        if (vocEligibilityDate.HasValue)
            query = query.Where(p => p.VOCEligibilityDate == vocEligibilityDate.Value);

        if (!string.IsNullOrEmpty(projectType))
            query = query.Where(p => p.ProjectType == projectType);

        if (!string.IsNullOrEmpty(domain))
            query = query.Where(p => p.Domain == domain);

        if (!string.IsNullOrEmpty(databaseUsed))
            query = query.Where(p => p.DatabaseUsed == databaseUsed);

        if (!string.IsNullOrEmpty(cloudUsed))
            query = query.Where(p => p.CloudUsed == cloudUsed);

        if (!string.IsNullOrEmpty(feedbackStatus))
            query = query.Where(p => p.FeedbackStatus == feedbackStatus);

        if (!string.IsNullOrEmpty(mailStatus))
            query = query.Where(p => p.MailStatus == mailStatus);

        return await query.ToListAsync();
    }

}
