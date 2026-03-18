using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.ViewModels
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}