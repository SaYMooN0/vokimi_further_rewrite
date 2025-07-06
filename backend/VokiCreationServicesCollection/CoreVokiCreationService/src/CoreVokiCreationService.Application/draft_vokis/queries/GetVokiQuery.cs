using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftVoki>
{
    public async Task<ErrOr<DraftVoki>> Handle(GetVokiQuery query, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}