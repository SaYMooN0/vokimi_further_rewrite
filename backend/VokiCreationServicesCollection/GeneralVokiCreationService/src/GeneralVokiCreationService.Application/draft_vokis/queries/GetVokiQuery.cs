using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }
    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        return (await _draftGeneralVokiRepository.GetByIdAsNoTracking(query.VokiId))!; 
        // no null check because IWithVokiAccessValidationStep already inculdes it
    }
}