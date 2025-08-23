using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionImagesRequest : IRequestWithValidationNeeded
{
    public string[] NewImages { get; init; }

    public ErrOrNothing Validate() {
        var keysRes = NewImages.Select(TempImageKey.FromString).ToArray();
        var keyErrs = keysRes.Where(r => r.IsErr()).ToArray();
        if (keyErrs.Length > 0) {
            ErrOrNothing errs = ErrOrNothing.Nothing;
            foreach (var e in keyErrs) {
                errs.AddNextIfErr(e);
            }

            return errs;
        }

        ParsedTempKeys = keysRes
            .Select(k => k.AsSuccess())
            .ToArray();
        return ErrOrNothing.Nothing;
    }

    public TempImageKey[] ParsedTempKeys { get; private set; }
}