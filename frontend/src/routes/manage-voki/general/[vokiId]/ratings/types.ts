import type { VokiRatingValue } from "$lib/ts/voki";

export type RatingValueToCountType = {
    [key in VokiRatingValue]: number;
};
export type VokiDailyRatingsSnapshot = {
    Date: Date,
    Distribution: RatingValueToCountType
};