﻿namespace VokimiStorageKeysLib;

public record class FileData(
    Stream Stream,
    string ContentType
);