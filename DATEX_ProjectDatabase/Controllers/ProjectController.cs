using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
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

                    var existingProject = _projectRepository.GetProjectByCode(externalProject.ProjectCode);

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

                _projectRepository.Save();

                return Ok(new { message = "Projects synced with external API" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // Get All Projects
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            try
            {
                var projects = _projectRepository.GetAllProjects();
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
        public IActionResult GetProjectById(int id)
        {
            try
            {
                var project = _projectRepository.GetProjectById(id);
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
        public IActionResult AddProjectEditableFields([FromBody] Project project)
        {
            try
            {
                _projectRepository.AddProjectEditableFields(project);
                _projectRepository.Save();
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
        public IActionResult UpdateProjectEditableFields(int id, [FromBody] Project project)
        {
            try
            {
                var existingProject = _projectRepository.GetProjectById(id);

                if (existingProject == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                _projectRepository.UpdateProjectEditableFields(id, project);
                _projectRepository.Save();

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
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var project = _projectRepository.GetProjectById(id);
                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                _projectRepository.DeleteProject(id);
                _projectRepository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
        [HttpGet("search")]
        public IActionResult SearchProjects(string query)
        {
            var projects = _projectRepository.SearchProjects(query);

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }
        [HttpGet("paged")]
        public IActionResult GetPagedProjects(int pageNumber, int pageSize)
        {
            var pagedProjects = _projectRepository.GetPagedProjects(pageNumber, pageSize);
            var totalProjects = _projectRepository.GetTotalProjectsCount();

            var response = new
            {
                TotalProjects = totalProjects,
                Projects = pagedProjects
            };

            return Ok(response);
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

       /* [HttpGet("recent-projects/customers")]*/
       /* public async Task<IActionResult> GetRecentProjectCustomers()
        {
            try
            {
                var recentProjects = await _projectRepository.GetProjectsFromLastThreeMonthsAsync();

                var customerProjects = recentProjects
                    .GroupBy(p => p.CustomerName)
                    .Select(group => new
                    {
                        CustomerName = group.Key,
                        NumberOfProjects = group.Count(),
                        Projects = group.ToList()
                    })
                    .ToList();

                return Ok(customerProjects);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
*/

    }


}
