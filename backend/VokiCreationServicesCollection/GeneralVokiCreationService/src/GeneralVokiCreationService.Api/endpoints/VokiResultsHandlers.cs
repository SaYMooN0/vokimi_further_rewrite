namespace GeneralVokiCreationService.Api.endpoints;

internal static class VokiResultsHandlers
{
    internal static void MapVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results/");

        group.WithGroupAuthenticationRequired();

        // group.MapPost("/add-new", AddNewResultToVoki)
        //     .WithRequestValidation<AddNewResultToVokiRequest>();
    }

  
}