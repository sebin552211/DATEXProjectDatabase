using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    /*Task<Project> GetVOCFeedbackReceivedDateByProjectIdAsync(int projectId);*/
    Task<Project> AddPMIntitiateDate(int projectId, DateTime? PMInitiateDate);

    Task<Project> DeletePMIntitiateDate(int projectId);
    Task<Project> DeleteVOCFeedbackReceivedDateByProjectIdAsync(int projectId);
    Task AddVOCFeedbackReceivedDateAsync(int projectId, DateTime? vocFeedbackDate);//DateTime
    Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate);
    public Task<Project> AddProjectRemarksAsync(int projectId, string remarks);
    Task<Project> UpdateProjectRemarksAsync(int projectId, string VocRemark);
    Task<Project> DeleteProjectRemarksAsync(int projectId);
    Task<Project> GetProjectsByPMNameAsync(string PMName);
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
