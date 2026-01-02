import type { VokiRatingValue } from "$lib/ts/voki";

export type RatingValueToCountType = {
    [key in VokiRatingValue]: number;
};
export type ApiDistributionPresentation = {
    Rating1Count: number,
    Rating2Count: number,
    Rating3Count: number,
    Rating4Count: number,
    Rating5Count: number,
};
export type VokiDailyRatingsSnapshot = {
    date: Date,
    distribution: RatingValueToCountType
};