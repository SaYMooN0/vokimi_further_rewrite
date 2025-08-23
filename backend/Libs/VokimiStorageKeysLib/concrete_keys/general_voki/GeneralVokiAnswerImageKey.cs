using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys.general_voki;

public class GeneralVokiAnswerImageKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }
    public GeneralVokiAnswerId AnswerId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public GeneralVokiAnswerImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var vokiId, out var questionId, out var answerId, out var ext)
        );

        VokiId = vokiId;
        QuestionId = questionId;
        AnswerId = answerId;
        ImageExtension = ext;
        Value = value;
    }

    public static ErrOr<GeneralVokiAnswerImageKey> FromString(string value) {
        if (Scheme.IsKeyValid(value, out _, out _, out _, out _).IsErr(out var err)) {
            return err;
        }

        return new GeneralVokiAnswerImageKey(value);
    }

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiQuestionId expectedQuestionId,
        GeneralVokiAnswerId expectedAnswerId
    ) =>
        VokiId == expectedVokiId &&
        QuestionId == expectedQuestionId &&
        AnswerId == expectedAnswerId;

    private static class Scheme
    {
        private const string Template =
            $"{KeyConsts.VokisFolder}/<vokiId:id>/questions/<questionId:id>/answers/<answerId:id>.<ext:imageExt>";

        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(
            string key,
            out VokiId vokiId,
            out GeneralVokiQuestionId questionId,
            out GeneralVokiAnswerId answerId,
            out ImageFileExtension ext
        ) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                vokiId = default!;
                questionId = default!;
                answerId = default!;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            vokiId = new VokiId(new Guid(parts["vokiId"]));
            questionId = new GeneralVokiQuestionId(new Guid(parts["questionId"]));
            answerId = new GeneralVokiAnswerId(new Guid(parts["answerId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}