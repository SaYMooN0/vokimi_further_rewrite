﻿using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Infrastructure.persistence;

public class VokiRatingsDbContext : DbContext
{
    private readonly IDomainEventsPublisher _publisher;

    public VokiRatingsDbContext(
        DbContextOptions<VokiRatingsDbContext> options, IDomainEventsPublisher publisher
    ) : base(options) {
        _publisher = publisher;
    }

    public DbSet<AppUser> AppUsers { get; init; } = null!;
    public DbSet<Voki> Vokis { get; init; } = null!;
    public DbSet<VokiRating> Ratings { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VokiRatingsDbContext).Assembly);
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