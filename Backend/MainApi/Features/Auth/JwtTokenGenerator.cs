using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatabaseServices.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Configuration;

namespace WinterExam24.Features.Auth;

public interface IJwtTokenGenerator
{
    public Task<string?> GenerateJwtTokenAsync(string username);

    public Task<bool> ValidateTokenAsync(string token);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtTokenSettings _jwtTokenSettings;
    private readonly IUserRepository _users;

    public JwtTokenGenerator(IOptions<JwtTokenSettings> jwtTokenSettingsConfig, IUserRepository users)
    {
        _users = users;
        _jwtTokenSettings = jwtTokenSettingsConfig.Value;
    }

    public async Task<string?> GenerateJwtTokenAsync(string username)
    {
        var user = await _users.FindByNameAsync(username);
        if (user is null) return null;
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: _jwtTokenSettings.Issuer,
            audience: _jwtTokenSettings.Audience,
            notBefore: now,
            claims: GetIdentity(user).Claims,
            expires: now + TimeSpan.FromMinutes(_jwtTokenSettings.Lifetime) + TimeSpan.FromDays(14),
            signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenSettings.Key)),
            SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtTokenSettings.Issuer,
            ValidAudience = _jwtTokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenSettings.Key))
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private static ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.UserName!),
            new("Id", user.Id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}