using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserFeaturedAuthorsSettingConverter : ValueConverter<UserFeaturedAuthorsSetting, string>
{
    private record Dto(Guid[] UserIds, bool ShowInProfile);

    private static string Serialize(UserFeaturedAuthorsSetting setting) =>
        JsonSerializer.Serialize(new Dto(
            setting.UserIds.Select(id => id.Value).ToArray(),
            setting.ShowOnProfile
        ));

    private static UserFeaturedAuthorsSetting Deserialize(string json) {
        var dto = JsonSerializer.Deserialize<Dto>(json)!;
        var userIds = dto.UserIds.Select(g => new AppUserId(g)).ToImmutableHashSet();
        return UserFeaturedAuthorsSetting.Create(userIds, dto.ShowInProfile).AsSuccess();
    }

    public UserFeaturedAuthorsSettingConverter() : base(
        setting => Serialize(setting),
        json => Deserialize(json)
    ) { }
}
