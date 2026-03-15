using Microsoft.AspNetCore.Identity;

namespace RustWebConsole.Web.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; } // Example of additional profile data
    }
}