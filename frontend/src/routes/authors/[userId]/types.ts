type PossiblyHidden<T> =
    | {
        showOnProfile: true;
        value: T;
    }
    | {
        showOnProfile: false;
    };

type AuthorViewData = {
    banner: AuthorBanner;
    displayName: string;
    uniqueName: string;
    profilePicKey: string;

    pronouns: PossiblyHidden<string>;
    status: PossiblyHidden<string>;
    aboutMe: PossiblyHidden<string>;

    links: PossiblyHidden<AuthorLink[]>;
    favouriteTags: PossiblyHidden<string[]>;
    favouriteAuthorIds: PossiblyHidden<string[]>;
};

type AuthorLink = {
    value: string;
    type: AuthorLinkType;
};

type AuthorLinkType =
    | 'website'
    | 'twitter'
    | 'instagram'
    | 'youtube'
    | 'tiktok'
    | 'other';

type AuthorBanner = BannerDefault | BannerFillColor;

type BannerDefault = {
    $type: 'default';
};

type BannerFillColor = {
    $type: 'fill-color';
    color: string;
};
