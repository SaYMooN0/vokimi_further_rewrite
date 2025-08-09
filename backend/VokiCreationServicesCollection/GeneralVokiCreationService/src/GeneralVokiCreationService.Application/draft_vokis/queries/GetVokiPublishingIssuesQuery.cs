using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiPublishingIssuesQuery(VokiId VokiId) :
    IQuery<ImmutableArray<VokiPublishingIssue>>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiPublishingIssuesQueryHandler : IQueryHandler<GetVokiPublishingIssuesQuery,
    ImmutableArray<VokiPublishingIssue>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiPublishingIssuesQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiPublishingIssue>>> Handle(
        GetVokiPublishingIssuesQuery query,
        CancellationToken ct
    ) {
        DraftGeneralVoki voki =
            (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResultsAsNoTracking(query.VokiId))!;
        return voki.CheckForPublishingIssues();
    }
}