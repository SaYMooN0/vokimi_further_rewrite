namespace GeneralVokiCreationService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");

        group.WithGroupAuthenticationRequired();

        // group.MapGet("/default-voki-details", GetDefaultVokiDetails);
    }

}