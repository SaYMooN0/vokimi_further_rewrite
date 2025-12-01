import { ApiVokiComments } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "../../$types";
import type { VokiIdToDateDict } from "../../types";

export const load: PageServerLoad<{
    response: ResponseResult<{ vokiIdWithCommentDate: VokiIdToDateDict }>;
}> = async ({ fetch }) => {
    return {
        response: await ApiVokiComments.serverFetchJsonResponse<{ vokiIdWithCommentDate: VokiIdToDateDict }>(
            fetch, `/commented-vokis`, { method: "GET" }
        )
    };
};
