﻿namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

public sealed class VokiPublishingIssue : ValueObject
{
    public PublishingIssueType Type { get; }
    public string Message { get; }
    public string Source { get; }
    public string FixRecommendation { get; }
    public override IEnumerable<object> GetEqualityComponents() => [Type, Message, Source, FixRecommendation];

    private VokiPublishingIssue(PublishingIssueType type, string message, string source,
        string fixRecommendation) {
        Type = type;
        Message = message;
        Source = source;
        FixRecommendation = fixRecommendation;
    }

    public static VokiPublishingIssue Problem(string message, string source, string fixRecommendation) =>
        new(PublishingIssueType.Problem, message, source, fixRecommendation);

    public static VokiPublishingIssue Warning(string message, string source, string fixRecommendation) =>
        new(PublishingIssueType.Warning, message, source, fixRecommendation);
}