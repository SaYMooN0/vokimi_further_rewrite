using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.integration_events.voki_publishing;

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
        .Select(QuestionDtoFromQuestion)
        .ToArray();


    private static GeneralVokiQuestionIntegrationEventDto QuestionDtoFromQuestion(QuestionDomainEventDto q) => new(
        q.Id,
        q.Text.ToString(),
        Images: q.ImageSet.Keys.Select(i => i.ToString()).ToArray(),
        ImagesAspectRatio: q.ImageSet.AspectRatio.GetRatio(),
        q.OrderInVoki,
        ShuffleAnswers: q.ShuffleAnswers,
        MinAnswersCount: q.AnswersCountLimit.MinAnswers,
        MaxAnswersCount: q.AnswersCountLimit.MaxAnswers,
        Content: CreateContentDtoFrom(q.Content)
    );

    private static IQuestionContentIntegrationEventDto CreateContentDtoFrom(
        BaseQuestionTypeSpecificContent content
    ) => content.Match<IQuestionContentIntegrationEventDto>(
        textOnly: c => new TextOnlyQuestionIntegrationEventDto(c.Answers
            .Select(a => new TextOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString()
            ))
            .ToArray()
        ),
        imageOnly: c => new ImageOnlyQuestionIntegrationEventDto(c.Answers
            .Select(a => new ImageOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Image: a.Image.ToString()
            ))
            .ToArray()
        ),
        imageAndText: c => new ImageAndTextQuestionIntegrationEventDto(c.Answers
            .Select(a => new ImageAndTextQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString(),
                Image: a.Image.ToString()
            ))
            .ToArray()
        ),
        colorOnly: c => new ColorOnlyQuestionIntegrationEventDto(c.Answers
            .Select(a => new ColorOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Color: a.Color.ToString()
            ))
            .ToArray()
        ),
        colorAndText: c => new ColorAndTextQuestionIntegrationEventDto(c.Answers
            .Select(a => new ColorAndTextQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString(),
                Color: a.Color.ToString()
            ))
            .ToArray()
        ),
        audioOnly: c => new AudioOnlyQuestionIntegrationEventDto(c.Answers
            .Select(a => new AudioOnlyQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Audio: a.Audio.ToString()
            ))
            .ToArray()
        ),
        audioAndText: c => new AudioAndTextQuestionIntegrationEventDto(c.Answers
            .Select(a => new AudioAndTextQuestionIntegrationEventDto.Answer(
                Id: GeneralVokiAnswerId.CreateNew(),
                Order: a.Order.Value,
                RelatedResultIds: a.RelatedResultIds.ToArray(),
                Text: a.Text.ToString(),
                Audio: a.Audio.ToString()
            ))
            .ToArray()
        )
    );
}