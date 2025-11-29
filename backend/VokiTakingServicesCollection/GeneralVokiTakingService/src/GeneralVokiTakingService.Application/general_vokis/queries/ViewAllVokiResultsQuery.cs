using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.common.vokis;
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

        Dictionary<GeneralVokiResultId, uint> resultIdsToCount = await _generalVokiTakenRecordsRepository
            .GetResultIdsToCountForVoki(voki.Id, ct);

        if (_userContext.UserIdFromToken().IsSuccess(out var userId)) {
            AppUser? user = await _appUsersRepository.GetById(userId, ct);
            if (user is null) {
                return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
            }

            return AssembleForAuthenticatedUser(voki, resultIdsToCount, user.ReceivedResultIds);
        }

        return AssembleForNonAuthenticatedUser(voki, resultIdsToCount);
    }

    private static ErrOr<ViewAllVokiResultsQueryResult> AssembleForAuthenticatedUser(
        GeneralVoki voki, Dictionary<GeneralVokiResultId, uint> resultIdsToCount, ISet<GeneralVokiResultId> userReceivedResultIds
    ) => voki.AllResultsWithDistributionForAuthenticatedUser(userReceivedResultIds, resultIdsToCount)
        .Bind<ViewAllVokiResultsQueryResult>(
            results => new ViewAllVokiResultsQueryResult(
                results.ToImmutableArray(),
                voki.InteractionSettings.ShowResultsDistribution,
                voki.InteractionSettings.ResultsVisibility,
                voki.Name,
                voki.UserHasTakenThisVoki(userReceivedResultIds)
            )
        );

    private static ErrOr<ViewAllVokiResultsQueryResult> AssembleForNonAuthenticatedUser(
        GeneralVoki voki, Dictionary<GeneralVokiResultId, uint> resultIdsToCount
    ) => voki.AllResultsWithDistributionForNonAuthenticatedUser(resultIdsToCount).Bind<ViewAllVokiResultsQueryResult>(
        results => new ViewAllVokiResultsQueryResult(
            results.ToImmutableArray(),
            voki.InteractionSettings.ShowResultsDistribution,
            voki.InteractionSettings.ResultsVisibility,
            voki.Name,
            HasUserTaken: false
        )
    );
}

public sealed record ViewAllVokiResultsQueryResult(
    ImmutableArray<VokiResultWithDistributionPercent> Results,
    bool ShowResultsDistribution,
    GeneralVokiResultsVisibility ResultsVisibility,
    VokiName VokiName,
    bool HasUserTaken
);