using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.contracts;

public record class VokiAuthorsInfoResponse(
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] InvitedCoAuthorIds
)
{
    public static VokiAuthorsInfoResponse Create(DraftVoki voki) => new(
        voki.PrimaryAuthorId.ToString(),
        voki.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        voki.InvitedForCoAuthorUserIds.Select(id => id.ToString()).ToArray()
    );
}