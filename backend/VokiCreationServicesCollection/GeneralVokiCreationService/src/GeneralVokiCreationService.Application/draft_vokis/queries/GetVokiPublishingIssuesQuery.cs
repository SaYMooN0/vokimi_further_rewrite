using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiPublishingIssuesQuery(VokiId VokiId) :
    IQuery<ImmutableArray<VokiPublishingIssue>>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiPublishingIssuesQueryHandler : IQueryHandler<GetVokiPublishingIssuesQuery,
    ImmutableArray<VokiPublishingIssue>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GetVokiPublishingIssuesQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiPublishingIssue>>> Handle(
        GetVokiPublishingIssuesQuery query,
        CancellationToken ct
    ) {
        DraftGeneralVoki voki =
            (await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResultsAsNoTracking(query.VokiId, ct))!;
        return voki.CheckForPublishingIssues();
    }
}