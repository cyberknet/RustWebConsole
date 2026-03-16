using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RustWebConsole.Web.Data.Entities
{
    public class ConsoleMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServerId { get; set; }

        [ForeignKey(nameof(ServerId))]
        public Server Server { get; set; } = null!;

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., Info, Warning, Error
    }
}