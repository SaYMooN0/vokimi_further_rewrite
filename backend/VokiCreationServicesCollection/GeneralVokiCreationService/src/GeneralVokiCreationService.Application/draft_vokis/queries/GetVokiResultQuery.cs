using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;


public sealed record GetVokiResultQuery(VokiId VokiId, GeneralVokiResultId ResultId) :
    IQuery<VokiResult>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiResultQueryHandler 
    : IQueryHandler<GetVokiResultQuery, VokiResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiResultQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiResult>> Handle(GetVokiResultQuery query, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithResultsAsNoTracking(query.VokiId))!;
        return voki.ResultWithId(query.ResultId);
    }
}