using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserLanguageSettingsConverter : ValueConverter<UserLanguageSettings, string>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private record BlacklistDto(string Value, string[] Blacklist);
    private record Dto(string[] KnownLanguages, bool ShowInProfile, BlacklistDto UnknownLanguages);

    private static string Serialize(UserLanguageSettings settings) =>
        JsonSerializer.Serialize(new Dto(
            settings.KnownLanguages.Select(l => l.ToString()).ToArray(),
            settings.ShowOnProfile,
            new BlacklistDto(
                settings.UnknownLanguages.Value.ToString(),
                settings.UnknownLanguages.Blacklist.Select(l => l.ToString()).ToArray()
            )
        ), Options);

    private static UserLanguageSettings Deserialize(string json) {
        var dto = JsonSerializer.Deserialize<Dto>(json, Options)!;
        var knownLanguages = dto.KnownLanguages
            .Select(s => Enum.Parse<Language>(s, true))
            .ToImmutableHashSet();
        var blacklist = dto.UnknownLanguages.Blacklist
            .Select(s => Enum.Parse<Language>(s, true))
            .ToImmutableHashSet();
        var unknownSettings = new UnknownLanguagesSettings(
            Enum.Parse<UnknownLanguagesSettingsValue>(dto.UnknownLanguages.Value, true),
            blacklist
        );
        return UserLanguageSettings.Create(dto.ShowInProfile, knownLanguages, unknownSettings).AsSuccess();
    }

    public UserLanguageSettingsConverter() : base(
        settings => Serialize(settings),
        json => Deserialize(json)
    ) { }
}
