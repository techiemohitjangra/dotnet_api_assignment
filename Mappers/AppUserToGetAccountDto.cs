using api.Dtos.Account;
using Microsoft.AspNetCore.Identity;
using api.Models;

namespace api.Mappers
{
    public static class AppUserToGetAccountDto
    {
        public static async Task<GetAccountDto> UserToGetAccountDto(this AppUser appUser, string token, UserManager<AppUser> userManager)
        {
            IList<string> roles = await userManager.GetRolesAsync(appUser);
            return new GetAccountDto
            {
                UserName = appUser.UserName!,
                Email = appUser.Email!,
                Role = roles.ToList()!,
                Token = token,
            };
        }
    }
}
