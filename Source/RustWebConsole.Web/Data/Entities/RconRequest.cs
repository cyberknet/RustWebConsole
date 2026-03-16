using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RustWebConsole.Web.Data.Entities
{
    public class RconRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int ServerId { get; set; }

        [ForeignKey(nameof(ServerId))]
        [Required]
        public Server Server { get; set; } = null!;

        [Required]
        public string Command { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; }
    }
}