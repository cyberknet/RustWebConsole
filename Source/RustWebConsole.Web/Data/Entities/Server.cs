using System.Collections.Generic;
using RustWebConsole.Web.Data.Services;

namespace RustWebConsole.Web.Data.Entities
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty; 
        public int Port { get; set; } 
        public string Password { get; set; } = string.Empty; 

        // Relationships
        public ICollection<UserServer> UserServers { get; set; } = new List<UserServer>();
        public ICollection<ConsoleMessage> ConsoleMessages { get; set; } = new List<ConsoleMessage>();
    }
}