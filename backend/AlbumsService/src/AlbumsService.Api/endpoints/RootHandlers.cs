using AlbumsService.Api.contracts;
using AlbumsService.Api.contracts.create_new_album;
using AlbumsService.Application.app_users.queries;
using AlbumsService.Application.voki_albums.commands;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;

namespace AlbumsService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/")
            .WithGroupAuthenticationRequired();

        group.MapGet("/all-albums-preview", GetAllUserAlbumsPreview);

        group.MapPost("/albums/create-new", CreateNewAlbum)
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