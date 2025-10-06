namespace VokiCommentsService.Application.common.repositories;

public interface ICommentsRepository
{
    Task<VokiIdWithLastCommentedDateDto[]> OrderedIdsOfVokiCommentedByUser(AppUserId userId, CancellationToken ct);

}
public record VokiIdWithLastCommentedDateDto(VokiId VokiId, DateTime Date);
