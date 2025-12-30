import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "./$types";
import type { RatingValueToCountType } from "./types";



export const load: PageServerLoad = async ({ params, fetch }): Promise<{ response: ResponseResult<RatingValueToCountType>, vokiId: string }> => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<SuccessResponseType>(
            fetch, `/vokis/${params.vokiId}/manage/ratings`, { method: 'GET' }
        ).then(r => {
            if (!r.isSuccess) {
                return r;
            }
            return {
                isSuccess: true,
                data: {
                    1: r.data.Rating1Count,
                    2: r.data.Rating2Count,
                    3: r.data.Rating3Count,
                    4: r.data.Rating4Count,
                    5: r.data.Rating5Count
                }
            }

        }),
        vokiId: params.vokiId
    };
};
type SuccessResponseType = {
    Rating1Count: number,
    Rating2Count: number,
    Rating3Count: number,
    Rating4Count: number,
    Rating5Count: number
}