import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki";

export type VokiPageTab = 'about' | 'comments' | 'ratings';

export type VokiOverviewInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    description: string;
    isAgeRestricted: boolean;
    language: Language;
    tags: string[];
    ratingsCount: number;
    commentsCount: number;
    publicationDate: Date;
};
