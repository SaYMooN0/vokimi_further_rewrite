using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestionAnswer : Entity<GeneralVokiAnswerId>
{
    private VokiQuestionAnswer() { }
    public ushort OrderInQuestion { get; }
    public BaseVokiAnswerTypeData TypeData { get; }
    public ImmutableHashSet<GeneralVokiResultId> RelatedResultIds { get; }
    public VokiQuestionAnswer(
        GeneralVokiAnswerId id,
        ushort orderInQuestion,
        BaseVokiAnswerTypeData typeData,
        ImmutableHashSet<GeneralVokiResultId> relatedResultIds
    ) {
        Id = id;
        OrderInQuestion = orderInQuestion;
        TypeData = typeData;
        RelatedResultIds = relatedResultIds;
    }
}