using GeneralVokiCreationService.Application.draft_vokis.queries;
using VokiCreationServicesLib.Api.contracts;

namespace GeneralVokiCreationService.Api;

public record VokiPublishingDataResponse(
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    VokiPublishingIssueResponse[] Issues
) : ICreatableResponse<GetVokiPublishingDataQueryResult>
{
    public static ICreatableResponse<GetVokiPublishingDataQueryResult> Create(GetVokiPublishingDataQueryResult res) =>
        new VokiPublishingDataResponse(
            res.PrimaryAuthorId.ToString(),
            res.CoAuthors.ToImmutableHashSet().Select(i => i.ToString()).ToArray(),
            res.Issues.Select(VokiPublishingIssueResponse.Create).ToArray()
        );
}