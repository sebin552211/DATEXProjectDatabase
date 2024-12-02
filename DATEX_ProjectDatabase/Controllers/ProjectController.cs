using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.Repository;
using DATEX_ProjectDatabase.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExternalApiService _externalApiService;
        private readonly IProjectManagerRepository _projectManagerRepository;
        private IProjectRepository object1;
        private IExternalApiService object2;
        private IProjectRepository @object;
        private IExternalApiService object3;
        private IProjectRepository object4;
        private IExternalApiService object5;


        public ProjectController(IProjectRepository projectRepository, IExternalApiService externalApiService, IProjectManagerRepository projectManagerRepository, IProjectRepository object1,  IExternalApiService object2, IProjectRepository @object,
        IExternalApiService object3, IProjectRepository object4, IExternalApiService object5)
        {
            _projectRepository = projectRepository;
            _externalApiService = externalApiService;
            _projectManagerRepository = projectManagerRepository;
            this.object1 = object1;
            this.object2 = object2;
            this.@object = @object;
            this.object3 = object3;
                this.object4 = object4;
            this.object5 = object5;
        }

  

        // Sync with External API
        [HttpGet("sync-external")]
        public async Task<IActionResult> SyncWithExternalApi()
        {
            try
            {
                var externalProjects = await _externalApiService.GetProjectsFromExternalApiAsync();

                foreach (var externalProject in externalProjects)
                {
                    var project = new Project
                    {
                        ProjectCode = externalProject.ProjectCode,
                        ProjectName = externalProject.ProjectName,
                        DU = externalProject.DU,
                        DUHead = externalProject.DUHead,
                        ProjectStartDate = externalProject.ProjectStartDate,
                        ProjectEndDate = (DateTime)externalProject.ProjectEndDate,
                        ProjectManager = externalProject.ProjectManager,
                        ContractType = externalProject.ContractType,
                        NumberOfResources = externalProject.NumberOfResources.HasValue ? (int?)externalProject.NumberOfResources.Value : null,
                        CustomerName = externalProject.CustomerName,
                        Region = externalProject.Region,
                        Technology = externalProject.Technology,
                        Status = externalProject.Status
                    };

                    // Use asynchronous method to get project by code
                    var existingProject = await _projectRepository.GetProjectByCodeAsync(externalProject.ProjectCode);

                    if (existingProject != null)
                    {
                        // Update existing project with external API data
                        existingProject.ProjectName = externalProject.ProjectName;
                        existingProject.DU = externalProject.DU;
                        existingProject.DUHead = externalProject.DUHead;
                        existingProject.ProjectStartDate = externalProject.ProjectStartDate;
                        existingProject.ProjectEndDate = (DateTime)externalProject.ProjectEndDate;
                        existingProject.ProjectManager = externalProject.ProjectManager;
                        existingProject.ContractType = externalProject.ContractType;
                        existingProject.NumberOfResources = externalProject.NumberOfResources.HasValue ? (int?)externalProject.NumberOfResources.Value : null;
                        existingProject.CustomerName = externalProject.CustomerName;
                        existingProject.Region = externalProject.Region;
                        existingProject.Technology = externalProject.Technology;
                        existingProject.Status = externalProject.Status;

                        _projectRepository.Update(existingProject);
                    }
                    else
                    {
                        _projectRepository.Add(project);
                    }
                }

                // Use asynchronous SaveChanges
                await _projectRepository.SaveAsync();

                return Ok(new { message = "Projects synced with external API" });
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
        [HttpGet("GetProjectsByFinancialQuarter")]
        public async Task<IActionResult> GetProjectsByFinancialQuarter(string financialYear, int quarter)
        {
            var years = financialYear.Split('-');
            if (years.Length != 2 || !int.TryParse(years[0], out int startYear) || !int.TryParse(years[1], out int endYear) || endYear != startYear + 1)
            {
                return BadRequest("Invalid financial year format. It must be in the format 'YYYY-YYYY' with consecutive years.");
            }

            if (quarter < 1 || quarter > 4)
            {
                return BadRequest("Invalid quarter. It must be between 1 and 4.");
            }

            var (startDate, endDate) = GetFinancialQuarterDates(startYear, quarter);

            var projects = await _projectRepository.GetProjectsByDateRangeAsync(startDate, endDate);
            return Ok(projects);
        }
        [HttpGet("GetProjectsByFinancialYear")]
        public async Task<IActionResult> GetProjectsByFinancialYear(string financialYear)
        {
            var years = financialYear.Split('-');
            if (years.Length != 2 || !int.TryParse(years[0], out int startYear) || !int.TryParse(years[1], out int endYear) || endYear != startYear + 1)
            {
                return BadRequest("Invalid financial year format. It must be in the format 'YYYY-YYYY' with consecutive years.");
            }

            var startDate = new DateTime(startYear, 4, 1); // Financial year starts on April 1st
            var endDate = new DateTime(endYear, 3, 31);    // Financial year ends on March 31st

            var projects = await _projectRepository.GetProjectsByDateRangeAsync(startDate, endDate);

            if (projects == null || !projects.Any())
            {
                return NotFound(new { Message = $"No projects found for the financial year {financialYear}." });
            }

            return Ok(projects);
        }

        [HttpPost("{projectId}/remarks")]
        public async Task<IActionResult> AddProjectRemarks(int projectId, [FromBody] ProjectRemarksUpdate model)
        {
            var updatedProject = await _projectRepository.AddProjectRemarksAsync(projectId, model.VocRemarks);

            if (updatedProject == null)
            {
                return NotFound();
            }

            return Ok(updatedProject);
        }

        [HttpPut("{projectId}/remarks")]
        public async Task<IActionResult> UpdateProjectRemarks(int projectId, [FromBody] ProjectRemarksUpdate model)
        {
            var updatedProject = await _projectRepository.UpdateProjectRemarksAsync(projectId, model.VocRemarks);

            if (updatedProject == null)
            {
                return NotFound();
            }

            return Ok(updatedProject);
        }

        [HttpDelete("{projectId}/remarks")]
        public async Task<IActionResult> DeleteProjectRemarks(int projectId)
        {
            var updatedProject = await _projectRepository.DeleteProjectRemarksAsync(projectId);

            if (updatedProject == null)
            {
                return NotFound();
            }

            return Ok(updatedProject);
        }

        [HttpGet("{projectId}/PMIntiateDate")]
        public async Task<IActionResult> GetPMIntitiateDate(int projectId)
        {
           var project = await _projectRepository.GetProjectByIdAsync(projectId);
                if (project == null || project.PMInitiateDate == null)
                {
                    return NotFound("Project or PMInitiateDate not found");
                }
                return Ok(project.PMInitiateDate);
        }

        [HttpPost("{projectId}/PMIntiateDate")]
        public async Task<IActionResult> AddPMIntitiateDates(int projectId, [FromBody] PMInitiateDateDTO PMIntitiateDate)
        {
            try
            {
                var project = await _projectRepository.AddPMIntitiateDate(projectId, PMIntitiateDate.PMInitiateDate);
                return Ok(new { success = true, message = "PM Initiate Date received." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "An error occurred while processing the request.", message = ex.Message });
            }
        }
        [HttpDelete("{projectId}/PMIntiateDate")]
        public async Task<IActionResult> DeletePMIntitiateDates(int projectId)
        {
            var project = await _projectRepository.DeletePMIntitiateDate(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpGet("{projectId}/VOCFeedbackReceivedDate")]
        public async Task<ActionResult<DateTime?>> GetVOCFeedbackReceivedDate(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null || project.VOCFeedbackReceivedDate == null)
            {
                return NotFound("Project or VocFeedbackDate not found");
            }
            return Ok(project.VOCFeedbackReceivedDate);
        }

        [HttpPost("{projectId}/VOCFeedbackReceivedDate")]
        public async Task<IActionResult> AddVOCFeedbackReceivedDate(int projectId, [FromBody] VOCFeedbackDto vOCFeedbackDto)
        {
           try
            {
                // Process feedback
                await _projectRepository.AddVOCFeedbackReceivedDateAsync(projectId, vOCFeedbackDto.VOCFeedbackReceivedDate);
                return Ok(new { success = true, message = "Feedback received." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "An error occurred while processing the request.", message = ex.Message });
            }
            //return Ok("VOCFeedbackReceivedDate updated successfully");
        }

        [HttpDelete("{projectId}/VOCFeedbackReceivedDate")]
        public async Task<IActionResult> DeleteVOCFeedbackReceivedDateByProjectId(int projectId)
        {
            var updatedProject = await _projectRepository.DeleteVOCFeedbackReceivedDateByProjectIdAsync(projectId);

            if (updatedProject == null)
            {
                return NotFound();
            }

            return Ok(updatedProject);
        }

        private (DateTime startDate, DateTime endDate) GetFinancialQuarterDates(int startYear, int quarter)
        {
            DateTime startDate;
            DateTime endDate;

            switch (quarter)
            {
                case 1:
                    startDate = new DateTime(startYear, 4, 1);  // April 1st
                    endDate = new DateTime(startYear, 6, 30);   // June 30th
                    break;
                case 2:
                    startDate = new DateTime(startYear, 7, 1);  // July 1st
                    endDate = new DateTime(startYear, 9, 30);   // September 30th
                    break;
                case 3:
                    startDate = new DateTime(startYear, 10, 1); // October 1st
                    endDate = new DateTime(startYear, 12, 31);  // December 31st
                    break;
                case 4:
                    startDate = new DateTime(startYear + 1, 1, 1); // January 1st of the next year
                    endDate = new DateTime(startYear + 1, 3, 31);  // March 31st of the next year
                    break;
                default:
                    throw new ArgumentException("Invalid quarter. It must be between 1 and 4.");
            }

            return (startDate, endDate);
        }

        [HttpPost("{PMName}")]
        public async Task<IActionResult> AddEmailforPRojectManager(string PMName, string PMEmail)
        {
            try
            {
                var Project = await _projectManagerRepository.AddProjectManagerMail(PMName, PMEmail);

                Project.PMMails = PMEmail;
                return Ok(new { success = true, message = "PM Mail received." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Either the given Project Manager's name doesn't exist or spelling is wrong ! !", message = ex.Message });
            }
        }

        [HttpDelete("{PMName}/PMMails")]
        public async Task<IActionResult> DeleteEmailforProjectManager(string PMName)
        {
            try
            {
                await _projectManagerRepository.DeleteProjectManagerMail(PMName);
                return Ok(new { success = true, message = "PM Mail Deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Either the given Project Manager's name doesn't exist or spelling is wrong ! !", message = ex.Message });
            }
        }

        /*[HttpPost("{Date}")]
        public async FeedbackDate(string Exceldate)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }*/

        // Get All Projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var projects = await _projectRepository.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Get Project by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _projectRepository.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Add Project with Editable Fields
        [HttpPost("editable")]
        public async Task<IActionResult> AddProjectEditableFields([FromBody] Project project)
        {
            try
            {
                _projectRepository.AddProjectEditableFields(project);
                await _projectRepository.SaveAsync();
                return CreatedAtAction("GetProjectById", new { id = project.ProjectId }, project);
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Update Project with Editable Fields
        [HttpPut("editable/{id}")]
        public async Task<IActionResult> UpdateProjectEditableFields(int id, [FromBody] Project project)
        {
            try
            {
                var existingProject = await _projectRepository.GetProjectByIdAsync(id);

                if (existingProject == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                _projectRepository.UpdateProjectEditableFields(id, project);
                await _projectRepository.SaveAsync();

                // Fetch the updated project to return in the response
                var updatedProject = await _projectRepository.GetProjectByIdAsync(id);

                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Delete Project
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _projectRepository.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                _projectRepository.DeleteProject(id);
                await _projectRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Search Projects
        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects(string query)
        {
            try
            {
                var projects = await Task.Run(() => _projectRepository.SearchProjects(query));

                if (projects == null || !projects.Any())
                {
                    return NotFound();
                }

                return Ok(projects);
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Get Paged Projects
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProjects(int pageNumber, int pageSize)
        {
            try
            {
                var pagedProjects = await Task.Run(() => _projectRepository.GetPagedProjects(pageNumber, pageSize));
                var totalProjects = await Task.Run(() => _projectRepository.GetTotalProjectsCount());

                var response = new
                {
                    TotalProjects = totalProjects,
                    Projects = pagedProjects
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Update Projects from Excel Data
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProjects([FromBody] List<Project> excelRows)
        {
            if (excelRows == null || !excelRows.Any())
            {
                return BadRequest(new { message = "No data provided" });
            }

            try
            {
                var projectCodes = excelRows.Select(row => row.ProjectCode).ToList();
                var existingProjects = await _projectRepository.GetProjectsByCodesAsync(projectCodes);

                if (existingProjects == null || !existingProjects.Any())
                {
                    return BadRequest(new { message = "No existing projects found for the provided codes" });
                }

                var updatedProjects = excelRows.Select(row =>
                {
                    var existingProject = existingProjects.FirstOrDefault(p => p.ProjectCode == row.ProjectCode);

                    if (existingProject != null)
                    {
                        // Map ExcelRow to existing project
                        existingProject.SQA = row.SQA;
                        existingProject.ForecastedEndDate = row.ForecastedEndDate;
                        existingProject.VOCEligibilityDate = row.VOCEligibilityDate;
                        existingProject.ProjectType = row.ProjectType;
                        existingProject.Domain = row.Domain;
                        existingProject.DatabaseUsed = row.DatabaseUsed;
                        existingProject.CloudUsed = row.CloudUsed;
                        existingProject.FeedbackStatus = row.FeedbackStatus;
                        existingProject.MailStatus = row.MailStatus;
                        existingProject.Technology = row.Technology;
                        // Add any other fields that need updating
                    }

                    return existingProject;
                }).ToList();

                await _projectRepository.UpdateProjectsAsync(updatedProjects);

                return Ok(new { message = "Projects updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                var errorMessage = $"Exception: {ex.Message}, StackTrace: {ex.StackTrace}, InnerException: {ex.InnerException?.Message}";
                Console.WriteLine(errorMessage);
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("filter")]

        public async Task<IActionResult> GetFilteredProjects(
     [FromQuery] string du = null,
     [FromQuery] string duHead = null,
     [FromQuery] DateTime? projectStartDate = null,
     [FromQuery] DateTime? projectEndDate = null,
     [FromQuery] string projectManager = null,
     [FromQuery] string contractType = null,
     [FromQuery] string customerName = null,
     [FromQuery] string region = null,
     [FromQuery] string technology = null,
     [FromQuery] string status = null,
     [FromQuery] string sqa = null,
     [FromQuery] DateTime? vocEligibilityDate = null,
     [FromQuery] string projectType = null,
     [FromQuery] string domain = null,
     [FromQuery] string databaseUsed = null,
     [FromQuery] string cloudUsed = null,
     [FromQuery] string feedbackStatus = null,
     [FromQuery] string mailStatus = null)
        {
            try
            {
                var projects = await _projectRepository.GetFilteredProjectsAsync(
                    du,
                    duHead,
                    projectStartDate,
                    projectEndDate,
                    projectManager,
                    contractType,
                    customerName,
                    region,
                    technology,
                    status,
                    sqa,
                    vocEligibilityDate,
                    projectType,
                    domain,
                    databaseUsed,
                    cloudUsed,
                    feedbackStatus,
                    mailStatus);

                if (projects == null || !projects.Any())
                {
                    return NotFound(new { message = "No projects found matching the criteria" });
                }

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }


}
