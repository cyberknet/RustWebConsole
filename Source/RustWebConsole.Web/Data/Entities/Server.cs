namespace RustWebConsole.Web.Data.Entities
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ConnectionDetails { get; set; } = string.Empty;
        public string EncryptedCredentials { get; set; } = string.Empty;
    }
}