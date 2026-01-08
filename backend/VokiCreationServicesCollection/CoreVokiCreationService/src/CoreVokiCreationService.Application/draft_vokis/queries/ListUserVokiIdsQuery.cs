using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListUserVokiIdsQuery() :
    IQuery<ImmutableArray<VokiId>>,
    IWithAuthCheckStep;

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
        return (await _draftVokiRepository.ListVokiAuthoredByUserIdOrderByCreationDate(userId, ct)).ToImmutableArray();
    }
}