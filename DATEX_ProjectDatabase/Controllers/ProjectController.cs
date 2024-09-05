using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.Service;
using Microsoft.AspNetCore.Mvc;
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

        public ProjectController(IProjectRepository projectRepository, IExternalApiService externalApiService)
        {
            _projectRepository = projectRepository;
            _externalApiService = externalApiService;
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
                        ProjectEndDate = externalProject.ProjectEndDate,
                        ProjectManager = externalProject.ProjectManager,
                        ContractType = externalProject.ContractType,
                        NumberOfResources = externalProject.NumberOfResources,
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
                        existingProject.ProjectEndDate = externalProject.ProjectEndDate;
                        existingProject.ProjectManager = externalProject.ProjectManager;
                        existingProject.ContractType = externalProject.ContractType;
                        existingProject.NumberOfResources = externalProject.NumberOfResources;
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
                // Log the exception (implement logging as needed)
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

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

                return NoContent();
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
                    }

                    return existingProject;
                }).Where(p => p != null).ToList();

                await _projectRepository.UpdateProjectsAsync(updatedProjects);

                return Ok(new { message = "Projects updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Get Filtered Projects
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
