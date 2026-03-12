using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace RealEstateApp.WebUI
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;

            // Önce listeye al, sonra ekle
            var roleClaims = principal.FindAll("https://RealEstateApp.com/roles").ToList();

            foreach (var claim in roleClaims)
            {
                identity?.AddClaim(new Claim(ClaimTypes.Role, claim.Value));
            }

            return Task.FromResult(principal);
        }
    }
}
