using Microsoft.AspNetCore.Identity;
using new_wr_api.Models;

namespace new_wr_api.Repositories
{
    public interface IAuthRepository
    {
        public Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        public Task<string> LoginAsync(LoginViewModel model);
        public Task<bool> LogoutAsync(HttpContext context);
    }
}
