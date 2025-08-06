using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.draft_general_voki.result_image;

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
            ParsedImage = null;
            return errs;
        }

        var keyCreationRes = DraftGeneralVokiResultImageKey.FromString(NewImage);
        errs.AddNextIfErr(keyCreationRes);
        if (!errs.IsErr()) {
            ParsedImage = keyCreationRes.AsSuccess();
        }

        return errs;
    }


    public VokiResultName ParsedName => VokiResultName.Create(NewName).AsSuccess();
    public VokiResultText ParsedText => VokiResultText.Create(NewText).AsSuccess();
    public DraftGeneralVokiResultImageKey? ParsedImage { get; private set; }
}