using AlbumsService.Api.contracts;
using AlbumsService.Api.contracts.create_new_album;
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
        var group = routeBuilder.MapGroup("/albums/{albumId}")
            .WithGroupAuthenticationRequired();

        group.MapGet("/", GetAlbum);

        group.MapPatch("/update", UpdateAlbum)
            .WithRequestValidation<SaveVokiAlbumRequest>();

        group.MapDelete("/delete", DeleteAlbum);
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
}