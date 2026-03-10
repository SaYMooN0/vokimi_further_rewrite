using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserProfileSettingsConverter : ValueConverter<UserProfileSettings, string>
{
    private static readonly JsonSerializerOptions Options = new() {
        Converters = { new JsonStringEnumConverter() }
    };

    private record BannerDto(string Type, string? Color);

    private record TextFieldDto(bool ShowInProfile, string Value);

    private record LinkDto(string Value, string Type);

    private record LinksSettingDto(bool ShowInProfile, LinkDto[] Links);

    private record Dto(
        BannerDto Banner,
        TextFieldDto Status,
        TextFieldDto Pronouns,
        TextFieldDto AboutMe,
        LinksSettingDto Links
    );

    private static string Serialize(UserProfileSettings settings) =>
        JsonSerializer.Serialize(new Dto(
            Banner: settings.Banner switch {
                UserBanner.FillColor fc => new BannerDto(UserBannerType.FillColor.ToString(), fc.Color.ToString()),
                _ => new BannerDto(UserBannerType.Default.ToString(), null)
            },
            Status: new TextFieldDto(settings.Status.ShowInProfile, settings.Status.Value),
            Pronouns: new TextFieldDto(settings.Pronouns.ShowInProfile, settings.Pronouns.Value),
            AboutMe: new TextFieldDto(settings.AboutMe.ShowInProfile, settings.AboutMe.Value),
            Links: new LinksSettingDto(
                settings.Links.ShowInProfile,
                settings.Links.Links.Select(l => new LinkDto(l.Value, l.Type.ToString())).ToArray()
            )
        ), Options);

    private static UserProfileSettings Deserialize(string json) {
        var dto = JsonSerializer.Deserialize<Dto>(json, Options)!;

        var banner = Enum.Parse<UserBannerType>(dto.Banner.Type, true) switch {
            UserBannerType.FillColor => (UserBanner)new UserBanner.FillColor(new HexColor(dto.Banner.Color!)),
            _ => new UserBanner.Default()
        };

        var links = dto.Links.Links
            .Select(l =>
                UserLink.Create(
                    l.Value,
                    Enum.Parse<UserLinkType>(l.Type, true)
                ).AsSuccess()
            )
            .ToImmutableArray();

        return new UserProfileSettings(
            Banner: banner,
            Status: UserStatus.Create(dto.Status.ShowInProfile, dto.Status.Value).AsSuccess(),
            Pronouns: UserPronouns.Create(dto.Pronouns.ShowInProfile, dto.Pronouns.Value).AsSuccess(),
            AboutMe: UserAboutMe.Create(dto.AboutMe.ShowInProfile, dto.AboutMe.Value).AsSuccess(),
            Links: UserLinksSetting.Create(dto.Links.ShowInProfile, links).AsSuccess()
        );
    }

    public UserProfileSettingsConverter() : base(
        settings => Serialize(settings),
        json => Deserialize(json)
    ) { }
}