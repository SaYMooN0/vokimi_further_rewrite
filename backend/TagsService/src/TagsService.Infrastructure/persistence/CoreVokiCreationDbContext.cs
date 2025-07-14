using InfrastructureShared.domain_events_publisher;
using Microsoft.EntityFrameworkCore;
using TagsService.Domain.voki_tag_aggregate;

namespace TagsService.Infrastructure.persistence;

public class CoreVokiCreationDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public CoreVokiCreationDbContext(
        DbContextOptions<CoreVokiCreationDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<VokiTag> VokiTags { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreVokiCreationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var domainEvents = ChangeTracker.Entries()
            .Where(e => e.Entity is IAggregateRoot)
            .SelectMany(e => ((IAggregateRoot)e.Entity).PopAndClearDomainEvents())
            .ToList();

        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents) {
        foreach (var domainEvent in domainEvents) {
            await _publisher.Publish(domainEvent);
        }
    }
}