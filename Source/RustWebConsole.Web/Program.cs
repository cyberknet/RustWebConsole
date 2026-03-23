using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RustWebConsole.Web.Components;
using RustWebConsole.Web.Components.Account;
using RustWebConsole.Web.Data;
using RustWebConsole.Web.Data.Entities;
using RustWebConsole.Web.Data.Services;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RustWebConsole.Web;
using RustWebConsole.Web.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using RustWebConsole.Web.Middleware;
using RustWebConsole.Web.Services; // Added namespace for UserActionLoggingService
using Microsoft.AspNetCore.DataProtection; // Added for Data Protection

var builder = WebApplication.CreateBuilder(args);
Dictionary<string, string?> overrides = AppSettings.EnvironmentMap.ToDictionary(
    kvp => kvp.Key,
    kvp => Environment.GetEnvironmentVariable(kvp.Value))
    .Where(x => x.Value != null)
    .ToDictionary(x => x.Key, x => x.Value);

builder.Configuration.AddInMemoryCollection(overrides);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Add a strongly-typed configuration object for AppSettings
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

// Register the AppSettings object as a singleton
builder.Services.AddSingleton(appSettings);

var connectionString = appSettings.ConnectionStrings.DefaultConnection ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = appSettings.Jwt.ValidIssuer,
        ValidAudience = appSettings.Jwt.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.IssuerSigningKey))
    };
    options.Authority = appSettings.Jwt.ValidIssuer;
    options.RequireHttpsMetadata = true;
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Register the EncryptionService
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

// Add role-based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("ViewerOnly", policy => policy.RequireRole("Viewer"));
    options.AddPolicy("ServerAccess", policy =>
        policy.Requirements.Add(new ServerAccessRequirement()));
    options.AddPolicy("ServerLevelAccess", policy =>
        policy.Requirements.Add(new ServerLevelAccessRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, ServerAccessHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ServerLevelAccessHandler>();

// Add Identity services with appSettings configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = appSettings.Identity.PasswordRequireDigit;
    options.Password.RequiredLength = appSettings.Identity.PasswordRequiredLength;
    options.Password.RequireNonAlphanumeric = appSettings.Identity.PasswordRequireNonAlphanumeric;
    options.Password.RequireUppercase = appSettings.Identity.PasswordRequireUppercase;
    options.Password.RequireLowercase = appSettings.Identity.PasswordRequireLowercase;
    options.SignIn.RequireConfirmedAccount = appSettings.Identity.RequireConfirmedAccount;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserActionLoggingService, UserActionLoggingService>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(appSettings.DataProtection.KeyPath))
    .SetApplicationName("RustWebConsole");

// Register the ServerStatusMonitoringService
builder.Services.AddHostedService<ServerStatusMonitoringService>();

var app = builder.Build();

// Add middleware for permission checking
app.UseMiddleware<PermissionCheckingMiddleware>();

// Apply EF Core migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Seed initial data
await ApplicationDbContextSeed.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
