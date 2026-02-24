using System.ComponentModel.DataAnnotations;

namespace WorkMosm.FrontEnd.Models
{
    public record LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
