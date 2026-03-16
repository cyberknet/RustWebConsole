using AutoMapper.Execution;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RustWebConsole.Web.Attributes;
using RustWebConsole.Web.Data.Entities;
using RustWebConsole.Web.Data.Services;

namespace RustWebConsole.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IEncryptionService _encryptionService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IEncryptionService encryptionService)
            : base(options)
        {
            _encryptionService = encryptionService;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var encryptedConverter = new ValueConverter<string, string>(
                    v => _encryptionService.Encrypt(v), // Encrypt on save
                    v => _encryptionService.Decrypt(v)  // Decrypt on load
                );

            // loop through all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ProcessEncryptedFieldAttribute(modelBuilder, entityType, encryptedConverter);
            }
        }

        private static void ProcessEncryptedFieldAttribute(ModelBuilder modelBuilder, Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType, ValueConverter<string, string> encryptedConverter)
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                if (Attribute.GetCustomAttribute(property, typeof(EncryptedFieldAttribute)) is EncryptedFieldAttribute)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(encryptedConverter);
                }
            }
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
