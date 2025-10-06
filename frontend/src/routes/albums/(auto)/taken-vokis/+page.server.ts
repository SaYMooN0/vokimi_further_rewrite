import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { ResponseResult } from "$lib/ts/backend-communication/result-types";
import type { PageServerLoad } from "../../../$types";
import type { VokiIdToDateDict } from "../../types";
export const load: PageServerLoad<{
    albumName: string;
    response: ResponseResult<{ vokiIdWithLastTakenDate: VokiIdToDateDict; }>
}> = async ({ fetch }) => {

    return {
        albumName: "taken",
        response: await ApiVokisCatalog.serverFetchJsonResponse<{ vokiIdWithLastTakenDate: VokiIdToDateDict }>(
            fetch, `/taken-vokis`, { method: 'GET' }
        )
    };
};