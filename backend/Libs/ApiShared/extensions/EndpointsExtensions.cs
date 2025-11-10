using System.Reflection;
using ApiShared.endpoints_filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiShared.extensions;

public static class EndpointsExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly) {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpointGroup)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpointGroup), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static void MapEndpointGroups(this WebApplication app) =>
        app.Services
            .GetRequiredService<IEnumerable<IEndpointGroup>>()
            .ToList()
            .ForEach(e => e.MapEndpoints(app));

    public static RouteGroupBuilder WithGroupAuthenticationRequired(this RouteGroupBuilder builder) =>
        builder.AddEndpointFilter<AuthenticationRequiredEndpointFilter>();

    public static RouteHandlerBuilder WithRequestValidation<T>(
        this RouteHandlerBuilder builder
    ) where T : class, IRequestWithValidationNeeded =>
        builder.AddEndpointFilter<RequestValidationRequiredEndpointFilter<T>>();


    public static RouteHandlerBuilder WithAuthenticationRequired(
        this RouteHandlerBuilder builder
    ) =>
        builder.AddEndpointFilter<AuthenticationRequiredEndpointFilter>();
}