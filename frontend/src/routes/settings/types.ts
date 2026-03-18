import type { Language } from "$lib/ts/language";
import type { LanguageFlagDisplay, AllowCoAuthorInvitesSettingValue, ProfilePicShape, AuthorBanner } from "$lib/ts/users";
import type { IconUtils } from "$lib/ts/utils/icons-utils";

export type UserSettingsData = {
    uniqueName: string;
    displayName: string;
    profilePic: UserProfilePicData;
    languageSettings: UserLanguageSettingsData;
    favoriteTagsSetting: UserFavoriteTagsSettingData;
    featuredAuthorsSetting: UserFeaturedAuthorsSettingData;
    frontendSettings: UserFrontendSettingsData;
    profileSettings: UserProfileSettingsData;
    socialInteractionSettings: UserSocialInteractionSettingsData;
};

export type UserProfilePicData = {
    key: string;
    shape: ProfilePicShape;
};
export type UnknownLanguagesSettingsValue = "HideAllUnknown" | "HideOnlyBlacklist"
export type UserLanguageSettingsData = {
    showOnProfile: boolean;
    knownLanguages: Language[];
    unknownLanguagesSettingsValue: UnknownLanguagesSettingsValue;
    unknownLanguagesBlacklist: Language[];
};

export type UserFavoriteTagsSettingData = {
    showOnProfile: boolean;
    tags: string[];
};

export type UserFeaturedAuthorsSettingData = {
    showOnProfile: boolean;
    authorIds: string[];
};

export type UserFrontendSettingsData = {
    vokiTakingSettings: UserVokiTakingSettingsData;
    flagsSettings: UserVokiFlagsSettingsData;
};

export type UserVokiTakingSettingsData = {
    general: UserGeneralVokiTakingSettingsData;
};

export type UserGeneralVokiTakingSettingsData = {
    allowArrowNavigationBetweenQuestions: boolean;
    allowArrowNavigationBetweenAnswers: boolean;
};
export type UserVokiFlagsSettingsData = {
    showSignInOnlyFlag: boolean;
    showHasMatureContentFlag: boolean;
    languageDisplay: LanguageFlagDisplay;
};

export type UserProfileSettingsData = {
    banner: AuthorBanner;
    status: UserProfileTextFieldSettingData;
    pronouns: UserProfileTextFieldSettingData;
    aboutMe: UserProfileTextFieldSettingData;
    links: UserLinksSettingData;
};

export type UserProfileTextFieldSettingData = {
    showOnProfile: boolean;
    value: string;
};

export type UserLinksSettingData = {
    showOnProfile: boolean;
    links: UserProfileLinkData[];
};

export type UserProfileLinkData = {
    value: string;
    type: IconUtils.AuthorProfileLinkType;
};

export type UserSocialInteractionSettingsData = {
    allowCoAuthorInvites: AllowCoAuthorInvitesSettingValue;
};
