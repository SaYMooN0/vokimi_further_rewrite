namespace VokimiStorageKeysLib;

public record class FileData(
    Stream Stream,
    string Name,
    string ContentType
);