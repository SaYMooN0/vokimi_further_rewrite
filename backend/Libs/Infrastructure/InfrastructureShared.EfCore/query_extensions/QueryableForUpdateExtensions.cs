using Microsoft.EntityFrameworkCore;

namespace InfrastructureShared.EfCore.query_extensions;

public static class QueryableForUpdateExtensions
{
    private const string ForUpdateTag = "FOR_UPDATE";

    public static IQueryable<T> ForUpdate<T>(this IQueryable<T> query)
        where T : class
    {
        return query.TagWith(ForUpdateTag);
    }

    public static IQueryable<T> ForUpdateNoWait<T>(this IQueryable<T> query)
        where T : class
    {
        return query.TagWith("FOR_UPDATE_NOWAIT");
    }
}
