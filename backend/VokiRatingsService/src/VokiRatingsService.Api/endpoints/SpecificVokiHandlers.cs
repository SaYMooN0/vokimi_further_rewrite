using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;
using SharedKernel.domain.ids;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.voki_ratings.commands;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        // group.MapGet("/ratings", GetVokiRatingsData);
        // group.MapGet("/average", GetVokiAverageRating);

        group.MapPost("/rate", RateVoki)
            .WithAuthenticationRequired()
            .WithRequestValidation<RateVokiRequest>();
    }

    private static async Task<IResult> RateVoki(
        HttpContext httpContext, CancellationToken ct, ICommandHandler<RateVokiCommand, RatingValueWithDate> handler
    ) {
        var request = httpContext.GetValidatedRequest<RateVokiRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        RateVokiCommand command = new(vokiId, request.RatingValue);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<RatingValueWithDate, VokiRatedResponse>(result);
    }
}