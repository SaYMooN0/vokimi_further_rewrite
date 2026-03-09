namespace UserProfilesService.Domain.app_user_aggregate;


public abstract record UserBanner
{
    public abstract UserBannerType Type { get; }
    public static UserBanner DefaultBanner => new Default();

    public record Default : UserBanner
    {
        public override UserBannerType Type => UserBannerType.Default;
    };
    public record FillColor(HexColor Color) : UserBanner
    {
        public override UserBannerType Type => UserBannerType.FillColor;
    };
}
public enum UserBannerType
{
    Default,
    FillColor
}