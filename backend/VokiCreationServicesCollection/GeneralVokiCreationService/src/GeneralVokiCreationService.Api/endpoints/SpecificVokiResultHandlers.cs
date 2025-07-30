namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiResultsHandlers
{
    internal static void MapSpecificVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results/{resultId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapGet("/", GetVokiResultFullData);
        // group.MapPatch("/update-name", UpdateResultText)
        //     .WithRequestValidation<UpdateResultNameRequest>();
        // group.MapPatch("/update-text", UpdateResultText)
        //     .WithRequestValidation<UpdateResultTextRequest>();
        // group.MapPatch("/update-images", UpdateResultImages)
        //     .WithRequestValidation<UpdateResultImagesRequest>();
        // // group.MapDelete("/delete", DeleteVokiResult);
        //
        // group.MapPost("/upload-image", UploadVokiResultImage)
        //     .DisableAntiforgery();
    }

   
}