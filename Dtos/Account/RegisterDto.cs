using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public required string Password { get; set; }
    }
}
