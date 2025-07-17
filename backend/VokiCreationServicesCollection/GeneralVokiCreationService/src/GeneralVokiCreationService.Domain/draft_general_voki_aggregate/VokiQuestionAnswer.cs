using GeneralVokiCreationService.Domain.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestionAnswer : Entity<GeneralVokiAnswerId>
{
    // private VokiQuestionAnswer() { }
    public BaseVokiAnswerTypeData TypeData { get; private set; }

}