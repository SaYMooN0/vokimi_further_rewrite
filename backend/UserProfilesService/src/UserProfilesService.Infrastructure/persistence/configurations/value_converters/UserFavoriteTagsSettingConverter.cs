using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserFavoriteTagsSettingConverter : ValueConverter<UserFavoriteTagsSetting, string>
{
    private record Dto(string[] Tags, bool ShowInProfile);

    private static string Serialize(UserFavoriteTagsSetting setting) =>
        JsonSerializer.Serialize(new Dto(
            setting.Tags.Select(t => t.ToString()).ToArray(),
            setting.ShowOnProfile
        ));

    private static UserFavoriteTagsSetting Deserialize(string json) {
        var dto = JsonSerializer.Deserialize<Dto>(json)!;
        var tags = dto.Tags.Select(s => new VokiTagId(s)).ToImmutableHashSet();
        return UserFavoriteTagsSetting.Create(tags, dto.ShowInProfile).AsSuccess();
    }

    public UserFavoriteTagsSettingConverter() : base(
        setting => Serialize(setting),
        json => Deserialize(json)
    ) { }
}
