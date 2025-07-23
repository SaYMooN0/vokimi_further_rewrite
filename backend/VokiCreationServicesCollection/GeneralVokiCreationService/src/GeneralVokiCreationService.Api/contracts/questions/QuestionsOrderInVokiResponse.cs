
namespace GeneralVokiCreationService.Api.contracts.questions;

public class QuestionsOrderInVokiResponse(
    Dictionary<string, ushort> QuestionsOrder
)
{
    public static QuestionsOrderInVokiResponse Create(ImmutableDictionary<GeneralVokiQuestionId, ushort> questionIds) =>
        new(
            questionIds.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value)
        );
}