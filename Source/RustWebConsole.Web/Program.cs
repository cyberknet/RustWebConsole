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

var builder = WebApplication.CreateBuilder(args);

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

T GetConfigValue<T>(IConfiguration configuration, PasswordOptions? passwordOptions, string propertyName, string envVar, T defaultValue) where T : struct
{
    var envValue = Environment.GetEnvironmentVariable(envVar);
    if (!string.IsNullOrEmpty(envValue))
    {
        if (typeof(T) == typeof(bool) && bool.TryParse(envValue, out var boolResult))
            return (T)(object)boolResult;

        if (typeof(T) == typeof(int) && int.TryParse(envValue, out var intResult))
            return (T)(object)intResult;
    }
    // no environment variable, try to get from JSON configuration
    else if (passwordOptions != null)
    {
        // use reflection to load the property value from the passwordOptions object
        var propertyInfo = typeof(PasswordOptions).GetProperty(propertyName);
        if (propertyInfo != null)
        {
            var jsonValue = propertyInfo.GetValue(passwordOptions);
            if (jsonValue != null && jsonValue is T value)
                return value;
        }
    }
    
    return defaultValue;
}

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;

    // Configure password policies using the helper function
    var passwordOptions = builder.Configuration.GetSection("Identity:Password").Get<PasswordOptions>();
    options.Password.RequireDigit = GetConfigValue(builder.Configuration, passwordOptions, nameof(PasswordOptions.RequireDigit), "RWC_PASSWORD_REQUIREDIGIT", true);
    options.Password.RequiredLength = GetConfigValue(builder.Configuration, passwordOptions,  nameof(PasswordOptions.RequiredLength), "RWC_PASSWORD_REQUIREDLENGTH", 8);
    options.Password.RequireNonAlphanumeric = GetConfigValue(builder.Configuration, passwordOptions, nameof(PasswordOptions.RequireNonAlphanumeric), "RWC_PASSWORD_REQUIRENONALPHANUMERIC", true);
    options.Password.RequireUppercase = GetConfigValue(builder.Configuration, passwordOptions, nameof(PasswordOptions.RequireUppercase), "RWC_PASSWORD_REQUIREUPPERCASE",  true);
    options.Password.RequireLowercase = GetConfigValue(builder.Configuration, passwordOptions, nameof(PasswordOptions.RequireLowercase), "RWC_PASSWORD_REQUIRELOWERCASE", true);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Register the EncryptionService
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");



var app = builder.Build();

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
