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

        group.MapGet("/user-albums", GetUserAlbumsList);
        group.MapGet("/all-albums-preview", GetAllUserAlbumsPreview);

        group.MapPost("/create-new", CreateNewAlbum)
            .WithRequestValidation<CreateNewVokiAlbumRequest>();

        // group.MapPatch("/update-voki-entries", UpdateVokiEntriesInAlbums)
        //     .WithRequestValidation<UpdateVokiEntriesInAlbumsRequest>();
    }

    private static async Task<IResult> GetUserAlbumsList(
        CancellationToken ct, IQueryHandler<ListUserAlbumsSortedQuery, VokiAlbum[]> handler
    ) {
        ListUserAlbumsSortedQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (albums) => Results.Json(
            UserAlbumsListResponse.Create(albums)
        ));
    }

    private static async Task<IResult> GetAllUserAlbumsPreview(
        CancellationToken ct, IQueryHandler<GetAllUserAlbumsPreviewQuery, GetAllUserAlbumsPreviewQueryResult> handler
    ) {
        GetAllUserAlbumsPreviewQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<GetAllUserAlbumsPreviewQueryResult, AllAlbumsPreviewResponse>(result);
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

        return CustomResults.FromErrOrToJson<VokiAlbum, AlbumDataResponse>(result);
    }
}