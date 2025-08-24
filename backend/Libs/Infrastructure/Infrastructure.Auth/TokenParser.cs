using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.auth;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace Infrastructure.Auth;

public class TokenParser : ITokenParser
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    private readonly ILogger<TokenParser> _logger;

    public TokenParser(JwtTokenConfig options, ILogger<TokenParser> logger) {
        _logger = logger;
        _secretKey = options.SecretKey;
        _issuer = options.Issuer;
        _audience = options.Audience;
    }

    public const string UserIdClaim = "UserId";

    private static readonly Err AuthErr = ErrFactory.AuthRequired(
        "User is not authenticated", details: "Log in to your account"
    );

    public ErrOr<AppUserId> UserIdFromJwtToken(JwtTokenString token) {
        TokenValidationParameters tokenValidationParameters = new() {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
        };

        try {
            var principal = JwtSecurityTokenHandler.ValidateToken(token.ToString(), tokenValidationParameters, out _);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == UserIdClaim)?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) {
                return AuthErr;
            }

            return new AppUserId(Guid.Parse(userIdClaim));
        }
        catch (Exception ex) {
            _logger.LogError("Couldn't parse jwt token. Ex: {exception}", ex.Message);
            return AuthErr;
        }
    }
}