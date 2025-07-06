using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.commands;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;


public static class VokisHandlers
{
    internal static void MapVokisHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/draft-vokis/");


        group.WithGroupAuthenticationRequired();

       
        // group.MapGet("/list-brief-info", ListVokisBriefData)
        //     .WithRequestValidation<ListVokisBriefInfoRequest>();
    }

    // private static async Task<IResult> ListVokisBriefData(
    //     HttpContext httpContext, CancellationToken ct,
    //     IQueryHandler<ListVokisQuery, ImmutableArray<DraftVoki>> handler
    // ) {
    //     var request = httpContext.GetValidatedRequest<InitializeNewVokiRequest>();
    //
    //     InitializeNewVokiCommand command = new(request.VokiType, request.ParseVokiName);
    //     var result = await handler.Handle(command, ct);
    //
    //     return CustomResults.FromErrOr(result,
    //         (voki) => CustomResults.Created(new {
    //             Id = voki.Id.ToString(),
    //             Type = voki.Type
    //         })
    //     );
    // }
}