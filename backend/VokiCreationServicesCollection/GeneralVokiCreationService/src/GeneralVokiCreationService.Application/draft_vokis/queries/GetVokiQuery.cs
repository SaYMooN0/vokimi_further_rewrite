using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        return (await _draftGeneralVokisRepository.GetById(query.VokiId, ct))!;
        // no null check because IWithVokiAccessValidationStep already includes it
    }
}