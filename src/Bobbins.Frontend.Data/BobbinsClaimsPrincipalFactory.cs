using System.Security.Claims;
using System.Threading.Tasks;
using Bobbins.Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Bobbins.Frontend.Data
{
    public class BobbinsClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public BobbinsClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (!string.IsNullOrWhiteSpace(user.ScreenName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(BobbinsClaimTypes.ScreenName, user.ScreenName));
            }
            return principal;
        }
    }
}