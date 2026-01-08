using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Application.app_users.queries;

public sealed record ListUserCommentedVokiIdsQuery() :
    IQuery<VokiIdWithLastCommentedDateDto[]>,
    IWithAuthCheckStep;

internal sealed class ListUserCommentedVokiIdsQueryHandler :
    IQueryHandler<ListUserCommentedVokiIdsQuery, VokiIdWithLastCommentedDateDto[]>
{
    private readonly IUserContext _userContext;
    private readonly ICommentsRepository _commentsRepository;

    public ListUserCommentedVokiIdsQueryHandler(IUserContext userContext, ICommentsRepository commentsRepository) {
        _userContext = userContext;
        _commentsRepository = commentsRepository;
    }

    public async Task<ErrOr<VokiIdWithLastCommentedDateDto[]>> Handle(
        ListUserCommentedVokiIdsQuery query, CancellationToken ct
    ) {
        var userIdOrErr = _userContext.UserIdFromToken();
        if (userIdOrErr.IsErr(out var err)) {
            return ErrFactory.AuthRequired("To see your commented Vokis you need to log into your account");
        }

        AppUserId userId = userIdOrErr.AsSuccess();
        return await _commentsRepository.OrderedIdsOfVokiCommentedByUser(userId, ct);
    }
}