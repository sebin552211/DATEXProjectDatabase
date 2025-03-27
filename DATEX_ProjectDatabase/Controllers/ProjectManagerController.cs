/*using DATEX_ProjectDatabase.Interfaces;
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
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectManagerRepository _projectManagerRepository;
        private IProjectManagerService @object;

        public ProjectManagerController(IProjectManagerService projectManagerService, IProjectRepository projectRepository, IProjectManagerRepository projectManagerRepository)
        {
            _projectManagerService = projectManagerService;
            _projectRepository = projectRepository;
            _projectManagerRepository = projectManagerRepository;
        }

        public ProjectManagerController(IProjectManagerService @object)
        {
            this.@object = @object;
        }

        // GET: api/<ProjectManagerController>
        [HttpGet]
        public IActionResult GetAllProjectManagers()
        {
            // Implementation needed to return all Project Managers
            return NotImplemented();
        }

        // GET api/<ProjectManagerController>/5
        [HttpGet("GetProjectMangerName/{PMName}")]
        public async  Task<IActionResult> GetProjectManager(string PMName)
        {
            var pmname = await _projectManagerService.GetPMMailAsync(PMName);
            if (pmname == null)
            {
                return NotFound("Either the given Project Manager's name doesn't exist or spelling is wrong ! !");
            }
            return Ok(pmname);
            // Implementation needed to return a specific Project Manager by ID
            *//*return NotImplemented();*//*
        }


        // POST api/<ProjectManagerController>
        [HttpPost]
        public async Task<IActionResult> PostProjectManagerwithName([FromBody] ProjectManagers projectManager)
        {
            try
            {
                if (projectManager == null)
                {
                    return BadRequest("Project manager data is required.");
                }

                // Assuming you want to handle insertion via POST
                await _projectManagerService.UpsertProjectManagerAsync(projectManager, projectManager.Name);
                return CreatedAtAction(nameof(GetProjectManager), new { PMName = projectManager.Name}, projectManager);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT api/<ProjectManagerController>/5
        [HttpPut]
        public async Task<IActionResult> PutProjectManager(string PMName, [FromBody] ProjectManagers projectManager)
        {
            if (projectManager == null)
            {
                return BadRequest("Project manager data is required.");
            }

            if (PMName != projectManager.Name)
            {
                return BadRequest("Project ID mismatch.");
            }

            try
            {
                await _projectManagerService.UpsertProjectManagerAsync(projectManager, PMName);
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
        [HttpDelete]
        public async Task<IActionResult> DeleteProjectManager(string PMName)
        {
            await Task.CompletedTask;

            return new StatusCodeResult(StatusCodes.Status501NotImplemented);
        }

        // Helper method for not implemented actions
        private IActionResult NotImplemented()
        {
            return StatusCode(501, new { message = "Not Implemented" });
        }
    }
}
*/