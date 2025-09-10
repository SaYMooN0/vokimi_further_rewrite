namespace GeneralVokiTakingService.Api.contracts;

public class SequentialAnsweringSessionQuestionAnsweredRequest : IRequestWithValidationNeeded
{
    public ushort QuestionOrderInTaking { get; init; }
    public string SessionId { get; init; }
    public string QuestionId { get; init; }
    public string[] ChosenAnswers { get; init; }
    public ErrOrNothing Validate() => throw new NotImplementedException();
}