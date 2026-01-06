using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiShared.EfCore;

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

    public static void MapAllEndpointWithConsistency<T>(this WebApplication app) where T : DbContext{
        app.Services
            .GetRequiredService<IEnumerable<IEndpointGroup>>()
            .ToList()
            .ForEach(e => e
                .MapEndpoints(app)
                .AddEndpointFilter<EventualConsistencyEndpointFilter<T>>()
            );
    }
    public static RouteHandlerBuilder DisableConsistencyFilter(
        this RouteHandlerBuilder builder
    ) {
        return builder.WithMetadata(DisableConsistencyFilterMetadata.Instance);
    }
}