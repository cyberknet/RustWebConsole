using System.Collections.Generic;

namespace RustWebConsole.Web.Data.Entities
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ConnectionDetails { get; set; } = string.Empty;
        public string EncryptedCredentials { get; set; } = string.Empty;

        // Relationships
        public ICollection<UserServer> UserServers { get; set; } = new List<UserServer>();
        public ICollection<ConsoleMessage> ConsoleMessages { get; set; } = new List<ConsoleMessage>();
    }
}