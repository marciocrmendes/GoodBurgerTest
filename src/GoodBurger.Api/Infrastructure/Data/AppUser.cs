using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace GoodBurger.Api.Infrastructure.Data
{
    public sealed class AppUser(IHttpContextAccessor httpContextAccessor)
    {
        private Guid _userId;
        private string? _userName;

        public Guid UserId
        {
            get
            {
                if (_userId != Guid.Empty) return _userId;

                var userId = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                _ = Guid.TryParse(userId, out var id);

                return _userId = id;
            }
        }

        public string UserName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_userName)
                    ? _userName
                    : (_userName = httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirst(ClaimTypes.Name)?.Value ?? string.Empty);
            }
        }
    }
}
