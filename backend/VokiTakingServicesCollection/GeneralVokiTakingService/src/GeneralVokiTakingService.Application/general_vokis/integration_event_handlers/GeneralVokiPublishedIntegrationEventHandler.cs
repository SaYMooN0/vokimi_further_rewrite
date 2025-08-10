using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.general_voki.answer_audio;
using VokimiStorageKeysLib.general_voki.answer_image;
using VokimiStorageKeysLib.general_voki.result_image;

namespace GeneralVokiTakingService.Application.vokis.integration_event_handlers;

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
            e.Questions.Select(QuestionFromEventDto).ToImmutableArray(),
            e.Results.Select(ResultFromEventDto).ToImmutableArray(),
            forceSequentialAnswering: e.ForceSequentialAnswering,
            shuffleQuestions: e.ShuffleQuestions
        );
        await _generalVokisRepository.Add(voki);
    }

    private static VokiResult ResultFromEventDto(GeneralVokiResultIntegrationEventDto r) => new(
        r.Id, r.Name, r.Text,
        r.DraftVokiImageKey is null ? null : new GeneralVokiResultImageKey(r.DraftVokiImageKey)
    );

    private static VokiQuestion QuestionFromEventDto(GeneralVokiQuestionIntegrationEventDto q) => new(
        q.Id, q.Text, q.Images, q.AnswersType, q.OrderInVoki,
        q.Answers.Select(a => AnswerFromEventDto(a, q.AnswersType)).ToImmutableArray(),
        q.ShuffleAnswers,
        new QuestionAnswersCountLimit(minAnswers: q.MinAnswersCount, maxAnswers: q.MaxAnswersCount)
    );

    private static VokiQuestionAnswer AnswerFromEventDto(
        GeneralVokiAnswerIntegrationEventDto a,
        GeneralVokiAnswerType type
    ) => new(
        a.Id, a.OrderInQuestion,
        ParseAnswerTypeDataFromDto(type, a.TypeData.Fields),
        a.RelatedResultIds.ToImmutableHashSet()
    );

    private static BaseVokiAnswerTypeData ParseAnswerTypeDataFromDto(
        GeneralVokiAnswerType type, ImmutableDictionary<string, string> f
    ) => type.Match(
        textOnly: () => GeneralVokiAnswerText
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Text])
            .Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.TextOnly(text)),
        //
        imageOnly: () => GeneralVokiAnswerImageKey
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Image])
            .Bind<BaseVokiAnswerTypeData>(image => new BaseVokiAnswerTypeData.ImageOnly(image)),
        //
        imageAndText: () => GeneralVokiAnswerText
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Text])
            .Bind(text => GeneralVokiAnswerImageKey
                .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Image])
                .Bind<BaseVokiAnswerTypeData>(image => new BaseVokiAnswerTypeData.ImageAndText(text, image))),
        //
        colorOnly: () => HexColor
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Color])
            .Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorOnly(color)),
        //
        colorAndText: () => GeneralVokiAnswerText
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Text])
            .Bind(text => HexColor.Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Color])
                .Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorAndText(text, color))),
        //
        audioOnly: () => GeneralVokiAnswerAudioKey
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Audio])
            .Bind<BaseVokiAnswerTypeData>(audio => new BaseVokiAnswerTypeData.AudioOnly(audio)),
        //
        audioAndText: () => GeneralVokiAnswerText
            .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Text])
            .Bind(text => GeneralVokiAnswerAudioKey
                .Create(f[GeneralVokiAnswerTypeDataIntegrationEventDto.Keys.Audio])
                .Bind<BaseVokiAnswerTypeData>(audio => new BaseVokiAnswerTypeData.AudioAndText(text, audio)))
        //
    ).AsSuccess();
}