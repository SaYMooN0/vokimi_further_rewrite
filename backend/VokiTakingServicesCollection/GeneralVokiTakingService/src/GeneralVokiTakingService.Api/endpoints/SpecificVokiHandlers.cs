using GeneralVokiTakingService.Api.contracts;
using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        group.MapPost("/start-taking", StartVokiTaking);
    }

    private static async Task<IResult> StartVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        StartVokiTakingCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (vokiTakingData) => Results.Json(
            GeneralVokiTakingResponse.Create(vokiTakingData)
        ));
    }
}