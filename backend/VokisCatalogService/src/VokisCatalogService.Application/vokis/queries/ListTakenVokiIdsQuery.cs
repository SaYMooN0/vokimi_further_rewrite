using SharedKernel.auth;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListTakenVokiIdsQuery() : IQuery<VokiIdWithLastTakenDateDto[]>;

internal sealed class ListTakenVokiIdsQueryHandler :
    IQueryHandler<ListTakenVokiIdsQuery, VokiIdWithLastTakenDateDto[]>
{
    private readonly IUserContext _userContext;
    private readonly IAppUsersRepository _appUsersRepository;

    public ListTakenVokiIdsQueryHandler(IUserContext userContext, IAppUsersRepository appUsersRepository) {
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<VokiIdWithLastTakenDateDto[]>> Handle(ListTakenVokiIdsQuery query, CancellationToken ct) {
        var userIdOrErr = _userContext.UserIdFromToken();
        if (userIdOrErr.IsErr()) {
            return ErrFactory.AuthRequired("To see your taken Vokis you need to log into your account");
        }

        AppUserId userId = userIdOrErr.AsSuccess();
        AppUser? user = await _appUsersRepository.GetUserWithTakenVokisAsNoTracking(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Cannot find user account", $"User id: {userId}");
        }

        return user.TakenVokis.Value
            .Select(v => new VokiIdWithLastTakenDateDto(v.Key, v.Value.LastTimeTaken))
            .ToArray();
    }
}

public record VokiIdWithLastTakenDateDto(VokiId VokiId, DateTime Date);