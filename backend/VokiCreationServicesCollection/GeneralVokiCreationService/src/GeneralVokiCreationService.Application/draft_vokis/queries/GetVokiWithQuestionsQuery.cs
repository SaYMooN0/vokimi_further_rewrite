using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiWithQuestionsQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiWithQuestionsQueryHandler 
    : IQueryHandler<GetVokiWithQuestionsQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiWithQuestionsQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiWithQuestionsQuery query, CancellationToken ct) {
        return (await _draftGeneralVokisRepository.GetWithQuestionsAsNoTracking(query.VokiId, ct))!;
    }
}