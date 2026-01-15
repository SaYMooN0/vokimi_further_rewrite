using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using System.Collections.Immutable;
using System.Text.Json;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public sealed class VokiQuestionTypeSpecificContentConverter
    : ValueConverter<BaseQuestionTypeSpecificContent, string>
{
    public VokiQuestionTypeSpecificContentConverter()
        : base(
            v => ToString(v),
            s => FromString(s)
        ) { }

    private const string TypeAndJsonDivider = ":";

    private static readonly JsonSerializerOptions JsonOpts = new() {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    private static string ToString(BaseQuestionTypeSpecificContent content) {
        var type = content.AnswersType;
        var json = JsonSerializer.Serialize(ToDbStoreObj(content), JsonOpts);
        return $"{type}{TypeAndJsonDivider}{json}";
    }

    private static BaseQuestionTypeSpecificContent FromString(string value) {
        var parts = value.Split(TypeAndJsonDivider, 2);
        var type = Enum.Parse<GeneralVokiAnswerType>(parts[0]);
        var json = parts[1];
        return FromDbStore(type, json);
    }

    private sealed record TextOnlyQuestionContentDbStoreObj(
        TextOnlyAnswerDbStoreObj[] Answers
    );

    private sealed record TextOnlyAnswerDbStoreObj(
        string Text,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record ImageOnlyQuestionContentDbStoreObj(
        ImageOnlyAnswerDbStoreObj[] Answers
    );

    private sealed record ImageOnlyAnswerDbStoreObj(
        string Image,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record ImageAndTextQuestionContentDbStoreObj(
        ImageAndTextAnswerDbStoreObj[] Answers
    );

    private sealed record ImageAndTextAnswerDbStoreObj(
        string Text,
        string Image,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record ColorOnlyQuestionContentDbStoreObj(
        ColorOnlyAnswerDbStoreObj[] Answers
    );

    private sealed record ColorOnlyAnswerDbStoreObj(
        string Color,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record ColorAndTextQuestionContentDbStoreObj(
        ColorAndTextAnswerDbStoreObj[] Answers
    );

    private sealed record ColorAndTextAnswerDbStoreObj(
        string Text,
        string Color,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record AudioOnlyQuestionContentDbStoreObj(
        AudioOnlyAnswerDbStoreObj[] Answers
    );

    private sealed record AudioOnlyAnswerDbStoreObj(
        string Audio,
        ushort Order,
        Guid[] RelatedResultIds
    );

    private sealed record AudioAndTextQuestionContentDbStoreObj(
        AudioAndTextAnswerDbStoreObj[] Answers
    );

    private sealed record AudioAndTextAnswerDbStoreObj(
        string Text,
        string Audio,
        ushort Order,
        Guid[] RelatedResultIds
    );


    private static object ToDbStoreObj(BaseQuestionTypeSpecificContent content) =>
        content.Match<object>(
            textOnly: c =>
                new TextOnlyQuestionContentDbStoreObj(
                    c.Answers.Select(a => new TextOnlyAnswerDbStoreObj(
                        a.Text.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            imageOnly: c =>
                new ImageOnlyQuestionContentDbStoreObj(
                    c.Answers.Select(a => new ImageOnlyAnswerDbStoreObj(
                        a.Image.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            imageAndText: c =>
                new ImageAndTextQuestionContentDbStoreObj(
                    c.Answers.Select(a => new ImageAndTextAnswerDbStoreObj(
                        a.Text.ToString(),
                        a.Image.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            colorOnly: c =>
                new ColorOnlyQuestionContentDbStoreObj(
                    c.Answers.Select(a => new ColorOnlyAnswerDbStoreObj(
                        a.Color.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            colorAndText: c =>
                new ColorAndTextQuestionContentDbStoreObj(
                    c.Answers.Select(a => new ColorAndTextAnswerDbStoreObj(
                        a.Text.ToString(),
                        a.Color.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            audioOnly: c =>
                new AudioOnlyQuestionContentDbStoreObj(
                    c.Answers.Select(a => new AudioOnlyAnswerDbStoreObj(
                        a.Audio.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                ),
            audioAndText: c =>
                new AudioAndTextQuestionContentDbStoreObj(
                    c.Answers.Select(a => new AudioAndTextAnswerDbStoreObj(
                        a.Text.ToString(),
                        a.Audio.ToString(),
                        a.Order.Value,
                        a.RelatedResultIds.Select(x => x.Value).ToArray()
                    )).ToArray()
                )
        );


    private static BaseQuestionTypeSpecificContent FromDbStore(
        GeneralVokiAnswerType type,
        string json
    ) => type.Match<BaseQuestionTypeSpecificContent>(
        textOnly: () => {
            var dto = JsonSerializer.Deserialize<TextOnlyQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.TextOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.TextOnly(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        imageOnly: () => {
            var dto = JsonSerializer.Deserialize<ImageOnlyQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ImageOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ImageOnly(
                        new GeneralVokiAnswerImageKey(a.Image),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        imageAndText: () => {
            var dto = JsonSerializer.Deserialize<ImageAndTextQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ImageAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ImageAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        new GeneralVokiAnswerImageKey(a.Image),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        colorOnly: () => {
            var dto = JsonSerializer.Deserialize<ColorOnlyQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ColorOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ColorOnly(
                        HexColor.Create(a.Color).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        colorAndText: () => {
            var dto = JsonSerializer.Deserialize<ColorAndTextQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ColorAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ColorAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        HexColor.Create(a.Color).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        audioOnly: () => {
            var dto = JsonSerializer.Deserialize<AudioOnlyQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.AudioOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.AudioOnly(
                        new GeneralVokiAnswerAudioKey(a.Audio),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        audioAndText: () => {
            var dto = JsonSerializer.Deserialize<AudioAndTextQuestionContentDbStoreObj>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.AudioAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.AudioAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        new GeneralVokiAnswerAudioKey(a.Audio),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        }
    );


    private static QuestionAnswersList<TOut> CreateAnswers<TIn, TOut>(
        TIn[] source,
        Func<TIn, TOut> func
    ) where TOut : BaseQuestionAnswer =>
        QuestionAnswersList<TOut>.Create(
            answers: source.Select(func).ToImmutableArray()
        ).AsSuccess();

    private static AnswerOrderInQuestion CreateOrder(ushort order) =>
        AnswerOrderInQuestion.Create(order).AsSuccess();

    private static AnswerRelatedResultIdsSet CreateRelated(Guid[] ids) =>
        AnswerRelatedResultIdsSet.Create(
            ids.Select(id => new GeneralVokiResultId(id)).ToImmutableHashSet()
        ).AsSuccess();
}