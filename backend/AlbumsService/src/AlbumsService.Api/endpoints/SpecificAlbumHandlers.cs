using AlbumsService.Api.contracts;
using AlbumsService.Api.contracts.copy_vokis_from_albums_to_album;
using AlbumsService.Api.extensions;
using AlbumsService.Application.app_users.commands;
using AlbumsService.Application.voki_albums.commands;
using AlbumsService.Application.voki_albums.queries;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;

namespace AlbumsService.Api.endpoints;

internal class SpecificAlbumHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/albums/{albumId}");

        group.MapGet("/", GetAlbum);

        group.MapPatch("/update", UpdateAlbum)
            .WithRequestValidation<SaveVokiAlbumRequest>();

        group.MapDelete("/delete", DeleteAlbum);

        group.MapPatch("/copy-vokis-from-albums", CopyVokisFromAlbumsToAlbum)
            .WithRequestValidation<CopyVokisFromAlbumsToAlbumRequest>();
    }

    private static async Task<IResult> GetAlbum(
        HttpContext httpContext, CancellationToken ct, IQueryHandler<GetVokiAlbumToViewQuery, VokiAlbum> handler
    ) {
        var albumId = httpContext.GetAlbumIdFromRoute();

        GetVokiAlbumToViewQuery query = new(albumId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiAlbum, AlbumWithVokiIdsResponse>(result);
    }

    private static async Task<IResult> UpdateAlbum(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateAlbumCommand, VokiAlbum> handler
    ) {
        var albumId = httpContext.GetAlbumIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveVokiAlbumRequest>();

        UpdateAlbumCommand command = new(
            albumId, request.ParsedName, request.ParsedIcon,
            request.ParsedMainColor, request.ParsedSecondaryColor
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiAlbum, VokiAlbumPreviewResponse>(result);
    }

    private static async Task<IResult> DeleteAlbum(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DeleteAlbumCommand> handler
    ) {
        var albumId = httpContext.GetAlbumIdFromRoute();

        DeleteAlbumCommand command = new(albumId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, CustomResults.Deleted);
    }

    private static async Task<IResult> CopyVokisFromAlbumsToAlbum(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<CopyVokisFromAlbumsToAlbumCommand, int> handler
    ) {
        var albumId = httpContext.GetAlbumIdFromRoute();
        var request = httpContext.GetValidatedRequest<CopyVokisFromAlbumsToAlbumRequest>();

        CopyVokisFromAlbumsToAlbumCommand command = new(albumId, request.ParsedAlbumIds);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result,
            (vokisAdded) => Results.Json(new { VokisAdded = vokisAdded })
        );
    }
}