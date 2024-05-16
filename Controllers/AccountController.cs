using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using api.Dtos.Account;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
                JwtService jwtService,
                SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return Ok(
                    new GetAccountDto
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = roles.ToList(),
                    });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users
                            .FirstOrDefaultAsync(user => user.UserName == loginDto.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }
            // false here disables lock out on failure
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Username and/or password is incorrect");
            }
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return Ok(
                    new GetAccountDto
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Role = roles.ToList(),
                        Token = _jwtService.GenerateJwtToken(user, roles.ToList()),
                    });
        }


        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        IList<string> roles = await _userManager.GetRolesAsync(appUser);
                        return Ok(
                                new GetAccountDto
                                {
                                    UserName = appUser.UserName,
                                    Email = appUser.Email,
                                    Role = roles.ToList(),
                                    Token = _jwtService.GenerateJwtToken(appUser, roles.ToList()),
                                }
                                );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }


        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        IList<string> roles = await _userManager.GetRolesAsync(appUser);
                        return Ok(
                                new GetAccountDto
                                {
                                    UserName = appUser.UserName,
                                    Email = appUser.Email,
                                    Role = roles.ToList(),
                                    Token = _jwtService.GenerateJwtToken(appUser, roles.ToList()),
                                }
                                );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
