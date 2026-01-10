using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public sealed record ViewVokiResultQuery(VokiId VokiId, GeneralVokiResultId ResultId) : IQuery<ViewVokiResultQueryResult>;

internal sealed class ViewVokiResultQueryHandler : IQueryHandler<ViewVokiResultQuery, ViewVokiResultQueryResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ViewVokiResultQueryHandler(
        IGeneralVokisRepository generalVokisRepository, IUserCtxProvider userCtxProvider, IAppUsersRepository appUsersRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userCtxProvider = userCtxProvider;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<ViewVokiResultQueryResult>> Handle(ViewVokiResultQuery query, CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithResultsById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        ErrOr<VokiResult> resultOrErr = await voki.InteractionSettings.ResultsVisibility.Match(
            anyone: () => Task.FromResult(voki.GetResultToViewByAnyOne(query.ResultId)),
            afterTaking: () => ResultToViewByUserAfterTaking(voki, query.ResultId, ct),
            onlyReceived: () => OnlyReceivedResultToViewByUser(voki, query.ResultId, ct)
        );
        return resultOrErr.Bind<ViewVokiResultQueryResult>(result =>
            new ViewVokiResultQueryResult(result, voki.InteractionSettings.ResultsVisibility, voki.Name, voki.ResultsCount)
        );
    }

    private async Task<ErrOr<VokiResult>> ResultToViewByUserAfterTaking(
        GeneralVoki voki,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) {
        var idOrErr = _userCtxProvider.UserId();
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
        var idOrErr = _userCtxProvider.UserId();
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

public sealed record ViewVokiResultQueryResult(
    VokiResult Result,
    GeneralVokiResultsVisibility ResultsVisibility,
    VokiName VokiName,
    uint TotalResultsCount
);