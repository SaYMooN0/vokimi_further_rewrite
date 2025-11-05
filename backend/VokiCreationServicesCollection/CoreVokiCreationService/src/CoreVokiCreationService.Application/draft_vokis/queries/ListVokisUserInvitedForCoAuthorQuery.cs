using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisUserInvitedForCoAuthorQuery() :
    IQuery<DraftVoki[]>;

internal sealed class ViewVokiAsInvitedForCoAuthorQueryHandler : IQueryHandler<ListVokisUserInvitedForCoAuthorQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
private readonly IUserContext _userContext;
    
    public ViewVokiAsInvitedForCoAuthorQueryHandler(IDraftVokiRepository draftVokiRepository, IUserContext userContext) {
        _draftVokiRepository = draftVokiRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(ListVokisUserInvitedForCoAuthorQuery query, CancellationToken ct) {
        return await _draftVokiRepository.ListByIdWhereUserIsInvitedForCoAuthorAsNoTracking(_userContext.AuthenticatedUserId);
    }
}