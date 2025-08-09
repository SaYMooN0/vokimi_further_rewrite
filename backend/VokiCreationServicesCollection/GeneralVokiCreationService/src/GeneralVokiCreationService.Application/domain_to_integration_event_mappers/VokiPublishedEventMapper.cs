using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using SharedKernel.integration_events.voki_publishing;
using static SharedKernel.integration_events.voki_publishing.GeneralVokiAnswerTypeDataIntegrationEventDto;

namespace GeneralVokiCreationService.Application.domain_to_integration_event_mappers;

public static class VokiPublishedEventMapper
{
    public static GeneralVokiResultIntegrationEventDto[] ResultIntegrationEventDtoArray(
        ResultDomainEventDto[] results
    ) => results
        .Select(r => new GeneralVokiResultIntegrationEventDto(
            r.Id,
            r.Name.ToString(),
            r.Text.ToString(),
            r.Image?.ToString() ?? null
        ))
        .ToArray();

    public static GeneralVokiQuestionIntegrationEventDto[] QuestionIntegrationEventDtoArray(
        QuestionDomainEventDto[] questions
    ) => questions
        .Select(VokiPublishedEventMapper.QuestionDtoFromQuestion)
        .ToArray();


    private static GeneralVokiQuestionIntegrationEventDto QuestionDtoFromQuestion(QuestionDomainEventDto q) => new(
        q.Id,
        q.Text.ToString(),
        q.Images.Keys.Select(i => i.ToString()).ToArray(),
        q.AnswersType,
        q.OrderInVoki,
        q.Answers.Select(a => new GeneralVokiAnswerIntegrationEventDto(
            a.Id,
            a.OrderInQuestion,
            AnswerEventDtoFromAnswerData(a.TypeData),
            a.RelatedResultIds
        )).ToArray(),
        q.ShuffleAnswers,
        MinAnswersCount: q.AnswersCountLimit.MinAnswers,
        MaxAnswersCount: q.AnswersCountLimit.MaxAnswers
    );

    private static GeneralVokiAnswerTypeDataIntegrationEventDto AnswerEventDtoFromAnswerData(
        BaseVokiAnswerTypeData data
    ) => data.Match(
        textOnly: d => NewDict((Keys.Text, d.Text.ToString())),
        imageOnly: d => NewDict((Keys.Image, d.Image.ToString())),
        imageAndText: d => NewDict(
            (Keys.Text, d.Text.ToString()),
            (Keys.Image, d.Image.ToString())
        ),
        colorOnly: d => NewDict((Keys.Color, d.Color.ToString())),
        colorAndText: d => NewDict(
            (Keys.Text, d.Text.ToString()),
            (Keys.Color, d.Color.ToString())
        ),
        audioOnly: d => NewDict((Keys.Audio, d.Audio.ToString())),
        audioAndText: d => NewDict(
            (Keys.Text, d.Text.ToString()),
            (Keys.Audio, d.Audio.ToString())
        )
    );

    private static GeneralVokiAnswerTypeDataIntegrationEventDto NewDict(
        params (string key, string value)[] items
    ) => new(items.ToImmutableDictionary(
        x => x.key,
        x => x.value
    ));
}