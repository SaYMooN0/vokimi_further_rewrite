﻿using AuthService.Application.common.repositories;
using Microsoft.Extensions.Logging;
using SharedKernel.auth;

namespace AuthService.Application.app_users.queries;

public sealed record PingUserAuthQuery() : IQuery<PingUserAuthQueryResult>;

internal sealed class PingUserAuthQueryHandler : IQueryHandler<PingUserAuthQuery, PingUserAuthQueryResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;
    private Logger<PingUserAuthQueryHandler> _logger;

    public PingUserAuthQueryHandler(
        IAppUsersRepository appUsersRepository,
        IUserContext userContext,
        Logger<PingUserAuthQueryHandler> logger
    ) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<ErrOr<PingUserAuthQueryResult>> Handle(PingUserAuthQuery query, CancellationToken ct) {
        var idFromToken = _userContext.UserIdFromToken();
        if (idFromToken.IsErr()) {
            return PingUserAuthQueryResult.Unauthenticated();
        }

        AppUserId userId = idFromToken.AsSuccess();
        bool doesUserExist = await _appUsersRepository.AnyUserWithId(userId);
        if (!doesUserExist) {
            _logger.LogError("User with correct userId in token ({1}) was not found in the database", userId);
            return PingUserAuthQueryResult.Unauthenticated();
        }

        return PingUserAuthQueryResult.Authenticated(userId);
    }
}

public sealed class PingUserAuthQueryResult
{
    private PingUserAuthQueryResult(AppUserId? userId) {
        UserId = userId;
    }

    private AppUserId? UserId { get; }
    public static PingUserAuthQueryResult Authenticated(AppUserId userId) => new(userId);
    public static PingUserAuthQueryResult Unauthenticated() => new(null);

    public T Switch<T>(Func<AppUserId, T> authenticated, Func<T> unauthenticated) {
        if (UserId is not null) {
            return authenticated(UserId);
        }

        return unauthenticated();
    }
}