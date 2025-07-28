using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestionAnswer : Entity<GeneralVokiAnswerId>
{
    private VokiQuestionAnswer(GeneralVokiAnswerId id, BaseVokiAnswerTypeData typeData, ushort orderInQuestion) {
        Id = id;
        TypeData = typeData;
        OrderInQuestion = orderInQuestion;
    }

    public ushort OrderInQuestion { get; private set; }
    public BaseVokiAnswerTypeData TypeData { get; private set; }

    public static VokiQuestionAnswer CreateNew(BaseVokiAnswerTypeData typeData, ushort orderInQuestion) =>
        new(GeneralVokiAnswerId.CreateNew(), typeData, orderInQuestion);

    public void UpdateTypeData(BaseVokiAnswerTypeData typeData) {
        TypeData = typeData;
    }

    public void UpdateOrder(ushort orderInQuestion) {
        OrderInQuestion = orderInQuestion;
    }
}