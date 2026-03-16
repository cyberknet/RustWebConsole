using Microsoft.AspNetCore.Identity;
using RustWebConsole.Web.Data.Entities;

namespace RustWebConsole.Web.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created
            await dbContext.Database.EnsureCreatedAsync();

            // Seed admin user
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, adminPassword);
            }

            // Seed sample server
            if (!dbContext.Servers.Any())
            {
                dbContext.Servers.Add(new Server
                {
                    Name = "Sample Server",
                    ConnectionDetails = "127.0.0.1:28015",
                    EncryptedCredentials = "sample-encrypted-credentials"
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}