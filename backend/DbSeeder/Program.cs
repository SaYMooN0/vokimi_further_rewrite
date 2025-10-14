using AlbumsService.Infrastructure.persistence;
using AuthService.Infrastructure.persistence;
using CoreVokiCreationService.Infrastructure.persistence;
using DbSeeder;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TagsService.Infrastructure.persistence;
using UserProfilesService.Infrastructure.persistence;
using VokiCommentsService.Infrastructure.persistence;
using VokiRatingsService.Infrastructure.persistence;
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
        VokisCatalogDbContext(config),
        GeneralVokiTakingDbContext(config),
        VokiRatingsDbContext(config),
        VokiCommentsDbContext(config),
        AlbumsDbContext(config),
    ];
    foreach (var db in dbs) {
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine($"Cleared {db.GetType().Name}");
    }
}

AuthDbContext AuthDbContext(IConfiguration config) => new(
    DbOptions<AuthDbContext>(config, "AuthServiceDb"), FakePublisher.Instance
);

TagsDbContext TagsDbContext(IConfiguration config) => new(
    DbOptions<TagsDbContext>(config, "TagsServiceDb"), FakePublisher.Instance
);

UserProfilesDbContext UserProfilesDbContext(IConfiguration config) => new(
    DbOptions<UserProfilesDbContext>(config, "UserProfilesServiceDb"), FakePublisher.Instance
);

CoreVokiCreationDbContext CoreVokiCreationDbContext(IConfiguration config) => new(
    DbOptions<CoreVokiCreationDbContext>(config, "CoreVokiCreationServiceDb"), FakePublisher.Instance
);

GeneralVokiCreationDbContext GeneralVokiCreationDbContext(IConfiguration config) => new(
    DbOptions<GeneralVokiCreationDbContext>(config, "GeneralVokiCreationServiceDb"), FakePublisher.Instance
);

VokisCatalogDbContext VokisCatalogDbContext(IConfiguration config) => new(
    DbOptions<VokisCatalogDbContext>(config, "VokisCatalogServiceDb"), FakePublisher.Instance
);

GeneralVokiTakingDbContext GeneralVokiTakingDbContext(IConfiguration config) => new(
    DbOptions<GeneralVokiTakingDbContext>(config, "GeneralVokiTakingServiceDb"), FakePublisher.Instance
);

VokiRatingsDbContext VokiRatingsDbContext(IConfiguration config) => new(
    DbOptions<VokiRatingsDbContext>(config, "VokiRatingsServiceDb"), FakePublisher.Instance
);

VokiCommentsDbContext VokiCommentsDbContext(IConfiguration config) => new(
    DbOptions<VokiCommentsDbContext>(config, "VokiCommentsServiceDb"), FakePublisher.Instance
);

AlbumsDbContext AlbumsDbContext(IConfiguration config) => new(
    DbOptions<AlbumsDbContext>(config, "AlbumsServiceDb"), FakePublisher.Instance
);


static DbContextOptions<T> DbOptions<T>(
    IConfiguration config,
    string str
) where T : DbContext {
    string connection = config.GetConnectionString(str)
                        ?? throw new NullReferenceException($"Connection string '{str}' is not provided");

    DbContextOptionsBuilder<T> optionsBuilder = new();
    return optionsBuilder.UseNpgsql(connection).Options;
}