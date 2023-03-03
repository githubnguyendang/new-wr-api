using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using new_wr_api.Data;
using new_wr_api.Models;

namespace new_wr_api.Repositories
{
    public class RoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DatabaseContext _context;

        public RoleRepository(IServiceProvider serviceProvider, DatabaseContext context)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            _context = context;
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
        {
            if (roleId == null)
            {
                return null;
            }
            var res = await _roleManager.Roles.FirstOrDefaultAsync(u => u.Id == roleId, CancellationToken.None);
            return res;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName, bool isDefault)
        {
            var role = new ApplicationRole
            {
                Name = roleName,
                IsDefault = isDefault
            };
            var res = await _roleManager.CreateAsync(role);
            return res;
        }

        public async Task<IdentityResult?> UpdateRoleAsync(string roleId, RoleViewModel model)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(u => u.Id == roleId, CancellationToken.None);

            // Update role properties based on the RegisterViewModel
            if (role == null)
            {
                return null;
            }

            role.Name = model.roleName;
            role.IsDefault = model.isDefault;
            var result = await _roleManager.UpdateAsync(role);
            return result;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            if (roleId == null)
            {
                return false;
            }
            var role = await _roleManager.Roles.FirstOrDefaultAsync(u => u.Id == roleId);
            if (role == null)
            {
                return false;
            }
            var res = await _roleManager.DeleteAsync(role);
            return res.Succeeded;
        }
    }
}
