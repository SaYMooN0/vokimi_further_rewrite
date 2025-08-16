using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisQuery(VokiId[] VokiIds) :
    IQuery<DraftVoki[]>,
    IWithMultipleVokiAccessValidationStep;

internal sealed class ListVokisQueryHandler : IQueryHandler<ListVokisQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public ListVokisQueryHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(
        ListVokisQuery query, CancellationToken ct
    ) {
        return await _draftVokiRepository.GetMultipleByIdAsNoTracking(query.VokiIds);
    }
}