using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestionAnswer : Entity<GeneralVokiAnswerId>
{
    private VokiQuestionAnswer() { }

    public ushort OrderInQuestion { get; private set; }
    public BaseVokiAnswerTypeData TypeData { get; private set; }
    public ImmutableHashSet<GeneralVokiResultId> RelatedResultIds { get; private set; }
    public static VokiQuestionAnswer CreateNew(BaseVokiAnswerTypeData typeData, ushort orderInQuestion) => new() {
        Id= GeneralVokiAnswerId.CreateNew(),
        OrderInQuestion = orderInQuestion,
        TypeData = typeData,
        RelatedResultIds = []
    };

    public void UpdateTypeData(BaseVokiAnswerTypeData typeData) {
        TypeData = typeData;
    }

    public void UpdateOrder(ushort orderInQuestion) {
        OrderInQuestion = orderInQuestion;
    }
}