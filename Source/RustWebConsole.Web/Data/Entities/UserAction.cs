using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Data.Entities
{
    public class UserAction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public ActionType ActionType { get; set; }

        [Required]
        public string Target { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; }

        public string? Details { get; set; } // Optional field for additional information
    }
}