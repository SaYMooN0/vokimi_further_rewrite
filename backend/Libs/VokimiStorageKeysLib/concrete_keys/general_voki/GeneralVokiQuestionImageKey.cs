using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys.general_voki;
public class GeneralVokiQuestionImageKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public GeneralVokiQuestionImageKey(string value)
    {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var vokiId, out var questionId, out var ext)
        );

        VokiId = vokiId;
        QuestionId = questionId;
        ImageExtension = ext;
        Value = value;
    }


    public static ErrOr<GeneralVokiQuestionImageKey> Create(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        ImageFileExtension extension
    )
    {
        var key = $"{KeyConsts.VokisFolder}/{vokiId}/questions/{questionId}/images/{Guid.NewGuid()}.{extension.Value}";
        var validate = Scheme.IsKeyValid(key, out _, out _, out _);
        if (validate.IsErr(out var err))
        {
            return err;
        }

        return new GeneralVokiQuestionImageKey(key);
    }

    public static ErrOr<GeneralVokiQuestionImageKey> Create(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        string extension
    )
    {
        var extOrErr = ImageFileExtension.Create(extension);
        if (extOrErr.IsErr(out var err))
        {
            return err;
        }
        return Create(vokiId, questionId, extOrErr.AsSuccess());
    }

    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId)=>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;

    private static class Scheme
    {
        private const string Template =
            $"{KeyConsts.VokisFolder}/<vokiId:id>/questions/<questionId:id>/images/<version:id>.<ext:imageExt>";

        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(
            string key,
            out VokiId vokiId,
            out GeneralVokiQuestionId questionId,
            out ImageFileExtension ext
        )
        {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err))
            {
                vokiId = default!;
                questionId = default!;
                ext = default;
                return err;
            }

            Dictionary<string, string> parts = parseResult.AsSuccess();

            vokiId = new VokiId(new Guid(parts["vokiId"]));
            questionId = new GeneralVokiQuestionId(new Guid(parts["questionId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}