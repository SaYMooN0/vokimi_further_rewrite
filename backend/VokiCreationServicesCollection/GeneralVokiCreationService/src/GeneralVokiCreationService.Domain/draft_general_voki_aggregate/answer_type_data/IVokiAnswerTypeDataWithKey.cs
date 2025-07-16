namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answer_type_data;

public interface IVokiAnswerTypeDataWithKey
{
    public bool IsForCorrectAnswer(
        VokiId expectedVokiId, 
        GeneralVokiQuestionId expectedQuestionId,
        GeneralVokiAnswerId answerId
    );
}