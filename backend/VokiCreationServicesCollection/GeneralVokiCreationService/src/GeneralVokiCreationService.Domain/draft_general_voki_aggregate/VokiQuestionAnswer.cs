using GeneralVokiCreationService.Domain.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answer_type_data;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestionAnswer : Entity<VokiQuestionAnswerId>
{
    // private VokiQuestionAnswer() { }
    public BaseVokiAnswerTypeData TypeData { get; private set; }

}