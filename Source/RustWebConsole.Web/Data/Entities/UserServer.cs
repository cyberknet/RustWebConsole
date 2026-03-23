using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RustWebConsole.Web.Data.Entities
{
    [Index(nameof(UserId), nameof(ServerId), IsUnique = true)]
    public class UserServer
    {
        [Key]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Key]
        public int ServerId { get; set; }

        [ForeignKey(nameof(ServerId))]
        public Server Server { get; set; } = null!;

        // No additional relationships needed as this is a join table
    }
}