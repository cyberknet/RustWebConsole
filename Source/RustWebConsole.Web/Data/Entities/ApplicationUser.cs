using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RustWebConsole.Web.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; } // Example of additional profile data

        // Navigation property for UserServer relationship
        public ICollection<UserServer> UserServers { get; set; } = new List<UserServer>();
    }
}