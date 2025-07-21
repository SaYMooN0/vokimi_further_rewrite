namespace UserProfilesService.Api.endpoints;

internal static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");
    }

}