using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiPublishingDataQuery(VokiId VokiId) :
    IQuery<GetVokiPublishingDataQueryResult>,
    IWithAuthCheckStep;

internal sealed class GetVokiPublishingDataQueryHandler :
    IQueryHandler<GetVokiPublishingDataQuery, GetVokiPublishingDataQueryResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiPublishingDataQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GetVokiPublishingDataQueryResult>> Handle(
        GetVokiPublishingDataQuery query,
        CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsAndResults(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = query.UserCtx(_userCtxProvider);
        if (!voki.HasUserAccess(aUserCtx)) {
            return ErrFactory.NotFound.Voki("You don't have access to this Voki");
        }

        var issuesOrErr = voki.GatherAllPublishingIssues(aUserCtx);
        if (issuesOrErr.IsErr(out var err)) {
            return err;
        }

        return new GetVokiPublishingDataQueryResult(
            voki.PrimaryAuthorId,
            voki.CoAuthors,
            issuesOrErr.AsSuccess()
        );
    }
}

public record GetVokiPublishingDataQueryResult(
    AppUserId PrimaryAuthorId,
    VokiCoAuthorIdsSet CoAuthors,
    ImmutableArray<VokiPublishingIssue> Issues
);