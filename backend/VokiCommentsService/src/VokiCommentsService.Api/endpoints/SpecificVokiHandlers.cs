namespace VokiCommentsService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        // group.MapGet("/comments", GetVokiComments);

        // group.MapPost("/comment", CommentVoki)
        //     .WithAuthenticationRequired()
        //     .WithRequestValidation<CommentVokiRequest>();
    }


    // private static async Task<IResult> CommentVoki(
    //     HttpContext httpContext, CancellationToken ct, ICommandHandler<CommentVokiCommand, > handler
    // ) {
    //     var request = httpContext.GetValidatedRequest<CommentVokiRequest>();
    //     VokiId vokiId = httpContext.GetVokiIdFromRoute();
    //
    //     CommentVokiCommand command = new(vokiId, request.RatingValue);
    //     var result = await handler.Handle(command, ct);
    //
    //     return CustomResults.FromErrOrToJson<>(result);
    // }
}