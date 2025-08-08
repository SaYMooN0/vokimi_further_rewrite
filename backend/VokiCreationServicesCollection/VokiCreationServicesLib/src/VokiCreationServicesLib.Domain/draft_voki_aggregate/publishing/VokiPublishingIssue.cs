namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing_issues;

public sealed class VokiPublishingIssue : ValueObject
{
    public PublishingIssueType IssueType { get; }
    public string Message { get; }
    public string Source { get; }
    public string FixRecommendation { get; }
    public override IEnumerable<object> GetEqualityComponents() => [IssueType, Message, Source, FixRecommendation];

    private VokiPublishingIssue(PublishingIssueType issueType, string message, string source,
        string fixRecommendation) {
        IssueType = issueType;
        Message = message;
        Source = source;
        FixRecommendation = fixRecommendation;
    }

    public static VokiPublishingIssue Error(string message, string source, string fixRecommendation) =>
        new(PublishingIssueType.Error, message, source, fixRecommendation);

    public static VokiPublishingIssue Warning(string message, string source, string fixRecommendation) =>
        new(PublishingIssueType.Warning, message, source, fixRecommendation);
}