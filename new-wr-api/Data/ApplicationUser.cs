using Microsoft.AspNetCore.Identity;

namespace new_wr_api.Data
{
    public partial class ApplicationUser: IdentityUser
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public DateTime PasswordUpdatedOn { get; set; } = DateTime.UtcNow;
        public string UserType { get; set; } = string.Empty;
    }
}
