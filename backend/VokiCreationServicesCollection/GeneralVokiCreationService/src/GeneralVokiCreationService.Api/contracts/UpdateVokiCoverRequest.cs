using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Api.contracts;

public class UpdateVokiCoverRequest : IRequestWithValidationNeeded
{
    public string NewVokiCover { get; init; }
    public ErrOrNothing Validate() => DraftVokiCoverKey.IsKeyValid(NewVokiCover);

    public DraftVokiCoverKey ParsedCoverKey => new(NewVokiCover);
}