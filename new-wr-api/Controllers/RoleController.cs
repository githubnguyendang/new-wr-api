using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using new_wr_api.Data;
using new_wr_api.Models;
using new_wr_api.Repositories;

namespace new_wr_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleRepository _repo;

        public RoleController(RoleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("roles")]
        //[Authorize(Roles = "admin")]
        public async Task<List<ApplicationRole>> GetAllRoles()
        {
            return await _repo.GetAllRolesAsync();
        }

        [HttpGet]
        [Route("role/{roleId}")]
        //[Authorize(Roles = "admin")]
        public async Task<ApplicationRole?> GetRoleById(string roleId)
        {
            return await _repo.GetRoleByIdAsync(roleId);
        }

        [HttpPost]
        [Route ("create-role")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApplicationRole>> CreateRole(RoleViewModel model)
        {
            var res = await _repo.CreateRoleAsync(model.roleName, model.isDefault);
            if (res.Succeeded)
            {
                return Ok(new
                {
                    message = "Role is created",
                    res = res
                });
            }
            return BadRequest(new
            {
                message = res
            });
        }

        [HttpPost]
        [Route("detete-role/{roleId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApplicationRole>> DeleteRole(string roleId)
        {
            var res = await _repo.DeleteRoleAsync(roleId);
            if (res == false)
            {
                return BadRequest(new
                {
                    message = res
                });
            }
            return Ok(new
            {
                message = "Role is deleted."
            });
        }
    }
}
