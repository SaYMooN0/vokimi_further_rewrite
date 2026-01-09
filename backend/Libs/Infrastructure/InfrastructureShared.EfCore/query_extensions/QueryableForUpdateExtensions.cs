using Microsoft.EntityFrameworkCore;

namespace InfrastructureShared.EfCore.query_extensions;

public static class QueryableForUpdateExtensions
{
    internal const string ForUpdateTagValue = "FOR_UPDATE";

    public static IQueryable<T> ForUpdate<T>(this IQueryable<T> query) where T : class
    {
        return query
            .AsTracking()
            .TagWith(ForUpdateTagValue);
    }
    internal const string ForUpdateNoWaitTagValue = "FOR_UPDATE_NOWAIT";

    public static IQueryable<T> ForUpdateNoWait<T>(this IQueryable<T> query) where T : class
    {
        return query
            .TagWith(ForUpdateNoWaitTagValue);
    }
}
