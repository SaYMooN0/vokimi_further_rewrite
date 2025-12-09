using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;


public sealed record GetVokiResultQuery(VokiId VokiId, GeneralVokiResultId ResultId) :
    IQuery<VokiResult>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiResultQueryHandler 
    : IQueryHandler<GetVokiResultQuery, VokiResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiResultQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiResult>> Handle(GetVokiResultQuery query, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithResultsAsNoTracking(query.VokiId, ct))!;
        return voki.ResultWithId(query.ResultId);
    }
}