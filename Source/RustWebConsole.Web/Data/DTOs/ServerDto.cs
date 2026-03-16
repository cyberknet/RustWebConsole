namespace RustWebConsole.Web.Data.DTOs
{
    public class ServerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ConnectionDetails { get; set; } = string.Empty;
    }
}