using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReaderApp.Commands.Infrastructure
{
    public class BaseRequest
    {
        public ClaimsPrincipal User { get; set; }

        public Guid UserId => new Guid(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
    }
}
