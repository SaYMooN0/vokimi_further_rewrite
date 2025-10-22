using AlbumsService.Api.contracts;
using AlbumsService.Application.voki_albums.commands;
using AlbumsService.Application.voki_albums.queries;
using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;
using SharedKernel.domain.ids;

namespace AlbumsService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/albums-data", GetAlbumsDataForVoki);
        group.MapPatch("/update-presence-in-albums", UpdateVokiPresenceInAlbums)
            .WithRequestValidation<UpdateVokiPresenceInAlbumsRequest>();
    }

    private static async Task<IResult> GetAlbumsDataForVoki(
        IQueryHandler<ListAlbumsDataForVokiQuery, AlbumWithVokiPresenceDto[]> handler,
        HttpContext httpContext,
        CancellationToken ct
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ListAlbumsDataForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<AlbumWithVokiPresenceDto[], ListAlbumsDataForVokiResponse>(result);
    }

    private static async Task<IResult> UpdateVokiPresenceInAlbums(
        ICommandHandler<UpdateVokiPresenceInAlbumsCommand, AlbumWithVokiPresenceDto[]> handler,
        HttpContext httpContext,
        CancellationToken ct
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiPresenceInAlbumsRequest>();

        UpdateVokiPresenceInAlbumsCommand command = new(vokiId, request.ParsedAlbumIdToEntry);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<AlbumWithVokiPresenceDto[], ListAlbumsDataForVokiResponse>(result);
    }
}