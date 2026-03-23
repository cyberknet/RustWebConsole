using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RustWebConsole.Web.Data;
using RustWebConsole.Web.Data.Entities;
using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Services
{
    /// <summary>
    /// Background service for monitoring the status of servers.
    /// </summary>
    public class ServerStatusMonitoringService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ServerStatusMonitoringService> _logger;

        public ServerStatusMonitoringService(IServiceProvider serviceProvider, ILogger<ServerStatusMonitoringService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Server Status Monitoring Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var servers = dbContext.Servers;

                        foreach (var server in servers)
                        {
                            // Simulate a server status check (replace with actual logic)
                            server.Status = CheckServerStatus(server) ? ServerStatus.Online : ServerStatus.Offline;
                            server.LastChecked = DateTime.UtcNow;
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while monitoring server statuses.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust interval as needed
            }

            _logger.LogInformation("Server Status Monitoring Service is stopping.");
        }

        private bool CheckServerStatus(Server server)
        {
            // Placeholder logic for server status check
            // Replace with actual RCON or network check logic
            return true;
        }
    }
}