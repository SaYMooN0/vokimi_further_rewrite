using System.Collections.Immutable;
using SharedKernel.domain.ids;
using SharedKernel.errs;

namespace VokimiStorageKeysLib.draft_general_voki_answers.image;

public class DraftGeneralVokiAnswerImageKeyScheme
{
    public const string Template = "/draft-vokis/{vokiId:id}/{questionId:id}/{answerId:id}/{versionId:id}";
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["jpg", "webp"];
    private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

    public static ErrOrNothing IsKeyValid(
        string key,
        out VokiId vokiId,
        out GeneralVokiQuestionId questionId,
        out GeneralVokiAnswerId answerId
    ) {
        var parseResult = Parser.TryParse(key);
        if (parseResult.IsErr(out var err)) {
            vokiId = default;
            questionId = default;
            answerId = default;
            return err;
        }

        var parts = parseResult.AsSuccess();
        vokiId = new VokiId(new Guid(parts["vokiId"]));
        questionId = new GeneralVokiQuestionId(new Guid(parts["questionId"]));
        answerId = new GeneralVokiAnswerId(new Guid(parts["answerId"]));
        return ErrOrNothing.Nothing;
    }
}