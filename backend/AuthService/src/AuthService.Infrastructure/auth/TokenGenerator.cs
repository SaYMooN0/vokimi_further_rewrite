using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Application.abstractions;
using AuthService.Domain.app_user_aggregate;
using InfrastructureShared.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.auth;

internal sealed class TokenGenerator : ITokenGenerator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly RsaSecurityKey _privateKey;
    private readonly ILogger<TokenGenerator> _logger;

    public TokenGenerator(JwtTokenConfig options, AuthPrivateKeyConfig privateKeyConfig, ILogger<TokenGenerator> logger) {
        _issuer = options.Issuer;
        _audience = options.Audience;

        RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyConfig.PrivateKey);
        _privateKey = new RsaSecurityKey(rsa);

        _logger = logger;
    }


    public JwtTokenString CreateToken(AppUser user) {
        try {
            Claim[] claims = [
                new(ITokenParser.UserIdClaim, user.Id.ToString())
            ];

            SigningCredentials creds =
                new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256);

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
        catch (Exception ex) {
            _logger.LogError(
                ex,
                "Failed to generate JWT token for userId '{userId}'. Error: {errorMessage}",
                user.Id.ToString(),
                ex.Message
            );

            throw;
        }
    }
}