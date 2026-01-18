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
        await db.LockByIdAsync<T, TId>(id, ct);

        return await db.Set<T>()
            .AsTracking()
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
        await db.LockByIdAsync<T, TId>(id, ct);
        var dbSetWithIncludes = includes(db.Set<T>());
        return await dbSetWithIncludes
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }


    public static async Task LockByIdAsync<T, TId>(
        this DbContext db,
        TId id,
        CancellationToken ct
    )
        where T : AggregateRoot<TId>
        where TId : ValueObject, IEntityId {
        await db.Set<T>()
            .TagWith(ForUpdateTagValue)
            .Where(e => e.Id == id)
            .Select(_ => 1)
            .FirstOrDefaultAsync(ct);
    }
}