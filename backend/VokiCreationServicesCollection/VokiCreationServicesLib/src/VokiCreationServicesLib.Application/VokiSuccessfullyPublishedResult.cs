using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using VokimiStorageKeysLib.voki_cover;

namespace VokiCreationServicesLib.Application;

public sealed record VokiSuccessfullyPublishedResult(
    VokiId Id,
    VokiCoverKey Cover,
    VokiName Name
);