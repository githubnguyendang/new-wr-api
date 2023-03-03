using Microsoft.AspNetCore.Identity;

namespace new_wr_api.Data
{
    public class ApplicationRole: IdentityRole
    {
        public bool IsDefault { get; set; }
        public string Discriminator { get; set; } = "ApplicationRole";
        
    }
}
