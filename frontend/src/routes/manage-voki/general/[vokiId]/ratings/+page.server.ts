import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "./$types";
import type { ApiDistributionPresentation, RatingValueToCountType } from "./types";



export const load: PageServerLoad = async ({ params, fetch }): Promise<{
    response: ResponseResult<{ distribution: ApiDistributionPresentation }>,
    vokiId: string
}> => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<{ distribution: ApiDistributionPresentation }>(
            fetch, `/vokis/${params.vokiId}/manage/overview`, { method: 'GET' }
        ),
        vokiId: params.vokiId
    };
};