using CoreVokiCreationService.Application.common.repositories;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListUserVokiIdsQuery() : IQuery<ImmutableArray<VokiId>>;

internal sealed class ListUserVokiIdsQueryHandler : IQueryHandler<ListUserVokiIdsQuery, ImmutableArray<VokiId>>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserContext _userContext;

    public ListUserVokiIdsQueryHandler(IDraftVokiRepository draftVokiRepository, IUserContext userContext) {
        _draftVokiRepository = draftVokiRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<ImmutableArray<VokiId>>> Handle(ListUserVokiIdsQuery query, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        return (await _draftVokiRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(userId)).ToImmutableArray();
    }
}