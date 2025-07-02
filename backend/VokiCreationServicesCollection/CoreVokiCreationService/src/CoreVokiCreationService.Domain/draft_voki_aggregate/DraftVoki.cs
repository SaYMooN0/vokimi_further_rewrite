using System.Collections.Immutable;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    private AppUserId PrimaryAuthorId { get; }
    private ImmutableHashSet<VokiId> CoAuthorsIds { get; init; }
    //add new co-author invite
    //cancel co-author invite
}