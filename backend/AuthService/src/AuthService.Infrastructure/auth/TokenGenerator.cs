using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.abstractions;
using AuthService.Domain.app_user_aggregate;
using InfrastructureShared.auth;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.auth;

namespace AuthService.Infrastructure.auth;

internal sealed class TokenGenerator : ITokenGenerator
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenGenerator(JwtTokenConfig options) {
        _secretKey = options.SecretKey;
        _issuer = options.Issuer;
        _audience = options.Audience;
    }


    public JwtTokenString CreateToken(AppUser user) {
        Claim[] claims = [new(TokenParser.UserIdClaim, user.Id.ToString())];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtTokenString(tokenString);
    }
}