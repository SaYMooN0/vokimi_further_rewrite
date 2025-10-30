using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.rules;

namespace CoreVokiCreationService.Api.contracts;

public record class VokiAuthorsInfoResponse(
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] InvitedForCoAuthorUserIds,
    DateTime VokiCreationDate,
    int MaxVokiCoAuthors
) : ICreatableResponse<DraftVoki>
{
    public static ICreatableResponse<DraftVoki> Create(DraftVoki voki) => new VokiAuthorsInfoResponse(
        voki.PrimaryAuthorId.ToString(),
        voki.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        voki.InvitedForCoAuthorUserIds.Select(id => id.ToString()).ToArray(),
        voki.CreationDate,
        VokiRules.MaxCoAuthors
    );
}