using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Api.contracts.voki.update_requests;

public class UpdateVokiCoverRequest : IRequestWithValidationNeeded
{
    public string NewCover { get; init; }
    public ErrOrNothing Validate() => TempImageKey.CheckAndExtractExtension(NewCover, out  _);
}