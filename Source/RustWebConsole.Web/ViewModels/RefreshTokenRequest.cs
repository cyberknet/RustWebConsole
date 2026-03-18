using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.ViewModels
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}