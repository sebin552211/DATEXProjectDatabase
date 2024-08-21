using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using Microsoft.AspNetCore.Mvc;

namespace DATEX_ProjectDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoleById(int id)
        {
            var role = _roleRepository.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        public IActionResult AddRole([FromBody] Role role)
        {
            _roleRepository.AddRole(role);
            _roleRepository.Save();
            return CreatedAtAction("GetRoleById", new { id = role.RoleId }, role);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            _roleRepository.UpdateRole(role);
            _roleRepository.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            _roleRepository.DeleteRole(id);
            _roleRepository.Save();
            return NoContent();
        }
    }
}
