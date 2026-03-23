using RustWebConsole.Web.Data;
using RustWebConsole.Web.Data.Entities;
using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Services
{
    public interface IUserActionLoggingService
    {
        Task LogActionAsync(string userId, ActionType actionType, string target, string? details = null);
    }

    public class UserActionLoggingService : IUserActionLoggingService
    {
        private readonly ApplicationDbContext _context;

        public UserActionLoggingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string userId, ActionType actionType, string target, string? details = null)
        {
            var userAction = new UserAction
            {
                UserId = userId,
                ActionType = actionType,
                Target = target,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            _context.UserActions.Add(userAction);
            await _context.SaveChangesAsync();
        }
    }
}