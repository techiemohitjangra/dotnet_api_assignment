using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Blog
{
    public class AdminDeleteBlogDto
    {
        [Required]
        public required string UserName { get; set; } = string.Empty;
        [Required]
        public required int BlogId { get; set; }
    }
}
