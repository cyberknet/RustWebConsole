using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RustWebConsole.Web.Attributes;
using RustWebConsole.Web.Data.Services;
using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Data.Entities
{
    public class Server
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Hostname { get; set; } = string.Empty;

        [Required]
        public int Port { get; set; }

        [Required]
        [EncryptedField]
        public string Password { get; set; } = string.Empty;

        public ServerStatus Status { get; set; }

        [Required]
        public DateTime LastChecked { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? Group { get; set; }

        [Required]
        [MaxLength(500)]
        public string TagsSerialized { get; set; } = string.Empty;


        // Relationships
        public ICollection<UserServer> UserServers { get; set; } = new List<UserServer>();
        public ICollection<ConsoleMessage> ConsoleMessages { get; set; } = new List<ConsoleMessage>();
        public ICollection<ServerTag> ServerTags { get; set; } = new List<ServerTag>();
    }
}