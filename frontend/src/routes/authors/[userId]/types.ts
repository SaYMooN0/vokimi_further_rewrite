import type { Language } from "$lib/ts/language";
import type { IconUtils } from "$lib/ts/utils/icons-utils";


export const AllAuthorPageTabs = ['vokis'] as const;
export type AuthorPageTab = typeof AllAuthorPageTabs[number];

export type AuthorViewData = {
    banner: AuthorBanner;
    displayName: string;
    uniqueName: string;
    profilePic: { key: string, shape: ProfilePicShape };

    knownLanguages: PossiblyHidden<Language[]>;

    pronouns: PossiblyHidden<string>;
    status: PossiblyHidden<string>;
    aboutMe: PossiblyHidden<string>;

    links: PossiblyHidden<AuthorLink[]>;
    favouriteTags: PossiblyHidden<string[]>;
    favouriteAuthorIds: PossiblyHidden<string[]>;
};
export type PossiblyHidden<T> =
    | { showOnProfile: true; value: T }
    | { showOnProfile: false };

export type AuthorLink = {
    value: string;
    type: IconUtils.AuthorProfileLinkType;
};

