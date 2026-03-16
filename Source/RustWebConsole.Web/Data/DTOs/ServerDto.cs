namespace RustWebConsole.Web.Data.DTOs
{
    public class ServerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}