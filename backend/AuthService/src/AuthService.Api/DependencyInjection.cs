using System.Text.Json.Serialization;
using ApiShared;
using AuthService.Infrastructure;
using SharedKernel.auth;

namespace AuthService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration) {
        services.AddFrontend(configuration);
        services.AddOpenApi();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContextProvider>();
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }
    
    private static IServiceCollection AddFrontend(
        this IServiceCollection services, IConfiguration configuration
    ) {
        var frontendConfig = configuration.GetSection("FrontendConfig").Get<FrontendConfig>();
        if (frontendConfig is null) {
            throw new Exception("Email service is not configured");
        }
        services.AddSingleton(frontendConfig);
        
        
        services.AddCors(options => {
            options.AddPolicy("AllowFrontend", policy => {
                policy
                    .WithOrigins(frontendConfig.BaseUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}