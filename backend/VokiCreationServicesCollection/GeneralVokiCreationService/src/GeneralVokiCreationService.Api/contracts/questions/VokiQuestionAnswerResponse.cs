using GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.questions;

internal record class VokiQuestionAnswerResponse(
    string Id,
    ushort Order,
    VokiAnswerTypeDataDto TypeData,
    string[] RelatedResultIds
) : ICreatableResponse<VokiQuestionAnswer>
{
    public static VokiQuestionAnswerResponse FromAnswer(VokiQuestionAnswer answer) => new (
        answer.Id.ToString(),
        answer.OrderInQuestion,
        VokiAnswerTypeDataDto.FromAnswerData(answer.TypeData),
        answer.RelatedResultIds.Select(id => id.ToString()).ToArray()
    );

    public static ICreatableResponse<VokiQuestionAnswer> Create(VokiQuestionAnswer answer) => FromAnswer(answer);
}