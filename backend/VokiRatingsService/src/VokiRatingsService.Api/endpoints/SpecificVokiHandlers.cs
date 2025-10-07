using ApiShared.extensions;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Api.contracts.rate_voki;
using VokiRatingsService.Application.voki_ratings.commands;
using VokiRatingsService.Application.voki_ratings.queries;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/ratings", GetVokiRatingsData);
        group.MapGet("/all-with-average", GetVokiOtherUsersRatingsWithAverage);

        group.MapPatch("/rate", RateVoki)
            .WithAuthenticationRequired()
            .WithRequestValidation<RateVokiRequest>();
    }

    private static async Task<IResult> GetVokiRatingsData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        UserRatingsDataForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserRatingsDataForVokiQueryResult, UserRatingsDataForVokiResponse>(result);
    }

    private static async Task<IResult> GetVokiOtherUsersRatingsWithAverage(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListVokiRatingsForVokiQuery, VokiRating[]> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ListVokiRatingsForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiRating[], RatingsWithAverageResponse>(result);
    }

    private static async Task<IResult> RateVoki(
        HttpContext httpContext, CancellationToken ct, ICommandHandler<RateVokiCommand, VokiRating> handler
    ) {
        var request = httpContext.GetValidatedRequest<RateVokiRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        RateVokiCommand command = new(vokiId, request.RatingValue);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiRating, VokiRatingDataResponse>(result);
    }
}