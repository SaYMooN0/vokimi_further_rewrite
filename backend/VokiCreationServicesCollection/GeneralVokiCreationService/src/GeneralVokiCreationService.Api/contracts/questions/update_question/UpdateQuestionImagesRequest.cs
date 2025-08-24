using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionImagesRequest : IRequestWithValidationNeeded
{
    public string[] NewImages { get; init; }

    public ErrOrNothing Validate() {
        if (VokiQuestionImagesSet.CheckForErr(NewImages.Length).IsErr(out var err)) {
            return err;
        }

        ErrOrNothing parsingErrs = ErrOrNothing.Nothing;

        var possibleTemp = NewImages
            .Where(k => k.StartsWith(KeyConsts.TempFolder))
            .Select(TempImageKey.FromString)
            .ToArray();
        foreach (var possibleErr in possibleTemp) {
            parsingErrs.AddNextIfErr(possibleErr);
        }

        var possibleSaved = NewImages
            .Where(k => !k.StartsWith(KeyConsts.TempFolder))
            .Select(GeneralVokiQuestionImageKey.FromString)
            .ToArray();
        foreach (var possibleErr in possibleSaved) {
            parsingErrs.AddNextIfErr(possibleErr);
        }

        if (parsingErrs.IsErr(out err)) {
            return err;
        }

        ParsedTempKeys = possibleTemp.Select(t => t.AsSuccess()).ToArray();
        ParsedSavedKeys = possibleSaved.Select(t => t.AsSuccess()).ToArray();
        return parsingErrs;
    }

    public TempImageKey[] ParsedTempKeys { get; private set; }
    public GeneralVokiQuestionImageKey[] ParsedSavedKeys { get; private set; }
}