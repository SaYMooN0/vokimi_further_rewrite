using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Api.contracts.results;

public class UpdateVokiResultRequest : IRequestWithValidationNeeded
{
    public string NewName { get; init; }
    public string NewText { get; init; }
    public string? NewImage { get; init; }

    public ErrOrNothing Validate() {
        var errs = VokiResultName.CheckForErr(NewName)
            .WithNextIfErr(VokiResultText.CheckForErr(NewText));
        if (NewImage is null) {
            ParsedImageKey = null;
            return errs;
        }

        ErrOr<TempImageKey> keyCreationRes = TempImageKey.FromString(NewImage);
        errs.AddNextIfErr(keyCreationRes);
        if (!errs.IsErr()) {
            ParsedImageKey = keyCreationRes.AsSuccess();
        }

        return errs;
    }


    public VokiResultName ParsedName => VokiResultName.Create(NewName).AsSuccess();
    public VokiResultText ParsedText => VokiResultText.Create(NewText).AsSuccess();
    public TempImageKey? ParsedImageKey { get; private set; }
}