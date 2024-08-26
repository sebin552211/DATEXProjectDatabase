using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagerController : ControllerBase
    {
        private readonly IProjectManagerService _projectManagerService;

        public ProjectManagerController(IProjectManagerService projectManagerService)
        {
            _projectManagerService = projectManagerService;
        }

        // GET: api/<ProjectManagerController>
        [HttpGet]
        public IActionResult GetAllProjectManagers()
        {
            // Implementation needed to return all Project Managers
            return NotImplemented();
        }

        // GET api/<ProjectManagerController>/5
        [HttpGet("{id}")]
        public IActionResult GetProjectManager(int id)
        {
            // Implementation needed to return a specific Project Manager by ID
            return NotImplemented();
        }

        // POST api/<ProjectManagerController>
        [HttpPost]
        public async Task<IActionResult> PostProjectManager([FromBody] ProjectManagers projectManager)
        {
            try
            {
                if (projectManager == null)
                {
                    return BadRequest("Project manager data is required.");
                }

                // Assuming you want to handle insertion via POST
                await _projectManagerService.UpsertProjectManagerAsync(projectManager, projectManager.ProjectId);
                return CreatedAtAction(nameof(GetProjectManager), new { id = projectManager.ProjectId }, projectManager);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT api/<ProjectManagerController>/5
        [HttpPut("{projectId}")]
        public async Task<IActionResult> PutProjectManager(int projectId, [FromBody] ProjectManagers projectManager)
        {
            if (projectManager == null)
            {
                return BadRequest("Project manager data is required.");
            }

            if (projectId != projectManager.ProjectId)
            {
                return BadRequest("Project ID mismatch.");
            }

            try
            {
                await _projectManagerService.UpsertProjectManagerAsync(projectManager, projectId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // DELETE api/<ProjectManagerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {
            // Implementation needed to delete a Project Manager
            return NotImplemented();
        }

        // Helper method for not implemented actions
        private IActionResult NotImplemented()
        {
            return StatusCode(501, new { message = "Not Implemented" });
        }
    }
}
