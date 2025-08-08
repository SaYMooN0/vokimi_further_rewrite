using SharedKernel.exceptions;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing_issues;

namespace GeneralVokiCreationService.Api.contracts.voki;

public record class VokiPublishingIssuesResponse(
    VokiSinglePublishingIssueResponse[] Errors,
    VokiSinglePublishingIssueResponse[] Warnings
)
{
    public static VokiPublishingIssuesResponse Create(ImmutableArray<VokiPublishingIssue> issues) {
        List<VokiSinglePublishingIssueResponse> errors = [];
        List<VokiSinglePublishingIssueResponse> warnings = [];
        foreach (var i in issues) {
            if (i.IssueType == PublishingIssueType.Error) {
                errors.Add(VokiSinglePublishingIssueResponse.Create(i));
            }
            else if (i.IssueType == PublishingIssueType.Warning) {
                warnings.Add(VokiSinglePublishingIssueResponse.Create(i));
            }
            else {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                    $"Invalid publishing issue type: {i.IssueType}"
                ));
            }
        }

        return new VokiPublishingIssuesResponse(errors.ToArray(), warnings.ToArray());
    }
}

public record VokiSinglePublishingIssueResponse(
    string Message,
    string Source,
    string FixRecommendation
)
{
    public static VokiSinglePublishingIssueResponse Create(VokiPublishingIssue issue) => new(
        issue.Message,
        issue.Source,
        issue.FixRecommendation
    );
}