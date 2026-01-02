import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "./$types";
import type { ApiDistributionPresentation, RatingValueToCountType } from "./types";



export const load: PageServerLoad = async ({ params, fetch }): Promise<{
    response: ResponseResult<{ distribution: RatingValueToCountType }>,
    vokiId: string
}> => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<ApiDistributionPresentation>(
            fetch, `/vokis/${params.vokiId}/manage/overview`, { method: 'GET' }
        ).then(r => {
            if (!r.isSuccess) {
                return r;
            }
            return {
                isSuccess: true,
                data: {
                    distribution:
                    {
                        1: r.data.Rating1Count,
                        2: r.data.Rating2Count,
                        3: r.data.Rating3Count,
                        4: r.data.Rating4Count,
                        5: r.data.Rating5Count
                    }
                }
            }

        }),
        vokiId: params.vokiId
    };
};