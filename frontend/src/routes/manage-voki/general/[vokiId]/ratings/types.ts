import type { VokiRatingValue } from "$lib/ts/voki";

export type RatingValueToCountType = {
    [key in VokiRatingValue]: number;
};
export type ApiDistributionPresentation = {
    rating1Count: number,
    rating2Count: number,
    rating3Count: number,
    rating4Count: number,
    rating5Count: number,
};
export type VokiDailyRatingsSnapshot = {
    date: Date,
    distribution: RatingValueToCountType
};