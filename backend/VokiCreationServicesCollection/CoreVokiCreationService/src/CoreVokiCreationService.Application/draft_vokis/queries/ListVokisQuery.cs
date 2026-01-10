using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisQuery(VokiId[] VokiIds) :
    IQuery<DraftVoki[]>,
    IWithAuthCheckStep;

internal sealed class ListVokisQueryHandler : IQueryHandler<ListVokisQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    public ListVokisQueryHandler(IDraftVokiRepository draftVokiRepository, IAppUsersRepository appUsersRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(
        ListVokisQuery query, CancellationToken ct
    ) {
        AppUserId userId = _userCtxProvider.AuthenticatedUserId;
        AppUser user = (await _appUsersRepository.GetById(userId, ct))!;

        var allAccessibleVokiIds = user.InitializedVokiIds
            .Concat(user.CoAuthoredVokiIds)
            .ToHashSet();

        var hasInaccessible = query.VokiIds.Any(id => !allAccessibleVokiIds.Contains(id));
        if (hasInaccessible) {
            return  ErrFactory.NoAccess("You do not have access to one or more of the requested vokis");
        }
        
        return await _draftVokiRepository.GetMultipleById(query.VokiIds, ct);
    }
}