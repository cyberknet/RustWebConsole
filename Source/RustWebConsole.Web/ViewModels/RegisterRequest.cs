using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.ViewModels
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}