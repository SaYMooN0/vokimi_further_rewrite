﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.auth;

namespace Infrastructure.Auth;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        var jwtTokenConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
        if (jwtTokenConfig is null) {
            throw new Exception("JWT token config not configured");
        }

        services.AddSingleton(jwtTokenConfig);
        services.AddScoped<ITokenParser, TokenParser>();
        return services;
    }
}