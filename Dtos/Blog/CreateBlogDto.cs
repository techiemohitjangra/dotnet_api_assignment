using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Blog
{
    public class CreateBlogDto
    {
        [Required]
        public required string Title { get; set; } = string.Empty;
        [Required]
        public required string Content { get; set; } = string.Empty;
    }
}
