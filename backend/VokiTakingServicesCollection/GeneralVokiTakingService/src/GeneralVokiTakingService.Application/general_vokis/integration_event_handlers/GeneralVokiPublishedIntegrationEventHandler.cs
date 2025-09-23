using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using static SharedKernel.integration_events.voki_publishing.GeneralVokiAnswerTypeDataIntegrationEventDto;

namespace GeneralVokiTakingService.Application.general_vokis.integration_event_handlers;

public class GeneralVokiPublishedIntegrationEventHandler : IConsumer<GeneralVokiPublishedIntegrationEvent>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;

    public GeneralVokiPublishedIntegrationEventHandler(IGeneralVokisRepository generalVokisRepository) {
        _generalVokisRepository = generalVokisRepository;
    }

    public async Task Consume(ConsumeContext<GeneralVokiPublishedIntegrationEvent> context) {
        var e = context.Message;
        var voki = GeneralVoki.CreateNew(
            e.VokiId,
            new VokiCoverKey(e.Cover),
            name: new VokiName(e.Name),
            e.Questions.Select(QuestionFromEventDto).ToImmutableArray(),
            e.Results.Select(ResultFromEventDto).ToImmutableArray(),
            forceSequentialAnswering: e.ForceSequentialAnswering,
            shuffleQuestions: e.ShuffleQuestions,
            GeneralVokiInteractionSettings.Create(
                e.AuthenticatedOnlyTaking,
                e.ResultsVisibility,
                showResultsDistribution: e.ShowResultsDistribution
            ).AsSuccess()
        );
        await _generalVokisRepository.Add(voki);
    }

    private static VokiResult ResultFromEventDto(GeneralVokiResultIntegrationEventDto r) => new(
        r.Id, r.Name, r.Text,
        r.DraftVokiImageKey is null ? null : new GeneralVokiResultImageKey(r.DraftVokiImageKey)
    );

    private static VokiQuestion QuestionFromEventDto(GeneralVokiQuestionIntegrationEventDto q) => new(
        q.Id, q.Text,
        ImageSetFromEventDto(q.Images, q.ImagesAspectRatio),
        q.AnswersType, q.OrderInVoki,
        q.Answers.Select(a => AnswerFromEventDto(a, q.AnswersType)).ToImmutableArray(),
        q.ShuffleAnswers,
        new QuestionAnswersCountLimit(minAnswers: q.MinAnswersCount, maxAnswers: q.MaxAnswersCount)
    );

    private static VokiQuestionImagesSet ImageSetFromEventDto(string[] images, double aspectRatio) =>
        new(images.Select(key => new GeneralVokiQuestionImageKey(key)).ToImmutableArray(), aspectRatio);

    private static VokiQuestionAnswer AnswerFromEventDto(
        GeneralVokiAnswerIntegrationEventDto a,
        GeneralVokiAnswerType type
    ) => new(
        a.Id, a.OrderInQuestion,
        ParseAnswerTypeDataFromDto(type, a.TypeData.Fields),
        a.RelatedResultIds.ToImmutableHashSet()
    );

    private static BaseVokiAnswerTypeData ParseAnswerTypeDataFromDto(
        GeneralVokiAnswerType type, Dictionary<string, string> f
    ) => type.Match(
        textOnly: () => GeneralVokiAnswerText
            .Create(f[Keys.Text])
            .Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.TextOnly(text)),
        imageOnly: () => new BaseVokiAnswerTypeData.ImageOnly(new(f[Keys.Image])),
        imageAndText: () => GeneralVokiAnswerText
            .Create(f[Keys.Text])
            .Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.ImageAndText(
                text, new GeneralVokiAnswerImageKey(f[Keys.Image])
            )),
        colorOnly: () => HexColor
            .Create(f[Keys.Color])
            .Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorOnly(color)),
        colorAndText: () => GeneralVokiAnswerText
            .Create(f[Keys.Text])
            .Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.ColorAndText(
                text, new HexColor(f[Keys.Color])
            )),
        audioOnly: () => new BaseVokiAnswerTypeData.AudioOnly(new(f[Keys.Audio])),
        audioAndText: () => GeneralVokiAnswerText
            .Create(f[Keys.Text])
            .Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.AudioAndText(
                text, new GeneralVokiAnswerAudioKey(f[Keys.Audio])
            ))
    ).AsSuccess();
}