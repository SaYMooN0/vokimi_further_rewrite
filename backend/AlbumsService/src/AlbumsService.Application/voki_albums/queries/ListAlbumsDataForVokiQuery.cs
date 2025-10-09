﻿using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record ListAlbumsDataForVokiQuery(VokiId VokiId) : IQuery<AlbumWithVokiPresenceDto[]>;

internal sealed class ListAlbumsDataForVokiQueryHandler
    : IQueryHandler<ListAlbumsDataForVokiQuery, AlbumWithVokiPresenceDto[]>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserContext _userContext;

    public ListAlbumsDataForVokiQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserContext userContext) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<AlbumWithVokiPresenceDto[]>> Handle(
        ListAlbumsDataForVokiQuery query, CancellationToken ct
    ) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        VokiAlbum[] albums = await _vokiAlbumsRepository.ListAlbumsForUserAsNoTracking(userId, ct);
        return albums
            .Select(a => AlbumWithVokiPresenceDto.FromAlbum(a, query.VokiId))
            .ToArray();
    }
}

public record AlbumWithVokiPresenceDto(
    VokiAlbumId Id,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor,
    DateTime CreationDate,
    bool HasSpecifiedVoki
)
{
    public static AlbumWithVokiPresenceDto FromAlbum(VokiAlbum albumId, VokiId vokiId) => new(
        albumId.Id,
        albumId.Name,
        albumId.Icon,
        albumId.MainColor,
        albumId.SecondaryColor,
        albumId.CreationDate,
        albumId.HasVoki(vokiId)
    );
}