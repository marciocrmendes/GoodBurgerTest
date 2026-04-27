using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Infrastructure.Options;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoodBurger.Api.Features.Auth;

public class LoginHandler(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions) : IDomainHandler<LoginRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> HandleAsync(LoginRequest request, CancellationToken stoppingToken = default)
    {
        var user = await userRepository.FindByUsernameAsync(request.Username.Trim(), stoppingToken);

        if (user is null || !user.VerifyPassword(request.Password))
            return Result.Failure<LoginResponse>(Error.Unauthorized("Usuário ou senha inválidos."));

        var token = GenerateToken(user.Username);
        return Result.Success(new LoginResponse(token, user.Username));
    }

    private string GenerateToken(string username)
    {
        var opts = jwtOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opts.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: opts.Issuer,
            audience: opts.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(opts.ExpiresInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
