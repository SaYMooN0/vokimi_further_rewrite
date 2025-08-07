namespace VokisCatalogService.Api.endpoints;

internal static class VokiHandlers
{
    internal static void MapVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/");


        // group.MapGet("/", GetVokisRecommendations);
    }
}