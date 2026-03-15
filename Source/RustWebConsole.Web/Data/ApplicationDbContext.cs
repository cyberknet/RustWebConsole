using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RustWebConsole.Web.Data.Entities;

namespace RustWebConsole.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<UserServer> UserServers { get; set; } = null!;
        public DbSet<ServerStatistics> ServerStatistics { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; } = null!;
        public DbSet<ConsoleMessage> ConsoleMessages { get; set; } = null!;
        public DbSet<RconRequest> RconRequests { get; set; } = null!;
        public DbSet<RconResponse> RconResponses { get; set; } = null!;
        public DbSet<UserAction> UserActions { get; set; } = null!;
    }
}
