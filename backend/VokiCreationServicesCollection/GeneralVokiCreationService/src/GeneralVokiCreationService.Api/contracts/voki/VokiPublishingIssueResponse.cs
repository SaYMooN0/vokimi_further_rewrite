using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Api.contracts.voki;

public record VokiPublishingIssueResponse(
    PublishingIssueType Type,
    string Message,
    string Source,
    string FixRecommendation
)
{
    public static VokiPublishingIssueResponse Create(VokiPublishingIssue issue) => new(
        issue.Type,
        issue.Message,
        issue.Source,
        issue.FixRecommendation
    );
}