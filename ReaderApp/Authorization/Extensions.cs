using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReaderApp.Authorization
{
    public static class Extensions
    {
        public static Guid GetId(this ClaimsPrincipal user) => new Guid(user.FindFirstValue(JwtRegisteredClaimNames.Sub));
    }
}
