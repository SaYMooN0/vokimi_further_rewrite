export type UserProfilePreview = {
    id: string;
    uniqueName: string;
    displayName: string;
    profilePic: string;
}
export type AllowCoAuthorInvitesSettingValue = 'Everyone' | 'NoOne';

export type LanguageFlagDisplay = 'Flag' | 'ThreeLetterCode'

export type ProfilePicShape = 'Circle' | 'Squircle' | 'RoundedSquare20' | 'RoundedSquare30' | 'Seal';


export type AuthorBanner = BannerDefault | BannerFillColor;

export type BannerDefault = {
    $type: 'Default';
};

export type BannerFillColor = {
    $type: 'FillColor';
    color: string;
};