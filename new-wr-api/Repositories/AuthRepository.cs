using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using new_wr_api.Data;
using new_wr_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace new_wr_api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SignInManager<ApplicationUser> _signInManager { get; }

        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration) {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists" });
            }

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var role = await _roleManager.Roles.FirstOrDefaultAsync(u => u.IsDefault == true);

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {

                // Add default role to the user
                if(role != null)
                {
                    if(role.Name != null)
                    {
                        if (role.IsDefault)
                        {
                            await _userManager.AddToRoleAsync(newUser, role.Name);
                        }
                    }
                }

                return result;
            }

            return result;
        }


        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var res = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if(!res.Succeeded)
            {
                return string.Empty;
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if( user != null )
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
            };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            return string.Empty;
        }

        public async Task<bool> LogoutAsync(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }

    }
}
