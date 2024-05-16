using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class GetAccountDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required List<string> Role { get; set; }

        public string? Token { get; set; }
    }
}
