type AuthorViewData = {
    banner: AuthorBanner;
    displayName: string;
    uniqueName: string;
    profilePic: string;
    pronouns: string;
    showPronouns: boolean;
    status: string;
    about: string;
    links: AuthorLink[];
    favouriteTags: string[];
    showFavouriteTags: boolean;
    faouriteAuthorIds: string[];
    showFavouriteAuthors: boolean;
}
type AuthorLink = {
    value: string;
    type: AuthorLinkType;
}
type AuthorLinkType = "website" | "twitter" | "instagram" | "youtube" | "tiktok" | "other";

type AuthorBanner = BannerFillColor | BannerDefault;
type BannerDefault = {
    type: "default";
}
type BannerFillColor = {
    type: "fill-color";
    color: string;
}