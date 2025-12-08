import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki-type";

export type VokiPageTab = 'about' | 'comments' | 'ratings';

export type VokiOverviewInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    description: string;
    hasMatureContent: boolean;
    language: Language;
    tags: string[];
    publicationDate: Date;
    ratingsCount: number;
    commentsCount: number;
    signedInOnlyTaking: boolean;
};

export type RatingsTabDataType =
    | { state: 'empty' }
    | { state: 'loading' }
    | { state: 'error', errs: Err[] }
    | {
        state: 'fetched',
        averageRating: number,
        allRatings: VokiRatingData[],
        userHasTaken: boolean,
        isAverageOutdated: boolean
    };

export type VokiRatingsWithAverage = { averageRating: number, ratings: VokiRatingData[] };
export type VokiRatingData = { value: number; userId: string; dateTime: Date; };
