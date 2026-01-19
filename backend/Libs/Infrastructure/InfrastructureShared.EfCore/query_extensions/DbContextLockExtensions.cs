using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.domain;

namespace InfrastructureShared.EfCore.query_extensions;

public static class DbContextLockExtensions
{
    internal const string ForUpdateTagValue = "FOR_UPDATE";
    public static async Task<T?> FindByIdForUpdateAsync<T, TId>(
        this DbContext db,
        TId id,
        CancellationToken ct
    )
        where T : AggregateRoot<TId>
        where TId : ValueObject, IEntityId {
        return await db.Set<T>()
            .AsTracking()
            .TagWith(ForUpdateTagValue)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }


    public static async Task<T?> FindByIdForUpdateAsync<T, TId>(
        this DbContext db,
        Func<IQueryable<T>, IQueryable<T>> includes,
        TId id,
        CancellationToken ct
    )
        where T : AggregateRoot<TId>
        where TId : ValueObject, IEntityId {
        // includes will cause joins so we first need to lock root with no includes
        await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .Where(e => e.Id == id)
            .Select(_ => 1)
            .FirstOrDefaultAsync(ct);

        var dbSetWithIncludes = includes(db.Set<T>());
        return await dbSetWithIncludes
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public static async Task<T?> FindForUpdateAsync<T>(
        this DbContext db,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct
    ) where T : class, IAggregateRoot {
        return await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .AsTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken: ct);
    }
    public static async Task<T[]> ListForUpdateAsync<T>(
        this DbContext db,
        Expression<Func<T, bool>> predicate,
        CancellationToken ct
    ) where T : class, IAggregateRoot {
        return await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .AsTracking()
            .Where(predicate)
            .ToArrayAsync(cancellationToken: ct);
    }
}