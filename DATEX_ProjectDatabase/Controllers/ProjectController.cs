using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("sync-external")]
        public async Task<IActionResult> SyncWithExternalApi()
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

                // Check if the project already exists in the database by ProjectCode or other unique identifier
                var existingProject = _projectRepository.GetProjectByCode(externalProject.ProjectCode);

                if (existingProject != null)
                {
                    // Update existing project with external API data (only non-editable fields)
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

            return Ok("Projects synced with external API");
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var projects = _projectRepository.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetProjectById(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost("editable")]
        public IActionResult AddProjectEditableFields([FromBody] Project project)
        {
            _projectRepository.AddProjectEditableFields(project);
            _projectRepository.Save();
            return CreatedAtAction("GetProjectById", new { id = project.ProjectId }, project);
        }

        [HttpPut("editable/{id}")]
        public IActionResult UpdateProjectEditableFields(int id, [FromBody] Project project)
        {
            var existingProject = _projectRepository.GetProjectById(id);

            if (existingProject == null)
            {
                return NotFound();
            }

            _projectRepository.UpdateProjectEditableFields(id, project);
            _projectRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }

            _projectRepository.DeleteProject(id);
            _projectRepository.Save();

            return NoContent();
        }

    }
}
