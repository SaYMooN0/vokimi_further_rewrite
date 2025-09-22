using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.dtos;
using SharedKernel.auth;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public sealed record ViewAllVokiResultsQuery(VokiId VokiId) : IQuery<ViewAllVokiResultsQueryResult>;

internal sealed class ViewAllVokiResultsQueryHandler : IQueryHandler<ViewAllVokiResultsQuery, ViewAllVokiResultsQueryResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IGeneralVokiTakenRecordsRepository _generalVokiTakenRecordsRepository;


    public ViewAllVokiResultsQueryHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserContext userContext,
        IAppUsersRepository appUsersRepository,
        IGeneralVokiTakenRecordsRepository generalVokiTakenRecordsRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
        _generalVokiTakenRecordsRepository = generalVokiTakenRecordsRepository;
    }


    public async Task<ErrOr<ViewAllVokiResultsQueryResult>> Handle(
        ViewAllVokiResultsQuery query, CancellationToken ct
    ) {
        var voki = await _generalVokisRepository.GetWithResultsByIdAsNoTracking(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        return (await GetVokiResultsWithDistribution(voki, ct)).Bind<ViewAllVokiResultsQueryResult>(
            results => new ViewAllVokiResultsQueryResult(
                results.ToImmutableArray(),
                voki.ShowResultsDistribution,
                voki.ResultsVisibility
            )
        );
    }

    private async Task<ErrOr<IEnumerable<VokiResultWithDistributionPercent>>> GetVokiResultsWithDistribution(
        GeneralVoki voki, CancellationToken ct
    ) {
        var resultIdsToCount = await _generalVokiTakenRecordsRepository
            .GetResultIdsToCountForVoki(voki.Id, ct);

        if (_userContext.UserIdFromToken().IsSuccess(out var userId)) {
            var user = await _appUsersRepository.GetById(userId, ct);
            if (user is null) {
                return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
            }

            return voki.AllResultsWithDistributionForAuthenticatedUser(
                user.ReceivedResultIds,
                resultIdsToCount
            );
        }

        return voki.AllResultsWithDistributionForNonAuthenticatedUser(
            resultIdsToCount
        );
    }
}

public sealed record ViewAllVokiResultsQueryResult(
    ImmutableArray<VokiResultWithDistributionPercent> Results,
    bool ShowResultsDistribution,
    GeneralVokiResultsVisibility ResultsVisibility
);