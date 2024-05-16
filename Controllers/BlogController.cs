using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.Dtos.Blog;
using api.Models;
using api.Data;
using api.Mappers;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDBContext _context;

        public BlogController(UserManager<AppUser> userManager,
                ApplicationDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        private async Task<AppUser?> GetCurrentUserAsync()
        {
            var username = User.FindFirst(ClaimTypes.GivenName)?.Value!;
            return await _userManager.FindByNameAsync(username);
        }

        [HttpGet("user/all")]
        [Authorize]
        public async Task<IActionResult> GetAllBlogs()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            var blogs = await _context.Blogs.Where(blog => blog.UserId == user.Id).ToListAsync();
            var blogDtos = blogs.Select(blog => blog.BlogToGetBlogDto());
            return Ok(blogDtos);
        }

        [HttpGet("{blogId:int}")]
        [Authorize]
        public async Task<IActionResult> GetBlogById([FromRoute] int blogId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            var blog = await _context.Blogs.FirstAsync(blog => blog.UserId == user.Id && blog.Id == blogId);
            return Ok(blog.BlogToGetBlogDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBlog([FromBody] CreateBlogDto createBlogDto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            var newBlog = new Blog
            {
                UserId = user.Id,
                Title = createBlogDto.Title,
                Content = createBlogDto.Content,
                User = user,
            };

            user.Blogs.Add(newBlog);
            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("user/{blogId:int}")]
        [Authorize]
        public async Task<IActionResult> UserDeleteBlog([FromRoute] int blogId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            var blog = await _context.Blogs
                .FirstOrDefaultAsync(b => b.UserId == user.Id && b.Id == blogId);
            if (blog == null)
            {
                return NotFound("Blog not found");
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok(blog.BlogToGetBlogDto());
        }


        [HttpDelete("admin/")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDeleteBlog([FromBody] AdminDeleteBlogDto adminDeleteBlogDto)
        {
            var user = await _userManager.FindByNameAsync(adminDeleteBlogDto.UserName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var blog = await _context.Blogs
                .FirstOrDefaultAsync(b => b.UserId == user.Id && b.Id == adminDeleteBlogDto.BlogId);
            if (blog == null)
            {
                return NotFound("Blog not found");
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
