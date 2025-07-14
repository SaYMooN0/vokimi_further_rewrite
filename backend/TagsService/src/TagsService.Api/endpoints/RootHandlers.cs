using TagsService.Application.voki_tags.queries;

namespace TagsService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");

        group.MapGet("/search", SearchTags);
    }

    private static async Task<IResult> SearchTags(
        CancellationToken ct,
        IQueryHandler<SearchTagsQuery, ImmutableArray<VokiTagId>> handler,
        string searchValue
    ) {
        SearchTagsQuery query = new(searchValue);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (tags) => Results.Json(
            new { Tags = tags.Select(t => t.ToString()).ToArray() }
        ));
    }
}