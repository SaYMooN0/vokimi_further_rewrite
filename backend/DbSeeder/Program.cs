using AlbumsService.Infrastructure.persistence;
using AuthService.Infrastructure.persistence;
using CoreVokiCreationService.Infrastructure.persistence;
using DbSeeder;
using DbSeeder.seeding.newtonsoft;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TagsService.Infrastructure.persistence;
using UserProfilesService.Infrastructure.persistence;
using VokiCommentsService.Infrastructure.persistence;
using VokiRatingsService.Infrastructure.persistence;
using VokisCatalogService.Infrastructure.persistence;


IConfiguration appSettingsConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
Dictionary<string, Func<Task>> actions = new() {
    ["clear"] = ClearAllDbs,
    ["add_draft_voki"] = AddDraftVokiFromJson,
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

async Task AddDraftVokiFromJson() {
    Console.WriteLine("Input path to file: ");
    string path = Console.ReadLine()!;
    var jsonString = File.ReadAllText(path);
    string filledJson = VokiJsonPlaceholderFiller.FillPlaceholders(jsonString);

    var settings = new JsonSerializerSettings {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        NullValueHandling = NullValueHandling.Include,
        ContractResolver = new PrivateSetterAndFieldsResolver(),
        TypeNameHandling = TypeNameHandling.Auto
    };
    DraftGeneralVoki voki = JsonConvert.DeserializeObject<DraftGeneralVoki>(filledJson, settings) ?? throw new();
    GeneralVokiCreationDbContext db = GeneralVokiCreationDbContext(appSettingsConfig);
    await db.Database.EnsureCreatedAsync();
    await db.Database.BeginTransactionAsync();
    db.Vokis.Add(voki);
    await db.SaveChangesAsync();
    await db.Database.CommitTransactionAsync();
}

async Task ClearAllDbs() {
    DbContext[] dbs = [
        AuthDbContext(appSettingsConfig),
        TagsDbContext(appSettingsConfig),
        UserProfilesDbContext(appSettingsConfig),
        CoreVokiCreationDbContext(appSettingsConfig),
        GeneralVokiCreationDbContext(appSettingsConfig),
        VokisCatalogDbContext(appSettingsConfig),
        GeneralVokiTakingDbContext(appSettingsConfig),
        VokiRatingsDbContext(appSettingsConfig),
        VokiCommentsDbContext(appSettingsConfig),
        AlbumsDbContext(appSettingsConfig),
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