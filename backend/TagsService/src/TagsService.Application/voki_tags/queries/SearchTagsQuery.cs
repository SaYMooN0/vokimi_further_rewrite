using System.Collections.Immutable;

namespace TagsService.Application.voki_tags.queries;

public sealed record SearchTagsQuery(string SearchValue) :
    IQuery<ImmutableArray<VokiTagId>>;

internal sealed class SearchTagsQueryHandler : IQueryHandler<SearchTagsQuery, ImmutableArray<VokiTagId>>
{
    public async Task<ErrOr<ImmutableArray<VokiTagId>>> Handle(SearchTagsQuery query, CancellationToken ct) {
        if (!VokiTagId.IsStringValidTag(query.SearchValue)) {
            return ErrFactory.IncorrectFormat($"'{query.SearchValue}' is not a valid tag");
        }

        return ErrOr<ImmutableArray<VokiTagId>>.Success([new VokiTagId(query.SearchValue)]);
    }
}