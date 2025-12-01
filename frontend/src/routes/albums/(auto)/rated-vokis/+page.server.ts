import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "../../$types";
import type { VokiIdToDateDict } from "../../types";

export const load: PageServerLoad<{
    response: ResponseResult<{ vokiIdWithRatingDate: VokiIdToDateDict }>;
}> = async ({ fetch }) => {
    return {
        response: await ApiVokiRatings.serverFetchJsonResponse<{ vokiIdWithRatingDate: VokiIdToDateDict }>(
            fetch, `/rated-vokis`, { method: "GET" }
        )
    };
};
