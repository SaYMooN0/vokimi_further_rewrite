﻿namespace VokimiStorageService.s3_storage.storage_service;

public interface IStorageService
{
    public Task<ErrOr<TempImageKey>> PutTempImageFile(FileData data, CancellationToken ct);
}