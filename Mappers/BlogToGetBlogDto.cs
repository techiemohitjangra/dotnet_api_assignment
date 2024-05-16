using api.Dtos.Blog;
using api.Models;

namespace api.Mappers
{
    public static class BlogToDto
    {
        public static GetBlogDto BlogToGetBlogDto(this Blog blog)
        {
            return new GetBlogDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.User.UserName!,
            };
        }
    }
}
