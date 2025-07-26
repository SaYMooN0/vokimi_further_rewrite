using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokimiStorageKeysLib.draft_general_voki.question_image;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionImagesRequest : IRequestWithValidationNeeded
{
    public string[] NewImages { get; init; }

    public ErrOrNothing Validate() {
        var keysRes = NewImages.Select(DraftGeneralVokiQuestionImageKey.Create).ToArray();
        var keyErrs = keysRes.Where(r => r.IsErr()).ToArray();
        if (keyErrs.Length > 0) {
            ErrOrNothing errs = ErrOrNothing.Nothing;
            foreach (var e in keyErrs) {
                errs.AddNext(e.AsErr());
            }

            return errs;
        }

        var setCreationRes = VokiQuestionImagesSet.Create(
            keysRes
                .Select(k => k.AsSuccess())
                .ToImmutableArray()
        );
        if (setCreationRes.IsErr(out var err)) {
            return err;
        }
        ParsedImagesSet= setCreationRes.AsSuccess();
        ParsedImagesSet= setCreationRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public VokiQuestionImagesSet ParsedImagesSet { get; private set; }
}