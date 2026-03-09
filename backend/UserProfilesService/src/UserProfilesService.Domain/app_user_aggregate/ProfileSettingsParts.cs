namespace UserProfilesService.Domain.app_user_aggregate;

public record UserLink(string Title, string Url);


public record ProfileLanguagesSettings(

)
{
    public static ProfileLanguagesSettings Default => new(false, []);
}

public enum LanguageFlagDisplayType
{
    Flag,
    ThreeLetterCode
}

public record VokiFlagsSettings(
    bool ShowSignInOnlyFlag,
    bool ShowHasMatureContentFlag,
    LanguageFlagDisplayType LanguageDisplay
)
{
    public static VokiFlagsSettings Default => new(true, true, LanguageFlagDisplayType.Flag);
}

public record VisualSettings(
    VokiFlagsSettings VokiFlags,
    bool RightLinksTextVisible
)
{
    public static VisualSettings Default => new(VokiFlagsSettings.Default, true);
}

public record GeneralVokiTakingSettings(
    bool AllowArrowNavigationBetweenQuestions,
    bool AllowArrowNavigationBetweenAnswers
)
{
    public static GeneralVokiTakingSettings Default => new(true, true);
}

public record VokiTakingSettings(GeneralVokiTakingSettings General)
{
    public static VokiTakingSettings Default => new(GeneralVokiTakingSettings.Default);
}
