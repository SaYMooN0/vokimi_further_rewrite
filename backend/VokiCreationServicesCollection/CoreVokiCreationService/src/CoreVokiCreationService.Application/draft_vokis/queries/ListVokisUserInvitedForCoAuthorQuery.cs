using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisUserInvitedForCoAuthorQuery() :
    IQuery<DraftVoki[]>,
    IWithAuthCheckStep;

internal sealed class ViewVokiAsInvitedForCoAuthorQueryHandler
    : IQueryHandler<ListVokisUserInvitedForCoAuthorQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserContext _userContext;

    public ViewVokiAsInvitedForCoAuthorQueryHandler(IDraftVokiRepository draftVokiRepository,
        IUserContext userContext) {
        _draftVokiRepository = draftVokiRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(ListVokisUserInvitedForCoAuthorQuery query, CancellationToken ct) {
        return await _draftVokiRepository.ListVokisWithUserAsInvitedForCoAuthorAsNoTracking(
            new AuthenticatedUserContext(_userContext.AuthenticatedUserId), ct
        );
    }
}