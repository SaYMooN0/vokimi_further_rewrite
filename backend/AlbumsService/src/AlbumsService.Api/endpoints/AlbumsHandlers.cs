using AlbumsService.Api.contracts;
using AlbumsService.Application.common.repositories;
using AlbumsService.Application.voki_albums.commands;
using AlbumsService.Application.voki_albums.queries;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;

namespace AlbumsService.Api.endpoints;

internal static class AlbumsHandlers
{
    internal static void MapAlbumsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/")
            .WithGroupAuthenticationRequired();

        group.MapGet("/all-albums-preview", GetAllUserAlbumsPreview);

        group.MapPost("/create-new", CreateNewAlbum)
            .WithRequestValidation<CreateNewVokiAlbumRequest>();
    }


    private static async Task<IResult> GetAllUserAlbumsPreview(
        CancellationToken ct, IQueryHandler<ListAllUserAlbumsPreviewQuery, ListAllUserAlbumsPreviewQueryResult> handler
    ) {
        ListAllUserAlbumsPreviewQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ListAllUserAlbumsPreviewQueryResult, AllAlbumsPreviewResponse>(result);
    }

    private static async Task<IResult> CreateNewAlbum(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<CreateNewAlbumCommand, VokiAlbum> handler
    ) {
        var request = httpContext.GetValidatedRequest<CreateNewVokiAlbumRequest>();

        CreateNewAlbumCommand command = new(
            request.ParsedName, request.ParsedIcon, request.ParsedMainColor, request.ParsedSecondaryColor
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiAlbum, AlbumCreatedResponse>(result);
    }
}