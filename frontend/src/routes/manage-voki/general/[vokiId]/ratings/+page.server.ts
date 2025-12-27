// import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
// import type { PublishedVokiBriefInfo } from "$lib/ts/voki";
import { type ServerLoad } from "@sveltejs/kit";
import type { RatingValueToCountType } from "./types";

export const load: ServerLoad = async ({ fetch }): Promise<{ averageRating: number,  valueToCountDistribution: RatingValueToCountType }> => {
    return {
        averageRating: 4.555555,
        valueToCountDistribution: {
            1: 2,
            2: 9,
            3: 10,
            4: 2,
            5: 8
        },
    }
    // return ApiVokiRatings.serverFetchJsonResponse<{ vokis: PublishedVokiBriefInfo[] }>(
    //     fetch, '/vokis/all', { method: 'GET' }
    // );
};

