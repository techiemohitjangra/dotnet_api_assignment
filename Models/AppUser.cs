using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
