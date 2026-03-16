namespace RustWebConsole.Web.Data.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SteamId { get; set; } = string.Empty;
        public int ServerId { get; set; }
    }
}