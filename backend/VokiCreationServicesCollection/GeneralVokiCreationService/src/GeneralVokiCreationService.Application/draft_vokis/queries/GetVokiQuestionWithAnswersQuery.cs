using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuestionWithAnswersQuery(VokiId VokiId, GeneralVokiQuestionId QuestionId) :
    IQuery<VokiQuestion>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiQuestionWithAnswersQueryHandler
    : IQueryHandler<GetVokiQuestionWithAnswersQuery, VokiQuestion>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiQuestionWithAnswersQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestion>> Handle(GetVokiQuestionWithAnswersQuery query, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAsNoTracking(query.VokiId))!;
        return voki.QuestionWithId(query.QuestionId);
    }
}