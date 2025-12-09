using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiWithResultsQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiWithResultsQueryHandler
    : IQueryHandler<GetVokiWithResultsQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiWithResultsQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiWithResultsQuery query, CancellationToken ct) =>
        (await _draftGeneralVokisRepository.GetWithResultsAsNoTracking(query.VokiId, ct))!;
}