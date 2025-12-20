using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiPublishingDataQuery(VokiId VokiId) :
    IQuery<GetVokiPublishingDataQueryResult>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiPublishingDataQueryHandler :
    IQueryHandler<GetVokiPublishingDataQuery, GetVokiPublishingDataQueryResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiPublishingDataQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<GetVokiPublishingDataQueryResult>> Handle(
        GetVokiPublishingDataQuery query,
        CancellationToken ct
    ) {
        DraftGeneralVoki voki = (
            await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResultsAsNoTracking(query.VokiId, ct)
        )!;
        return new GetVokiPublishingDataQueryResult(
            voki.PrimaryAuthorId,
            voki.CoAuthors,
            voki.GatherAllPublishingIssues()
        );
    }
}

public record GetVokiPublishingDataQueryResult(
    AppUserId PrimaryAuthorId,
    VokiCoAuthorIdsSet CoAuthors,
    ImmutableArray<VokiPublishingIssue> Issues
);