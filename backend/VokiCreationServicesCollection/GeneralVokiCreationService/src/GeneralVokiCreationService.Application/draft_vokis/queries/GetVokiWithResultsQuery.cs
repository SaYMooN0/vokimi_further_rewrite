using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiWithResultsQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiWithResultsQueryHandler
    : IQueryHandler<GetVokiWithResultsQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiWithResultsQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiWithResultsQuery query, CancellationToken ct) =>
        (await _draftGeneralVokiRepository.GetWithResultsAsNoTracking(query.VokiId))!;
}