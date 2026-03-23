using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RustWebConsole.Web.Data;
using RustWebConsole.Web.Data.Entities;
using RustWebConsole.Web.Data.Enums;
using RustWebConsole.Web.Services.Authorization;
using RustWebConsole.Web.Services.Validation;
using System.Security.Claims;

namespace RustWebConsole.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ServerConnectionValidator _connectionValidator;

        public ServersController(ApplicationDbContext context, IAuthorizationService authorizationService, ServerConnectionValidator connectionValidator)
        {
            _context = context;
            _authorizationService = authorizationService;
            _connectionValidator = connectionValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var servers = isAdmin
                ? await _context.Servers.ToListAsync()
                : await _context.Servers
                    .Where(s => s.UserServers.Any(us => us.UserId == userId))
                    .ToListAsync();

            return servers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServer(int id)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, id, new ServerAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var server = await _context.Servers.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            return server;
        }

        [HttpPost]
        public async Task<ActionResult<Server>> CreateServer(Server server)
        {
            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServer), new { id = server.Id }, server);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServer(int id, Server server)
        {
            if (id != server.Id)
            {
                return BadRequest();
            }

            if (User.IsInRole(PermissionLevel.Viewer.ToString()))
            {
                return Forbid();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, id, new ServerAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            _context.Entry(server).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(int id)
        {
            if (User.IsInRole(PermissionLevel.Viewer.ToString()))
            {
                return Forbid();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, id, new ServerAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/validate-connection")]
        public async Task<IActionResult> ValidateServerConnection(int id)
        {
            var server = await _context.Servers.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            var isValid = await _connectionValidator.ValidateConnectionAsync(server.Hostname, server.Port, server.Password);

            if (!isValid)
            {
                return BadRequest("Unable to connect to the server with the provided credentials.");
            }

            return Ok("Connection successful.");
        }

        private bool ServerExists(int id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }
    }
}