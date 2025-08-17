using AuthService.Infrastructure.persistence;
using CoreVokiCreationService.Infrastructure.persistence;
using DbSeeder;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernel.domain.ids;
using TagsService.Infrastructure.persistence;
using UserProfilesService.Infrastructure.persistence;
using VokisCatalogService.Infrastructure.persistence;

Dictionary<string, Func<Task>> actions = new() {
    ["clear"] = ClearAllDbs,
    ["exit"] = () => {
        Console.WriteLine("Program exit...");
        return Task.CompletedTask;
    }
};
var actionKeys = string.Join(", ", actions.Keys);
Console.WriteLine($"Select action: ({actionKeys})");
var action = Console.ReadLine()!;
await actions[action].Invoke();
return 0;

async Task ClearAllDbs() {
    IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
    DbContext[] dbs = [
        AuthDbContext(config),
        TagsDbContext(config),
        UserProfilesDbContext(config),
        CoreVokiCreationDbContext(config),
        GeneralVokiCreationDbContext(config),
        
        VokisCatalogTakingDbContext(config),
        VokisCatalogTakingDbContext(config),
        GeneralVokiTakingDbContext(config)
    ];
    foreach (var db in dbs) {
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine($"Cleared {db.GetType().Name}");
    }
}

AuthDbContext AuthDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<AuthDbContext>().UseNpgsql(
        config.GetConnectionString("AuthServiceDb")
    ).Options, FakePublisher.Instance
);

TagsDbContext TagsDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<TagsDbContext>().UseNpgsql(
        config.GetConnectionString("TagsServiceDb")
    ).Options, FakePublisher.Instance
);

UserProfilesDbContext UserProfilesDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<UserProfilesDbContext>().UseNpgsql(
        config.GetConnectionString("UserProfilesServiceDb")
    ).Options, FakePublisher.Instance
);

CoreVokiCreationDbContext CoreVokiCreationDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<CoreVokiCreationDbContext>().UseNpgsql(
        config.GetConnectionString("CoreVokiCreationServiceDb")
    ).Options, FakePublisher.Instance
);

GeneralVokiCreationDbContext GeneralVokiCreationDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<GeneralVokiCreationDbContext>().UseNpgsql(
        config.GetConnectionString("GeneralVokiCreationServiceDb")
    ).Options, FakePublisher.Instance
);

VokisCatalogTakingDbContext VokisCatalogTakingDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<VokisCatalogTakingDbContext>().UseNpgsql(
        config.GetConnectionString("VokisCatalogServiceDb")
    ).Options, FakePublisher.Instance
);

GeneralVokiTakingDbContext GeneralVokiTakingDbContext(IConfiguration config) => new(
    new DbContextOptionsBuilder<GeneralVokiTakingDbContext>().UseNpgsql(
        config.GetConnectionString("GeneralVokiTakingServiceDb")
    ).Options, FakePublisher.Instance
);