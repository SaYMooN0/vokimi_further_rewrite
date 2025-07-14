using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public GetVokiQueryHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task<ErrOr<DraftVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        DraftVoki? voki = await _draftVokiRepository.GetByIdAsNoTracking(query.VokiId);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki not found", $"Voki with id {query.VokiId} does not exist"
            );
        }

        return voki;
    }
}