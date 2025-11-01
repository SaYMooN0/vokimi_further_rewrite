using ApplicationShared.messaging.pipeline_behaviors;
using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Application.app_users.queries;

public sealed record SearchUsersByNameQuery(
    string SearchValue,
    int Limit
) :
    IQuery<UserPreviewDto[]>,
    IWithBasicValidationStep
{
    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SearchValue)) {
            return ErrFactory.NoValue.Common("No value provided");
        }

        return ErrOrNothing.Nothing;
    }
}

internal sealed class SearchUsersByNameQueryHandler :
    IQueryHandler<SearchUsersByNameQuery, UserPreviewDto[]>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public SearchUsersByNameQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserPreviewDto[]>> Handle(SearchUsersByNameQuery query, CancellationToken ct) =>
        await _appUsersRepository.SearchByNameQuery(query.SearchValue, query.Limit, ct);
}