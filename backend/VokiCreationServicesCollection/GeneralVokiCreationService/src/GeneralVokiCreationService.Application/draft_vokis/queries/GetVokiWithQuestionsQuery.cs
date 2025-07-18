using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiWithQuestionsQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiWithQuestionsQueryHandler 
    : IQueryHandler<GetVokiWithQuestionsQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiWithQuestionsQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiWithQuestionsQuery query, CancellationToken ct) {
        return (await _draftGeneralVokiRepository.GetWithQuestionsAsNoTracking(query.VokiId))!;
    }
}