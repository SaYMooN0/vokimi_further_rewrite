import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "./$types";
import type { RatingValueToCountType } from "./types";



export const load: PageServerLoad = async ({ params, fetch }): Promise<{ response: ResponseResult<SuccessFrontendDataType>, vokiId: string }> => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<ApiSuccessResponseType>(
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
                    },
                    vokiPublicationDate: r.data.vokiPublicationDate
                }
            }

        }),
        vokiId: params.vokiId
    };
};
type ApiSuccessResponseType = {
    Rating1Count: number,
    Rating2Count: number,
    Rating3Count: number,
    Rating4Count: number,
    Rating5Count: number,
    vokiPublicationDate: Date
}
type SuccessFrontendDataType = {
    distribution: RatingValueToCountType
    vokiPublicationDate: Date
}
