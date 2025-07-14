using System.Text.Json.Serialization;
using SharedKernel.auth;

namespace TagsService.Api;

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

    private static IServiceCollection AddFrontend(this IServiceCollection services, IConfiguration configuration) {
        var frontendUrl = configuration["FrontendUrl"] ?? throw new Exception("FrontendUrl is not configured");

        services.AddCors(options => {
            options.AddPolicy("AllowFrontend", policy => {
                policy
                    .WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}