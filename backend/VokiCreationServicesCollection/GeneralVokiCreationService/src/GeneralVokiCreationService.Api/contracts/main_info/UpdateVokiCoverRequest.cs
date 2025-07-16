using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Api.contracts.main_info;

public class UpdateVokiCoverRequest : IRequestWithValidationNeeded
{
    public IFormFile? File { get; init; }

    public ErrOrNothing Validate() =>
        File is null ? ErrFactory.NoValue.Common("No file selected") : ErrOrNothing.Nothing;
}