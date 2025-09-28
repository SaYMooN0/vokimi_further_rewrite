
namespace VokiRatingsService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");
        
        // group.MapGet("/rated-vokis", GetUserRatedVokis)
        //     .WithAuthenticationRequired();
    }

}