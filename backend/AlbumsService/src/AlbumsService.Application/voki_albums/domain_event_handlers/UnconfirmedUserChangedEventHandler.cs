﻿using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate.events;
using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Application.voki_albums.domain_event_handlers;

internal class VokiAlbumDeletedDomainEventHandler : IDomainEventHandler<VokiAlbumDeletedDomainEvent>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public VokiAlbumDeletedDomainEventHandler(IVokiAlbumsRepository vokiAlbumsRepository) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task Handle(VokiAlbumDeletedDomainEvent e, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetById(e.AlbumId);
        if (album is not null) {
            await _vokiAlbumsRepository.DeleteAlbum(album);
        }
    }
}