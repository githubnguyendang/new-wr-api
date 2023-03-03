using Microsoft.AspNetCore.Mvc;
using new_wr_api.Models;
using new_wr_api.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace new_wr_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route ("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var res = await _repo.RegisterAsync(model);
            if(res == null)
            {
                return BadRequest(new
                {
                    message = "Register Failed. User is already exits",
                });
            }
            if(res.Succeeded == false) {
                return BadRequest(res);
            }
            return Ok(new
            {
                message = "User is registered",
                res = res
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            var res = await _repo.LoginAsync(model);
            if(string.IsNullOrEmpty(res))
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(HttpContext context)
        {
            var res = await _repo.LogoutAsync(context);
            if (res == false)
            {
                return BadRequest(false);
            }
            return Ok(true);
        }
    }

}
