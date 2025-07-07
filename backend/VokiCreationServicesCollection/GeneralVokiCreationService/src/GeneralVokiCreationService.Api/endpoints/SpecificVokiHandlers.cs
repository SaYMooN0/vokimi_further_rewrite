namespace GeneralVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapPatch("/set-cover-to-default", SetVokiCoverToDefault);
        // group.MapPatch("/update-cover", UpdateVokiCover)
        //     .WithRequestValidation<UpdateVokiCoverRequest>();
        // group.MapPatch("/update-details", UpdateVokiDetails)
        //     .WithRequestValidation<UpdateVokiDetailsRequest>();
    }
}