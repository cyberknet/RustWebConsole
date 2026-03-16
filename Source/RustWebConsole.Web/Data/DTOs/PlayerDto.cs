using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Data.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SteamId { get; set; } = string.Empty;
        public int ServerId { get; set; }
        public string? LastIpAddress { get; set; }
        public DateTime LastConnectedOn { get; set; }
        public TimeSpan ConnectionDuration { get; set; } = TimeSpan.Zero;
        public PlayerStatus Status { get; set; }
    }
}