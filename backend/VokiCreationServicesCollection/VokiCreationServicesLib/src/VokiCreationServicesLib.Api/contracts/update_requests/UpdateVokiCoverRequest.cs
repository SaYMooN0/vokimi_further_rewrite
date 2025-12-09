using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Api.contracts.update_requests;

public class UpdateVokiCoverRequest : IRequestWithValidationNeeded
{
    public string NewCover { get; init; }
    public ErrOrNothing Validate() => TempImageKey.CheckAndExtractExtension(NewCover, out  _);
}