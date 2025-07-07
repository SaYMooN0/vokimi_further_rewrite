using GeneralVokiCreationService.Domain.app_user_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using InfrastructureShared.domain_events_publisher;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiCreationService.Infrastructure.persistence;

public class GeneralVokiCreationDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public GeneralVokiCreationDbContext(
        DbContextOptions<GeneralVokiCreationDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }
    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<DraftGeneralVoki> Vokis { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeneralVokiCreationDbContext).Assembly);
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