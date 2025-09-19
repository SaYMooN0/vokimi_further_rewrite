using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.auth;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public sealed record ViewVokiResultQuery(VokiId VokiId, GeneralVokiResultId ResultId) : IQuery<VokiResult>;

internal sealed class ViewVokiResultQueryHandler : IQueryHandler<ViewVokiResultQuery, VokiResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;

    public ViewVokiResultQueryHandler(
        IGeneralVokisRepository generalVokisRepository, IUserContext userContext, IAppUsersRepository appUsersRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<VokiResult>> Handle(ViewVokiResultQuery query, CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithResultsByIdAsNoTracking(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        return await GetResultToViewByUser(voki, query.ResultId, ct);
    }

    private Task<ErrOr<VokiResult>> GetResultToViewByUser(GeneralVoki voki, GeneralVokiResultId resultId, CancellationToken ct) =>
        voki.ResultsVisibility.Match(
            anyone: () => Task.FromResult(voki.GetResultToViewByAnyOne(resultId)),
            afterTaking: () => ResultToViewByUserAfterTaking(voki, resultId, ct),
            onlyReceived: () => OnlyReceivedResultToViewByUser(voki, resultId, ct)
        );

    private async Task<ErrOr<VokiResult>> ResultToViewByUserAfterTaking(
        GeneralVoki voki,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) {
        var idOrErr = _userContext.UserIdFromToken();
        if (idOrErr.IsErr(out _)) {
            return ErrFactory.NoAccess("To see this voki results you need to login and take this voki at least once");
        }

        AppUser? user = await _appUsersRepository.GetById(idOrErr.AsSuccess(), ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
        }

        return voki.GetResultToViewByUserAfterTaking(resultId, user.ReceivedResultIds);
    }

    private async Task<ErrOr<VokiResult>> OnlyReceivedResultToViewByUser(
        GeneralVoki voki,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) {
        var idOrErr = _userContext.UserIdFromToken();
        if (idOrErr.IsErr(out _)) {
            return ErrFactory.NoAccess("To see this voki results you need to login and take this voki at least once");
        }

        AppUser? user = await _appUsersRepository.GetById(idOrErr.AsSuccess(), ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
        }

        return voki.GetOnlyReceivedResultToViewByUser(resultId, user.ReceivedResultIds);
    }
}